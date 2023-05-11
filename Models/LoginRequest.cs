namespace plain.Models;
using System.ComponentModel.DataAnnotations;

public class LoginRequest
{
    [Required]
    [Range(1, 10)]
    [Display(Name = "アイディー")]
    public int Id { get; set; }

    [Required]
    public string Password { get; set; }

    public LoginRequest(int id, string password)
    {
        Id = id;
        Password = password;
    }

    public string ToText()
    {
        return $"Id: {Id}, Password: {Password}";
    }
}
