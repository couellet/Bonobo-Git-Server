using System;
using System.Linq;
using System.Web.Mvc;
using Bonobo.Git.Server.App_GlobalResources;
using Bonobo.Git.Server.Models;
using Bonobo.Git.Server.Security;
using Microsoft.Practices.Unity;

namespace Bonobo.Git.Server.Controllers
{
    public class UserController : Controller
    {
        [Dependency]
        public IMembershipService MembershipService { get; set; }

        [Dependency]
        public IFormsAuthenticationService FormsAuthenticationService { get; set; }

        [AuthorizeRedirect(Roles = Definitions.Roles.Administrator)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, AuthorizeRedirect(Roles = Definitions.Roles.Administrator)]
        public ActionResult Create(RegisterModel model)
        {
            while (!String.IsNullOrEmpty(model.Username) && model.Username.Last() == ' ')
            {
                model.Username = model.Username.Substring(0, model.Username.Length - 1);
            }

            if (ModelState.IsValid)
            {
                if (MembershipService.CreateUser(model.Username, model.Password, model.Name, model.Surname, model.Email))
                {
                    TempData["CreateSuccess"] = true;
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("Username", Resources.Home_Register_AccountAlreadyExists);
                }
            }

            return View(model);     
        }
    }
}