using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiValidation.Models;
using WebApiValidation.ViewModels;

namespace WebApiValidation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClassController : ControllerBase
	{
		private readonly ApplicationDbcontext _db;
        public ClassController(ApplicationDbcontext db)
        {
            _db = db;
        }
        [HttpGet("ShowClass")]
		public IEnumerable<ClassViewModel> Show(int Id)
		{
			List<ClassViewModel> model = new List<ClassViewModel>();
			if (Id == 0)
			{
				var classes = _db.Classes.ToList();
				if (classes != null && classes.Any())
				{
					foreach (var item in classes)
					{
						ClassViewModel vm = new ClassViewModel();
						vm.Id = item.ClassId;
						vm.ClassName = item.ClassName;
						model.Add(vm);
					}
				}
			}
			return model;
		}
		[HttpGet("SearchClass")]
		public async Task<IActionResult> Search(string ClassName)
		{
			try
			{
				ClassViewModel model = new ClassViewModel();
				if (ClassName != null)
				{
					var classes = await _db.Classes.FirstOrDefaultAsync(x => x.ClassName == ClassName);
					if (classes != null)
					{
						model.Id = classes.ClassId;
						model.ClassName = classes.ClassName;
					}
					return Ok(new { message = "Successfully , Search Class here!! ", data = model });
				}
				return Ok(new { message = "May be no class exist or something wrong!! ", data = model });
			}
			catch (Exception)
			{
				throw;
			}
		}
		[HttpPost("AddClass")]
		public async Task<IActionResult> AddClass(ClassViewModel model)
		{
			try
			{
				var ExistClass = await _db.Classes.FirstOrDefaultAsync(c => c.ClassName == model.ClassName);
				if(ExistClass != null) { return Ok(new {message = "Already class existed."}); }
				if (model != null)
				{
					Class clas = new Class()
					{
						ClassId = model.Id,
						ClassName = model.ClassName
					};
					await _db.Classes.AddAsync(clas);
					await _db.SaveChangesAsync();
				}
				return Ok(new { message = "Already class existed." });
			}
			catch (Exception)
			{
				throw;
			}
		}
		[HttpGet("Update")]
		public JsonResult Edit(int Id)
		{
			var getfromdb = _db.Classes.FirstOrDefault(x => x.ClassId == Id);
			if (getfromdb == null && Id == 0)
			{
				return null;
			}
			ClassViewModel model = new ClassViewModel
			{
				Id = getfromdb.ClassId,
				ClassName = getfromdb.ClassName,
			};
			return new JsonResult(model);
		}
		[HttpPut("UpdateClass")]
		public JsonResult Update(ClassViewModel model)
		{
			Class classs = new Class()
			{
				ClassId = model.Id,
				ClassName = model.ClassName
			};
			if (model.Id != null)
			{
				var verify = _db.Classes.Where(c => c.ClassName == classs.ClassName).FirstOrDefault();
				if(verify == null)
				{
					_db.Classes.Update(classs);
					_db.SaveChanges();
				}
				return new JsonResult("Already class existed.");
			}
			return new JsonResult("Updated from Api controller.");
		}
		[HttpDelete("DeleteClass")]
		public JsonResult Del(int Id)
		{
			var getfromdb = _db.Classes.FirstOrDefault(d => d.ClassId == Id);
			if(getfromdb != null && Id != 0) {
				_db.Classes.Remove(getfromdb);
				_db.SaveChanges();
			}
			return new JsonResult("Deleted Successfully!!");
		}
	}
}
