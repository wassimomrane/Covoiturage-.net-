using Covoiturage.Domain.Entities;
using Covoiturage.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Covoiturage.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Ville> Villes => Set<Ville>();
    public DbSet<Vehicule> Vehicules => Set<Vehicule>();
    public DbSet<Trajet> Trajets => Set<Trajet>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 1. Configuration de l'entité Ville
        builder.Entity<Ville>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Nom).IsRequired().HasMaxLength(100);
            entity.Property(v => v.CodePostal).IsRequired().HasMaxLength(10);
            entity.HasIndex(v => v.Nom);
        });

        // 2. Configuration de l'entité Vehicule
        builder.Entity<Vehicule>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Marque).IsRequired().HasMaxLength(50);
            entity.Property(v => v.Modele).IsRequired().HasMaxLength(50);
            entity.Property(v => v.Immatriculation).IsRequired().HasMaxLength(30);

            entity.HasOne(v => v.Conducteur)
                  .WithMany(u => u.Vehicules)
                  .HasForeignKey(v => v.ConducteurId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 3. Configuration de l'entité Trajet
        builder.Entity<Trajet>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.PrixParPlace).HasPrecision(10, 2);

            // Relation VilleDepart (Restrict pour empêcher la suppression en cascade)
            entity.HasOne(t => t.VilleDepart)
                  .WithMany(v => v.TrajetsDepart)
                  .HasForeignKey(t => t.VilleDepartId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relation VilleArrivee (Restrict pour éviter les cycles de cascade)
            entity.HasOne(t => t.VilleArrivee)
                  .WithMany(v => v.TrajetsArrivee)
                  .HasForeignKey(t => t.VilleArriveeId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relation Conducteur (ApplicationUser)
            entity.HasOne(t => t.Conducteur)
                  .WithMany(u => u.TrajetsConducteur)
                  .HasForeignKey(t => t.ConducteurId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relation Vehicule
            entity.HasOne(t => t.Vehicule)
                  .WithMany(v => v.Trajets)
                  .HasForeignKey(t => t.VehiculeId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Index composite pour optimiser la recherche de trajets par date et villes (Exigence T5 & Backlog)
            entity.HasIndex(t => new { t.VilleDepartId, t.VilleArriveeId, t.DateDepart });
        });

        // 4. Configuration de l'entité Reservation
        builder.Entity<Reservation>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasOne(r => r.Trajet)
                  .WithMany(t => t.Reservations)
                  .HasForeignKey(r => r.TrajetId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Passager)
                  .WithMany(u => u.ReservationsPassager)
                  .HasForeignKey(r => r.PassagerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // 5. Seeding des Villes initiales (Exigence T8)
        builder.Entity<Ville>().HasData(
            new Ville { Id = 1, Nom = "Tunis", CodePostal = "1000" },
            new Ville { Id = 2, Nom = "Sousse", CodePostal = "4000" },
            new Ville { Id = 3, Nom = "Sfax", CodePostal = "3000" },
            new Ville { Id = 4, Nom = "Bizerte", CodePostal = "7000" },
            new Ville { Id = 5, Nom = "Gabès", CodePostal = "6000" }
        );
    }
}