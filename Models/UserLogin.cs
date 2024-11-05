using System.ComponentModel.DataAnnotations;

namespace WebApiValidation.Models
{
    public class UserLogin
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public  string Email { get; set; } = string.Empty;
        [Required]
        [DataType (DataType.Password)]
        public required  string Password { get; set; } 
    }
}
