using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        // Remove Required attribute from here if it exists
        public int? ForumId { get; set; }

        public Forum Forum { get; set; }
    }
}