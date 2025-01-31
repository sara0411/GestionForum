using ForumRecrutementApp.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace ForumRecrutementApp.Models
{
public class Recruteur
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nom { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
        public string? IdentityUserId { get; set; }  
        public IdentityUser? IdentityUser { get; set; }  
    public string Entreprise { get; set; }
    public int? ForumId { get; set; }
    public Forum Forum { get; set; }
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    }

}