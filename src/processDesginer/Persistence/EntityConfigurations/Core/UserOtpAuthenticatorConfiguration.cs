using Crea.Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Drawing.Drawing2D;

namespace Persistence.EntityConfigurations.Core;

public class UserOtpAuthenticatorConfiguration : IEntityTypeConfiguration<UserOtpAuthenticator>
{
    public void Configure(EntityTypeBuilder<UserOtpAuthenticator> builder)
    {
        builder.ToTable("UserOtpAuthenticators").HasKey(x => x.Id);
        builder.Property(e => e.Id).HasColumnName("Id");
        builder.Property(e => e.UserId).HasColumnName("UserId");
        builder.Property(e => e.SecretKey).HasColumnName("SecretKey");
        builder.Property(e => e.IsVerified).HasColumnName("IsVerified");
        builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
        builder.HasOne(e => e.User);
    }
}
