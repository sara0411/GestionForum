using ForumRecrutementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForumRecrutementApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Recruteur> Recruteurs { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Administrateur> Administrateurs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Forum>()
                .HasMany(f => f.Recruteurs)
                .WithOne(r => r.Forum)
                .HasForeignKey(r => r.ForumId);

            modelBuilder.Entity<Forum>()
                .HasMany(f => f.Candidats)
                .WithOne(c => c.Forum)
                .HasForeignKey(c => c.ForumId);

            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Candidat)
                .WithMany()
                .HasForeignKey(e => e.CandidatId);

            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Recruteur)
                .WithMany()
                .HasForeignKey(e => e.RecruteurId);


            modelBuilder.Entity<Forum>().HasData(
       new Forum { Id = 1, Nom = "Forum 1" },
       new Forum { Id = 2, Nom = "Forum 2" },
       new Forum { Id = 3, Nom = "Forum 3" });


            // Seed Candidats
            modelBuilder.Entity<Candidat>().HasData(
                new Candidat
                {
                    Id = 1,
                    Nom = "Doe",
                    Prenom = "John",
                    Email = "john.doe@example.com",
                    Competences = "C#, .NET, SQL",
                    CV = "john_doe_cv.pdf",
                    ForumId = 1
                },
                new Candidat
                {
                    Id = 2,
                    Nom = "Smith",
                    Prenom = "Jane",
                    Email = "jane.smith@example.com",
                    Competences = "Java, Spring Boot, Hibernate",
                    CV = "jane_smith_cv.pdf",
                    ForumId = 2
                }

                );
        }
    
    }
}
