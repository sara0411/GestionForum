using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ForumRecrutementApp.Models
{
    public class Candidat
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Les compétences sont requises")]
        public string Competences { get; set; }

        [NotMapped]
        public IFormFile CVFile { get; set; }

        public string CV { get; set; }

        // Foreign key for Forum (nullable)
        public int? ForumId { get; set; }

        // Navigation property for Forum
        public Forum Forum { get; set; }

        // Navigation property for Evaluations
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    }
}