using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder
            .HasOne(s => s.Role)
            .WithMany(g => g.Users)
            .HasForeignKey(s => s.RoleId);

            builder.HasIndex(p => p.UserName).IsUnique();
            builder.Property(p => p.UserName).HasMaxLength(256);
            builder.Property(p => p.Password).HasMaxLength(256);

            builder.HasData(new UserEntity[]
            {
                new UserEntity { Id=1, UserName="Moderator", Password="Moderator", RoleId = 1},
                new UserEntity { Id=2, UserName="Client", Password="Client", RoleId = 2},
            });
        }
    }
}
