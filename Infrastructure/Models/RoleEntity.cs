using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class RoleEntity: IBaseEntity
    {
        public int Id { get; set; }
        public String Code { get; set; }
        public ICollection<UserEntity> Users { get; set; }
    }
}
