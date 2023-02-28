using Crea.Core.Security.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistence.EntityConfigurations.Core;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.Property(p => p.Id).HasColumnName("Id");
        builder.Property(p => p.UserId).HasColumnName("UserId");
        builder.Property(p => p.Token).HasColumnName("Token");
        builder.Property(p => p.ExpiresDate).HasColumnName("ExpiresDate");
        builder.Property(p => p.RevokedDate).HasColumnName("RevokedDate");
        builder.Property(p => p.ReplacedByToken).HasColumnName("ReplacedByToken");
        builder.Property(p => p.RevokedReason).HasColumnName("RevokedReason");

        builder.Property(p => p.CretedByIp).HasColumnName("CretedByIp");
        builder.Property(p => p.RevokedByIp).HasColumnName("RevokedByIp");
    }
}
