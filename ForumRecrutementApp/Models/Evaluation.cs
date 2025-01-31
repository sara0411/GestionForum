using System;
using System.ComponentModel.DataAnnotations;

namespace ForumRecrutementApp.Models
{
    public class Evaluation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CandidatId { get; set; }

        [Required]
        public int RecruteurId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La note doit être comprise entre 1 et 5.")]
        public int Note { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Le commentaire ne peut pas dépasser 500 caractères.")]
        public string Commentaire { get; set; }

        [Required]
        public DateTime DateEvaluation { get; set; } = DateTime.Now;

        // Navigation properties
        public Candidat Candidat { get; set; }
        public Recruteur Recruteur { get; set; }
    }
}