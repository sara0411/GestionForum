using System.ComponentModel.DataAnnotations;

public class Administrateur
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nom { get; set; }
    [Required]
    public string Email { get; set; }
}