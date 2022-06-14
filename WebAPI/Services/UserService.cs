using Application.DTO;
using Domain.Services;
using Infrastructure.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI.Services
{
    public interface IUserService
    {
        Task<JwtAuthResult> Authenticate(LoginDTO model);

        Task Register(RegisterUserRequest model);

        Task<List<RoleRequest>> GetRoles();
    }

    public class UserService: IUserService
    {
        private readonly IRepository _repository;
        private readonly IJwtAuthManager _jwtAuthManager;
        public UserService(IRepository repository, IJwtAuthManager jwtAuthManager)
        {
            _repository = repository;
            _jwtAuthManager = jwtAuthManager;
        }

        public async Task<JwtAuthResult> Authenticate(LoginDTO model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                throw new Exception("Username and/or Password not specified");

            var user = await _repository.SingleAsync<UserEntity>(f1 => f1.UserName == model.UserName);

            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                var role = await _repository.SingleAsync<RoleEntity>(f1 => f1.Id == user.RoleId);
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role.Code),
                };

                return _jwtAuthManager.GenerateTokens(claims);
            }
            else
            {
                throw new Exception("Username or password is incorrect");
            }
        }

        public async Task<List<RoleRequest>> GetRoles()
        {
            return (await _repository.ListAsync<RoleEntity>()).Select(f1 => new RoleRequest () {  Code = f1.Code }).ToList();
        }

        public async Task Register(RegisterUserRequest model)
        {
            var user = await _repository.SingleAsync<UserEntity>(f1 => f1.UserName == model.UserName);

            // validate
            if (user != null)
                throw new Exception("Username '" + model.UserName + "' is already taken");

            var role = await _repository.SingleAsync<RoleEntity>(f1 => f1.Code == model.RoleCode);

            user = new UserEntity()
            {
                RoleId = role.Id,
                UserName = model.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            };

            // save user
            await _repository.AddAsync(user);
        }
    }
}
