

using System.ComponentModel.DataAnnotations;

public class RegisterUserDto
{
   

    [Required(ErrorMessage = " Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
  
    public string? Role { get;  set; }
}
