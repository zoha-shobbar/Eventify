using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventify.Server.Domain.Entities.Identity;

public partial class Role : IdentityRole<Guid>
{
}

public partial class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(role => role.Name).HasMaxLength(50);
    }
}

