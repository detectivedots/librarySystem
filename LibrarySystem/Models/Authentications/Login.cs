using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models.Authentications
{
    public class Login
    {
        [EmailAddress(ErrorMessage = "Email ex #####@gmail.com")]
        [Required(ErrorMessage = "Email is required..!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required..!")]
        public string Password { get; set; }
    }
}
