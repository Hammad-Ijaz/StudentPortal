using Microsoft.EntityFrameworkCore;
using WebApiValidation.Contracts;
using static WebApiValidation.DTOs.ServiceResponse;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;
using NuGet.Versioning;
using WebApiValidation.DTOs;
using Microsoft.AspNet.Identity;

namespace WebApiValidation.Repositories
{
    public class ClassScheduleService(
        Microsoft.AspNetCore.Identity.UserManager<User> userManager,
         ApplicationDbcontext dbcontext) : IClassScheduleInterface
    {
        public async Task<GetTimeTableResponse> GetTimeTable(int Id, string userId)
        {
            User user = new User();
            List<ScheduleClassViewModel> classes = new List<ScheduleClassViewModel>();
            var ExistingTeacherSchedule = dbcontext.ScheduleClass.FirstOrDefault(x => x.TeacherId == Id);
            var ListSchedule = await dbcontext.ScheduleClass.OrderBy(x => x.StartDate).Where(x => x.TeacherId == Id).ToListAsync();
            if (ExistingTeacherSchedule == null || ListSchedule == null){
                return new GetTimeTableResponse(false, "No Schedule List yet!!", user, classes);}
            try
            {
                user = await userManager.FindByIdAsync(userId);
                var role = await userManager.GetRolesAsync(user);
               if (userId != null &&role.Contains("Admin"))
                {
                        foreach (var dd in ListSchedule)
                        {
                            ScheduleClassViewModel model = new ScheduleClassViewModel
                            {
                                Id = dd.Id,
                                TeacherId = dd.TeacherId,
                                Days = dd.Days.ToString(),
                                DurationTime = dd.DurationTime,
                                StartDate = dd.StartDate.Date,
                                EndDate = dd.EndDate.Date,
                                Room = dd.Room
                            };
                            var teacherCourses = dbcontext.ScheduleClass
                                .Where(p => p.TeacherId == Id && p.Course_Id == dd.Course_Id)
                                .Select(teacher => new
                                {
                                    teacher.Course_Id,
                                    teacher.Course.Courses
                                }).FirstOrDefault();
                            if (teacherCourses != null)
                            {
                                model.Course_Id = teacherCourses.Course_Id;
                                model.Coursess = teacherCourses.Courses;
                            }
                            var teacherClass = dbcontext.ScheduleClass
                                .Where(p => p.TeacherId == Id && p.ClassId == dd.ClassId)
                                .Select(teacher => new
                                {
                                    teacher.ClassId,
                                    teacher.Class.ClassName
                                }).FirstOrDefault();
                            if (teacherClass != null)
                            {
                                model.ClassId = teacherClass.ClassId;
                                model.Classess = teacherClass.ClassName;
                            }
                            classes.Add(model);

                        return new GetTimeTableResponse(true, "Successfully Schedule list show.", user, classes);
                    }
                }
                else
                {
                    var ExistingTeacher = dbcontext.Teachers.FirstOrDefault(x => x.TeacherId == Id);
                    user = await userManager.FindByEmailAsync(ExistingTeacher.Email);
                    if (ListSchedule != null)
                    {
                        foreach (var dd in ListSchedule)
                        {
                            ScheduleClassViewModel model = new ScheduleClassViewModel
                            {
                                Id = dd.Id,
                                TeacherId = dd.TeacherId,
                                Days = dd.Days.ToString(),
                                DurationTime = dd.DurationTime,
                                StartDate = dd.StartDate.Date,
                                EndDate = dd.EndDate.Date,
                                Room = dd.Room
                            };
                            var teacherCourses = dbcontext.ScheduleClass
                                .Where(p => p.TeacherId == Id && p.Course_Id == dd.Course_Id)
                                .Select(teacher => new
                                {
                                    teacher.Course_Id,
                                    teacher.Course.Courses
                                }).FirstOrDefault();
                            if (teacherCourses != null)
                            {
                                model.Course_Id = teacherCourses.Course_Id;
                                model.Coursess = teacherCourses.Courses;
                            }
                            var teacherClass = dbcontext.ScheduleClass
                                .Where(p => p.TeacherId == Id && p.ClassId == dd.ClassId)
                                .Select(teacher => new
                                {
                                    teacher.ClassId,
                                    teacher.Class.ClassName
                                }).FirstOrDefault();
                            if (teacherClass != null)
                            {
                                model.ClassId = teacherClass.ClassId;
                                model.Classess = teacherClass.ClassName;
                            }
                            classes.Add(model);
                        }

                        return new GetTimeTableResponse(true, "Successfully Schedule list show.", user, classes);
                    }
                }
                return new GetTimeTableResponse(true, "No  Schedule list yet.", user, classes);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<SearchTimeTableResponse> GetSearchTimeTable(string Course)
        {
            try
            {
                List<ScheduleClassViewModel> classes = new List<ScheduleClassViewModel>();
                var exist = dbcontext.Courserecord.FirstOrDefault(x=> x.Courses == Course);
                if (exist == null) { return new SearchTimeTableResponse(false,"No course exist or invalid course name.", classes); }
                var ListSchedule = await dbcontext.ScheduleClass.Where(x => x.Course_Id == exist.Course_Id).ToListAsync();
                if (ListSchedule != null && ListSchedule.Count != 0)
                {
                    foreach (var dd in ListSchedule)
                    {
                        ScheduleClassViewModel model = new ScheduleClassViewModel
                        {
                            Id = dd.Id,
                            TeacherId = dd.TeacherId,
                            Days = dd.Days.ToString(),
                            DurationTime = dd.DurationTime,
                            StartDate = dd.StartDate.Date,
                            EndDate = dd.EndDate.Date,
                            Room = dd.Room
                        };
                        var teacherCourses = dbcontext.ScheduleClass
                            .Where(p => p.Course_Id == dd.Course_Id)
                            .Select(teacher => new
                            {
                                teacher.Course_Id,
                                teacher.Course.Courses
                            }).FirstOrDefault();
                        if (teacherCourses != null)
                        {
                            model.Course_Id = teacherCourses.Course_Id;
                            model.Coursess = teacherCourses.Courses;
                        }
                        var teacherClass = dbcontext.ScheduleClass
                            .Where(p => p.ClassId == dd.ClassId)
                            .Select(teacher => new
                            {
                                teacher.ClassId,
                                teacher.Class.ClassName
                            }).FirstOrDefault();
                        if (teacherClass != null)
                        {
                            model.ClassId = teacherClass.ClassId;
                            model.Classess = teacherClass.ClassName;
                        }
                        classes.Add(model);
                    }
                    return new SearchTimeTableResponse(true, "Successfully Schedule list show.", classes);
                }
				return new SearchTimeTableResponse(false, "No  Schedule list yet.", classes);
			}
            catch (Exception ex)
            {
                throw;
            }
        }
		public async Task<TimeTableResponse> AddTimeTable(AddScheduleClassViewModel model)
        {
            if (model == null)
            {
                return new TimeTableResponse(false, "Failed! No schedule class occurred.");
            }
            var checkTeacherExist = await dbcontext.Teachers.FirstOrDefaultAsync(x => x.TeacherId == model.TeacherId);
            if (checkTeacherExist == null) { return new TimeTableResponse(false, "Teacher is not existed."); }
            try
            {
                List<ScheduleClassViewModel> listModel = new List<ScheduleClassViewModel>();
                var checkRoomTime = dbcontext.ScheduleClass.Where(x => x.Room == model.Room && x.DurationTime == model.DurationTime &&
                x.StartDate == model.StartDate && x.EndDate == model.EndDate && x.Days == (DayOfWeek)model.Day).FirstOrDefault();
                if (checkRoomTime != null)
                { return new TimeTableResponse(false, "Sorry, unavailable room."); }
                TeacherCourse teacherCourse = new TeacherCourse();
                DateTime currentDate = model.StartDate;
                while (currentDate <= model.EndDate)
                {
                    if (currentDate.DayOfWeek == (DayOfWeek)model.Day)
                    {
                        ScheduleClass scheduleClass = new ScheduleClass
                        {
                            TeacherId = model.TeacherId,
                            DurationTime = model.DurationTime,
                            Days = (DayOfWeek)model.Day,
                            StartDate = DateTime.Parse(currentDate.ToString()).Date,
                            EndDate = model.EndDate.Date,
                            Room = model.Room
                        };
                        if (model.Classes != null)
                        {
                            foreach (var classId in model.Classes)
                            {
                                listModel.Add(new ScheduleClassViewModel
                                {
                                    ClassId = classId
                                });
                                scheduleClass.ClassId = classId;
                                model.ClassId = classId;
                            }
                        }
                        if (model.Courses != null)
                        {
                            foreach (var courseId in model.Courses)
                            {
                                listModel.Add(new ScheduleClassViewModel
                                {
                                    ClassId = model.ClassId,
                                    Course_Id = courseId,
                                });
                                scheduleClass.Course_Id = courseId;
                                model.Course_Id = courseId;
                                teacherCourse.Course_Id = courseId;
                                teacherCourse.TeacherId = model.TeacherId;
                            }
                        }
                        var checkClass = dbcontext.ScheduleClass.FirstOrDefault(c =>
                          c.ClassId == model.ClassId && c.DurationTime == model.DurationTime);
                    if(checkClass != null)
                        {
                            if (checkClass.ClassId == model.ClassId && checkClass.Course_Id == model.Course_Id && checkClass.TeacherId == model.TeacherId && checkClass.StartDate == model.StartDate)
                            { return new TimeTableResponse(false, "Someone teacher already teaching this course to this class."); }
                            else if (checkClass.ClassId == model.ClassId && checkClass.DurationTime == model.DurationTime && checkClass.StartDate == model.StartDate)
                            { return new TimeTableResponse(false, "Sorry, already class on that time."); }
                            else if (checkClass.ClassId == model.ClassId && checkClass.DurationTime == model.DurationTime &&
                                      checkClass.StartDate == model.StartDate && checkClass.EndDate == model.EndDate)
                            { return new TimeTableResponse(false, "Sorry, already class on that date."); }
                            else if (checkClass.DurationTime == model.DurationTime && checkClass.StartDate == model.StartDate &&
                                checkClass.Room == model.Room) {
                                return new TimeTableResponse(false,"Sorry ,Room is unavailable."+currentDate);
                            }
                        }
                        await dbcontext.ScheduleClass.AddAsync(scheduleClass);
                    }
                    currentDate = currentDate.AddDays(1);
                }
                await dbcontext.TeacherCourse.AddAsync(teacherCourse);
                await dbcontext.SaveChangesAsync();
                return new TimeTableResponse(true, "Successfully scheduled class.");
                Console.WriteLine("For git Schedule class console write..");
            }
                catch (Exception)
            {
                throw;
               // return new TimeTableResponse(false, "An error occurred while scheduling the class. Please try again.");
            }
        }
    }
}
