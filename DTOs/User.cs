using Microsoft.AspNetCore.Identity;
using WebApiValidation.Models;
namespace WebApiValidation.DTOs
{
    public class User : IdentityUser
    {
        public string? Name {  get; set; } 
        public  string? Class {  get; set; } 
        public string? RefreshToken { get; set; }
       public DateTime RefreshTokenExpirytime { get; set; }
        public virtual  Studentrec? Studentrec { get; set; }
    }
}
