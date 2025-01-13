using ForumRecrutementApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ForumRecrutementApp.Models
{
    public class Forum
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        public DateTime Date { get; set; }
        public ICollection<Recruteur> Recruteurs { get; set; }
        public ICollection<Candidat> Candidats { get; set; }

    }
}
