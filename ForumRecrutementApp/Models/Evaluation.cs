using ForumRecrutementApp.Models;
using System.ComponentModel.DataAnnotations;
namespace ForumRecrutementApp.Models
{

    public class Evaluation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CandidatId { get; set; }
        public Candidat Candidat { get; set; }

        [Required]
        public int RecruteurId { get; set; }
        public Recruteur Recruteur { get; set; }

        [Range(1, 5)]
        public int Note { get; set; }

        public string Commentaire { get; set; }
    }
}
