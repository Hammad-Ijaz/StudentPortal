using Azure.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiValidation.Contracts;
using WebApiValidation.DTOs;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.ServiceResponse;
namespace WebApiValidation.Repositories
{
    public class AccountService(
        Microsoft.AspNetCore.Identity.UserManager<User> userManager,
        Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager,
        ApplicationDbcontext applicationDbcontext,
        IConfiguration configuration
        ) : IAccountInterface
    {
        // Add Teacher
        public async Task<GeneralResponse> CreateAccount(TeacherViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return new GeneralResponse(false, "Model is empty!");
                }
                var newUser = new User()
                {
                    Name = model.Name,
                    UserName = model.Name,
                    PhoneNumber = model.Contactno,
                    Email = model.Email
                };
                var existingUser = await userManager.FindByEmailAsync(newUser.Email);
                if (existingUser != null)
                {
                    return new GeneralResponse(false, "User already registered!");
                }
                var createUserResult = await userManager.CreateAsync(newUser, model.Password);
                var adminRole = await roleManager.FindByNameAsync("Admin");
                if (adminRole == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await userManager.AddToRoleAsync(newUser, "Admin");
                    return new GeneralResponse(true, "Created Account as an Admin");
                }
                if (createUserResult.Succeeded)
                {
                    TeacherRegister teacherDto = new TeacherRegister()
                    {
                        TeacherId = model.Id,
                        Name = model.Name,
                        Contactno = model.Contactno,
                        Email = model.Email,
                        Password = model.Password
                    };
                    applicationDbcontext.Teachers.Add(teacherDto);
                    await applicationDbcontext.SaveChangesAsync();
                }
                if (!createUserResult.Succeeded)
                {
						return new GeneralResponse(false, "Error! Please try again.");
                }
                var teacherRole = await roleManager.FindByNameAsync("Teacher");
            if (teacherRole == null || teacherRole != null)
            {
                await roleManager.CreateAsync(new IdentityRole("Teacher"));
                await userManager.AddToRoleAsync(newUser, "Teacher");
                return new GeneralResponse(true, "Account created successfully as a Teacher.");
             }
                return new GeneralResponse(false, "Account not created, try again.");
            }
            catch(Exception)
            {
                throw;
            }
        }
        // Delete Teacher
        public async Task<DeleteResponse> DeleteTeacherAccount(int Id)
        {
            TeacherRegister teacherRegister = new TeacherRegister();
            List<ScheduleClass> scheduleClasses = new List<ScheduleClass>();
            if (Id != null)
            {
                teacherRegister = await applicationDbcontext.Teachers.Include("ScheduleClass").FirstOrDefaultAsync(x => x.TeacherId == Id);
                teacherRegister.ScheduleClass.ToList().ForEach(sc => scheduleClasses.Add(sc));
                applicationDbcontext.ScheduleClass.RemoveRange(scheduleClasses);
                var deletedd = applicationDbcontext.Teachers.Remove(teacherRegister);
                if (deletedd != null)
                {

                    var getteacher = applicationDbcontext.Teachers.Find(Id);
                    var user = await userManager.FindByEmailAsync(getteacher.Email);
                    if (user != null)
                    {
                        applicationDbcontext.Users.Remove(user);
                    }
                }
                applicationDbcontext.SaveChanges();
                return new DeleteResponse(true,"Successfully , Teacher's record deleted along with Schedule's of classes");
            }
                return new DeleteResponse(false,"No Record Delete of teacher may Id null or no reacord available against this Id;");
        }
        //Add  Student
        public async Task<StudentCreateAccountResponse> StudentCreateAccount(StudentViewModel studentrec)
        {
            if (studentrec == null)
            { return new StudentCreateAccountResponse(false, "Model is an empty!"); }
            var Newuser = new User()
            {
                Name = studentrec.Name,
                UserName = studentrec.Name,
                PhoneNumber = studentrec.Contactno,
                Email = studentrec.Email
            };
             var Existinguser = await userManager.FindByEmailAsync(Newuser.Email);
             if (Existinguser != null) { return new StudentCreateAccountResponse(false, "User already registered!!"); }
              var Createuser = await userManager.CreateAsync(Newuser!, studentrec.Password);
            if(Createuser.Succeeded) {
                Studentrec std = new Studentrec()
                {
                    Id = studentrec.Id,
                    Name = studentrec.Name,
                    ClassId = studentrec.ClassIds,
                    Contactno = studentrec.Contactno,
                    Email = studentrec.Email,
                    Password = studentrec.Password 
                };
                applicationDbcontext.Studentslist.Add(std);
                await applicationDbcontext.SaveChangesAsync();
            }
             if (!Createuser.Succeeded) { return new StudentCreateAccountResponse(false, "Error!!  Please try again."); }
            if (await roleManager.FindByNameAsync("Student") == null)      {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Student" });
            }
            await userManager.AddToRoleAsync(Newuser, "Student");
            return new StudentCreateAccountResponse(true,"Student's Account Created!.");
        }
        // login 
        public async Task<LoginResponse> LoginAccount(UserLogin userLogin)
        {
            if (userLogin == null)
            {
                return new LoginResponse(false, null!, null!, "Model is an empty!");
            }
            var getuser = await userManager.FindByEmailAsync(userLogin.Email);
            if (getuser == null)
            {
                return new LoginResponse(false, null!, null!, "User not found");
            }
            bool getpassword = await userManager.CheckPasswordAsync(getuser, userLogin.Password);
            if (!getpassword)
            {
                return new LoginResponse(false, null!, null!, "Invalid Email or Password");
            }
            var getrole = await userManager.GetRolesAsync(getuser);
            var usersession = new Usersession(getuser.Id, getuser.UserName, getuser.Email, getrole.First());
            bool IsLoggedin = true;
            string generateToken = GenerateToken(usersession);
            string token = ValidateToken(generateToken);
            string refreshtoken = GenerateRefreshToken();
            getuser.RefreshToken = refreshtoken;
            getuser.RefreshTokenExpirytime = DateTime.Now.AddDays(1);
            await userManager.UpdateAsync(getuser);
            return new LoginResponse(IsLoggedin,token,refreshtoken , "Login Successfully");
        }
                    //  Refresh  Token Api Service
        public async Task<LoginResponse> RefreshToken(RefreshTokenModel model)
        {
            var principal = GetPrincipalClaimsfromExpiredToken(model.Token);
            if (principal?.Identity?.Name is null)
            {
                return null;
            }
            var identityUser = await userManager.FindByNameAsync(principal.Identity.Name);
            if (identityUser is null ||
                identityUser.RefreshToken != model.RefreshToken  ||
                identityUser.RefreshTokenExpirytime <= DateTime.Now) 
            {
                return null;
            }
            var roles = await userManager.GetRolesAsync(identityUser);
            var userSession = new Usersession(identityUser.Id, identityUser.UserName, identityUser.Email, roles.FirstOrDefault());
            bool isLoggedIn = true;
            string token = GenerateToken(userSession);
            string refreshToken = GenerateRefreshToken();
            identityUser.RefreshToken = refreshToken;
          //  identityUser.RefreshTokenExpirytime = DateTime.Now.AddDays(1).Date;
            await userManager.UpdateAsync(identityUser);
            return new LoginResponse(isLoggedIn, token, refreshToken, "Get Refresh Token For Security Reasons!!");
        }
                     //  Generate token
        private string GenerateToken(Usersession user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name , user.Name),
                new Claim(ClaimTypes.Email , user.Email),
                new Claim(ClaimTypes.Role , user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials     );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
                       //     verify token
        private string ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return "Token is null or empty";
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                }, out SecurityToken validatedToken);
                return token;
            }
            catch (SecurityTokenExpiredException)
            {
                return "Token has expired";
            }
            catch (SecurityTokenException)
            {
                return "Invalid token";
            }
            catch (Exception)
            {
                return "Error while validating token";
            }
        }
                           // Refresh token generate
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private ClaimsPrincipal GetPrincipalClaimsfromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:key").Value!)),
                ValidateLifetime = false, // This is set to false to allow extraction of claims from an expired token
            };
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsprincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                    return claimsprincipal;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Token validation failed", ex);
            }
        }
    }
}

