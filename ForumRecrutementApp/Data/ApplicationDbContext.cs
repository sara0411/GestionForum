using ForumRecrutementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
            modelBuilder.Entity<Recruteur>()
          .HasOne(r => r.IdentityUser)
          .WithOne()
          .HasForeignKey<Recruteur>(r => r.IdentityUserId);

            modelBuilder.Entity<Administrateur>()
                .HasOne(a => a.IdentityUser)
                .WithOne()
                .HasForeignKey<Administrateur>(a => a.IdentityUserId);
            /*  var hasher = new PasswordHasher<IdentityUser>();

              modelBuilder.Entity<IdentityUser>().HasData(
                  new IdentityUser
                  {
                      Id = "1",
                      UserName = "john.doe@example.com",
                      NormalizedUserName = "JOHN.DOE@EXAMPLE.COM",
                      Email = "john.doe@example.com",
                      NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
                      EmailConfirmed = true,
                      PasswordHash = hasher.HashPassword(null, "Password123!")
                  },
                  new IdentityUser
                  {
                      Id = "2",
                      UserName = "jane.smith@example.com",
                      NormalizedUserName = "JANE.SMITH@EXAMPLE.COM",
                      Email = "jane.smith@example.com",
                      NormalizedEmail = "JANE.SMITH@EXAMPLE.COM",
                      EmailConfirmed = true,
                      PasswordHash = hasher.HashPassword(null, "Password123!")
                  },
                  new IdentityUser
                  {
                      Id = "3",
                      UserName = "recruiter1@company.com",
                      NormalizedUserName = "RECRUITER1@COMPANY.COM",
                      Email = "recruiter1@company.com",
                      NormalizedEmail = "RECRUITER1@COMPANY.COM",
                      EmailConfirmed = true,
                      PasswordHash = hasher.HashPassword(null, "Password123!")
                  },
                  new IdentityUser
                  {
                      Id = "4",
                      UserName = "admin@forum.com",
                      NormalizedUserName = "ADMIN@FORUM.COM",
                      Email = "admin@forum.com",
                      NormalizedEmail = "ADMIN@FORUM.COM",
                      EmailConfirmed = true,
                      PasswordHash = hasher.HashPassword(null, "Password123!")
                  }
              );
  */
            // Seed Forums
            /* modelBuilder.Entity<Forum>().HasData(
                 new Forum
                 {
                     Id = 1,
                     Nom = "Forum Tech 2025",
                     Date = new DateTime(2025, 3, 15)
                 },
                 new Forum
                 {
                     Id = 2,
                     Nom = "Forum Digital Innovation",
                     Date = new DateTime(2025, 4, 20)
                 },
                 new Forum
                 {
                     Id = 3,
                     Nom = "Forum Startup",
                     Date = new DateTime(2025, 5, 10),
                 }
             );


             // In the Seed Candidats section, modify the existing seeding:
             modelBuilder.Entity<Candidat>().HasData(
                 new Candidat
                 {
                     Id = 1,
                     Nom = "Doe",
                     Prenom = "John",
                     Email = "john.doe@example.com",
                     Competences = "C#, .NET, SQL",
                     CV = "john_doe_cv.pdf",
                     CVFileName = "john_doe_cv.pdf",
                     CVContentType = "application/pdf",
                     CVData = Convert.FromBase64String(GetSamplePdfBase64()), // Method to generate sample PDF bytes
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
                     CVFileName = "jane_smith_cv.pdf",
                     CVContentType = "application/pdf",
                     CVData = Convert.FromBase64String(GetSamplePdfBase64()),
                     ForumId = 2
                 }
             );

             // Seed Recruteurs
             modelBuilder.Entity<Recruteur>().HasData(
                 new Recruteur
                 {
                     Id = 1,
                     IdentityUserId = "3",
                     Entreprise = "Company 1",
                     Nom = "Martin",
                     Email = "recruiter1@company.com",
                     ForumId = 1
                 }
             );

             // Seed Administrateurs
             modelBuilder.Entity<Administrateur>().HasData(
                 new Administrateur
                 {
                     Id = 1,
                     IdentityUserId = "4",
                     Nom = "Admin",
                     Email = "admin@forum.com",
                 }
             );

             // Seed Evaluations
             modelBuilder.Entity<Evaluation>().HasData(
                 new Evaluation
                 {
                     Id = 1,
                     CandidatId = 1,
                     RecruteurId = 1,
                     Note = 4,
                     Commentaire = "Excellent profil technique, bonne communication",
                 },
                 new Evaluation
                 {
                     Id = 2,
                     CandidatId = 2,
                     RecruteurId = 1,
                     Note = 5,
                     Commentaire = "Profil très prometteur, expertise Java impressionnante",
                 }
             );
         }

         private static string GetSamplePdfBase64()
         {
             // This is a minimal valid PDF file in Base64 format
             return "JVBERi0xLjcKJeLjz9MKNSAwIG9iago8PAovVHlwZSAvWE9iamVjdAovU3VidHlwZSAvSW1hZ2UKL1dpZHRoIDEKL0hlaWdodCAxCi9Db2xvclNwYWNlIC9EZXZpY2VSR0IKL0JpdHNQZXJDb21wb25lbnQgOAovRmlsdGVyIC9EQ1REZWNvZGUKL0xlbmd0aCA4Nwo+PgpzdHJlYW0K/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHR4fHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Iyx4RDhEMDgyMjX/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAABAAEDASIAAhEBAhEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAb/xAAhEAACAQMDBQAAAAAAAAAAAAABAgMABAUGIWEREiMxUf/EABUBAQEAAAAAAAAAAAAAAAAAAAMF/8QAGhEAAgMBAQAAAAAAAAAAAAAAAAECEgMRkf/aAAwDAQACEQMRAD8AltJagyeH0AthI5xdrLcNM91BF5pX2HaH9bcfaSXWGaRmknyJckliyjqTzSlT54b6bk+h0R//2Q==";
         }*/
        }

    }
}