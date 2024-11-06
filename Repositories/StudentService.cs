using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiValidation.Contracts;
using WebApiValidation.DTOs;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using static WebApiValidation.DTOs.ServiceResponse;

namespace WebApiValidation.Repositories
{
    public class StudentService(Microsoft.AspNetCore.Identity.UserManager<User> userManager,
        Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager,
        ApplicationDbcontext dbcontext) : IStudentInterface
    {
        public async Task<GetDataResponse> ShowDataUser(int Id , string UserId)
        {
            User user = new User();
            List<StudentViewModel> student = new List<StudentViewModel>();
            var Studentlisting = await dbcontext.Studentslist.ToListAsync();
            var AdminExist = await dbcontext.Admin.FirstOrDefaultAsync(x => x.AdminId == Id);
            var TeacherExist = await dbcontext.Teachers.FirstOrDefaultAsync(x => x.TeacherId == Id);
            if (AdminExist == null || TeacherExist == null) { return new GetDataResponse(false, "User is not existed.", user,student); }
            try
            {
                user = await userManager.FindByEmailAsync(AdminExist.Email);
                var role = await userManager.GetRolesAsync(user);
                if (AdminExist != null  && user.Id == UserId && role.Contains("Admin"))
                {
                    if (Studentlisting != null)
                    {
                        foreach (var data in Studentlisting)
                        {
                            StudentViewModel model = new StudentViewModel();
                            model.Id = data.Id;
                            model.Name = data.Name;
                            model.Contactno = data.Contactno;
                            model.Email = data.Email;
                            model.Password = data.Password;
                            var stds = dbcontext.Studentslist.Where(z => z.Id == data.Id).Select(z => new
                            { z.ClassId, z.Class.ClassName }).FirstOrDefault();
                            if (stds != null)
                            {
                                model.ClassIds = stds.ClassId; model.ClassN = stds.ClassName;
                            }
                            student.Add(model);
                        }
                        return new GetDataResponse(true, "Admin", user, student);
                    }
                }
                if(TeacherExist != null)
                {
                    var teacherClass = await dbcontext.ScheduleClass.Where(x => x.TeacherId == Id)
                                     .Select(c => c.Class).FirstOrDefaultAsync();
                    if (teacherClass != null)
                    {
                        user = await userManager.FindByEmailAsync(TeacherExist.Email);
                        var studentlist = await dbcontext.Studentslist.Where(x => x.Class.ClassName == teacherClass.ClassName)                                 
                                           .ToListAsync();
                        if (studentlist != null && studentlist.Count != 0)
                        {
                            foreach (var data in studentlist)
                            {
                                StudentViewModel model = new StudentViewModel();
                                model.Id = data.Id;
                                model.Name = data.Name;
                                model.Contactno = data.Contactno;
                                model.Email = data.Email;
                                model.Password = data.Password;
								var stds = dbcontext.Studentslist.Where(z => z.Id == data.Id).Select(z => new
								{ z.ClassId, z.Class.ClassName }).FirstOrDefault();
								if (stds != null)
								{
									model.ClassIds = stds.ClassId; model.ClassN = stds.ClassName;
								}
								student.Add(model);
							}
                            return new GetDataResponse(true, "Teacher", user, student);
                        }
                        return new GetDataResponse(false, "No Student Registered in this class yet.", user, student);
                    }
                }
                return new GetDataResponse(false, "Error !! Fetching Student data.", user, student);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<SearchStdResponse> SearchUserData(string Name,string Class)
        {
            List<StudentViewModel> student = new List<StudentViewModel>();
            if (Name != null || Class != null)
            {
                var SearchStudents = await dbcontext.Studentslist
                    .Where(s => s.Name == Name && s.Class.ClassName == Class)
                    .ToListAsync();
                foreach (var user in SearchStudents)
                {
                    StudentViewModel model = new StudentViewModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Contactno = user.Contactno,
                        Email = user.Email,
                        Password = user.Password
                    };
                    var stds = dbcontext.Studentslist.Where(z => z.Id == user.Id).Select(z => new
                    { z.ClassId, z.Class.ClassName }).FirstOrDefault();
                    if (stds != null) {
                            model.ClassIds = stds.ClassId; model.ClassN = stds.ClassName;    }
                        student.Add(model);
                }
            }
                    return new SearchStdResponse(Name, Class, student);
        }
        public async Task<GetDataTeacherResponse> ShowDataTeacher()
        {
            List<TeacherViewModel> teacher = new List<TeacherViewModel>();
            var users = await dbcontext.Teachers.ToListAsync();
            if (users != null)
            {
                foreach (var user in users)
                {
                        TeacherViewModel model = new TeacherViewModel();
                        model.Id = user.TeacherId;
                        model.Name = user.Name;
                        model.Contactno = user.Contactno;
                        model.Email = user.Email;
                        model.Password = user.Password;
                        teacher.Add(model);
                }
                return new GetDataTeacherResponse(true, "Teacher", teacher);
            }
            return new GetDataTeacherResponse(false, "Error", teacher);
        }
        public async Task<UpdateUserResponse> UpdateAccount(StudentViewModel model)
        {
            if (model == null)
                { return new UpdateUserResponse(false, "Model is empty!!");   }
            var user = await dbcontext.Studentslist.FirstOrDefaultAsync(x => x.Id ==model.Id);
            if (user == null)
               { return new UpdateUserResponse(false, "User not found!!");}
            user.Name = model.Name;
            user.Contactno = model.Contactno;
            user.ClassId = model.ClassIds;
            user.Password = model.Password; 
            user.Email = model.Email;
            //if (!string.IsNullOrEmpty(model.Password))
            //{
            //    var removePasswordResult = await userManager.RemovePasswordAsync(user);
            //    if (!removePasswordResult.Succeeded)
            //    {  return new UpdateUserResponse(false, "Failed to remove old password.");}
            //    var addPasswordResult = await userManager.AddPasswordAsync(user, model.Password);
            //    if (!addPasswordResult.Succeeded)
            //        {  return new UpdateUserResponse(false, "Failed to set new password.");}
            //}
				dbcontext.Studentslist.Update(user);
				dbcontext.SaveChanges();
				return new UpdateUserResponse(true, "Successfully, Data Updated!!");
        }
		public async Task<SearchReportStudentResponse> SearchQuizAssignmentStudent(int Id, string Name)
		{
            List<StudentViewModel> student = new List<StudentViewModel>();
             if(Id != 0 && Name != null)
            {
                var users = dbcontext.Studentslist.Where(y => y.Id == Id && y.Name == Name).ToList();
				foreach(var user in users)
                {
					StudentViewModel model = new StudentViewModel
					{
						Id = user.Id,
						Name = user.Name
					};
                    student.Add(model);
				}
			}
            return new SearchReportStudentResponse(student);
		}
    }
}
