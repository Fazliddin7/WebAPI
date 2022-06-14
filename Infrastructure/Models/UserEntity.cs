using Domain.Common;

namespace Infrastructure.Models
{
    public class UserEntity: IBaseEntity
    {
        public int Id { get; set; }
        public String UserName { get; set; }
        public String PasswordHash { get; set; }
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
    }
}
