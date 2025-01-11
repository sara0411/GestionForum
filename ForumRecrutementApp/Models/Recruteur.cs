using System.ComponentModel.DataAnnotations;

public class Recruteur
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nom { get; set; }
    [Required]
    public string Entreprise { get; set; }
    public int? ForumId { get; set; }
    public Forum Forum { get; set; }
}
