using Domain.Common;

namespace Infrastructure.Models
{
    public class RoleEntity: IBaseEntity
    {
        public int Id { get; set; }
        public String Code { get; set; }
        public ICollection<UserEntity> Users { get; set; }
    }
}
