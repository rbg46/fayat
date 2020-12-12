
using Fred.Entities.Utilisateur;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fred.EntityFramework
{
    public class AffectationSeuilUtilisateurEntConfiguration : IEntityTypeConfiguration<AffectationSeuilUtilisateurEnt>
    {
        public void Configure(EntityTypeBuilder<AffectationSeuilUtilisateurEnt> builder)
        {
            builder.ToTable("FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE");
            builder.HasKey(asu => asu.AffectationRoleId);

            builder.Property(asu => asu.AffectationRoleId)
                .HasColumnName("UtilisateurRoleOrganisationDeviseId")
                .ValueGeneratedOnAdd();
            builder.Property(asu => asu.CommandeSeuil)
                .HasColumnType("decimal(11, 2)");
            builder.Ignore(asu => asu.IsDeleted);

            builder.HasOne(asu => asu.Organisation).WithMany(o => o.AffectationSeuilUtilisateurs).HasForeignKey(asu => asu.OrganisationId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(asu => asu.Role).WithMany(r => r.AffectationSeuilUtilisateurs).HasForeignKey(asu => asu.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(asu => asu.Devise).WithMany(d => d.AffectationSeuilUtilisateurs).HasForeignKey(asu => asu.DeviseId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(asu => asu.Utilisateur).WithMany(u => u.AffectationsRole).HasForeignKey(asu => asu.UtilisateurId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(asu => asu.Delegation).WithMany().HasForeignKey(asu => asu.DelegationId);
        }
    }
}

