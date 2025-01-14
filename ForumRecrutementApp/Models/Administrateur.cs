using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace ForumRecrutementApp.Models
{

    public class Administrateur
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Email { get; set; }
        public string? IdentityUserId { get; set; }  
        public IdentityUser? IdentityUser { get; set; }
    }
}