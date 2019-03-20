using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using WebSessionDemo.Models;

namespace WebSessionDemo.Controllers
{
    public class ValidationTestController : Controller
    {
	    private List<ValidationTestViewModel> _validationTestViewModels
	    {
		    get
		    {
			    var validationTestViewModels = new List<ValidationTestViewModel>();
			    for (int i = 1; i < 10; i++)
				    validationTestViewModels.Add(new ValidationTestViewModel
				    {
					    Id = i,
					    Name = "Validationtest " + i,
					    Comments = "Comments no " + i
				    });

			    return validationTestViewModels;
		    }
	    }

	    // GET: ValidationTest
        public ActionResult Index()
        {
            return View(_validationTestViewModels);
        }
	    
	    public ActionResult Edit(int id)
	    {
		    var model = _validationTestViewModels.FirstOrDefault(x => x.Id == id);
            return View(model);
        }

		[HttpPost]
	    public ActionResult Edit(ValidationTestViewModel model)
	    {
		    if (!ModelState.IsValid)
			    return View(model);

		    return RedirectToAction("Index");
	    }
    }
}