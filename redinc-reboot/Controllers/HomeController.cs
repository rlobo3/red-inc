﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using redinc_reboot.Models;
using core.Modules.ProblemSet;
using core.Modules.Problem;

namespace redinc_reboot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PythonTestPage()
        {
            ViewBag.Message = "Python test page.";

            return View();
        }

        //
        // POST: /Home/PythonTestPage

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PythonTestPage(CodeModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to execute code
                model.OutputCode = GlobalStaticVars.StaticCore.ExecutePythonCode(model.InputCode);
            }
            
            // Redisplay form with output
            return View(model);
        }

        public ActionResult ProblemSet(int id = 0)
        {
            ViewBag.Message = "Modify the problem set.";
            ProblemSetData set = GlobalStaticVars.StaticCore.GetSetById(id);
            ICollection<ProblemSetData> prereqs = GlobalStaticVars.StaticCore.GetSetPrereqs(id);
            ProblemSetViewModel model = new ProblemSetViewModel();
            model.Set = set;
            model.Prereqs = prereqs;
            return View(model);
        }

        [HttpPost]
        public ActionResult ProblemSet(ProblemSetViewModel model)
        {
            if (ModelState.IsValid)
            {
                GlobalStaticVars.StaticCore.ModifySet(model.Set);
                GlobalStaticVars.StaticCore.UpdateSetPrereqs(model.Set.Id, model.Prereqs);

                //This is necessary in case bad prereqs (ex. duplicates) are removed by the backend
                model.Prereqs = GlobalStaticVars.StaticCore.GetSetPrereqs(model.Set.Id);
            }

            return View(model);
        }

        public ActionResult NewPrereq()
        {
            //TODO Get current class from persistant storage
            ICollection<ProblemSetData> sets = GlobalStaticVars.StaticCore.GetSetsForClass(2);
            ViewBag.Sets = new SelectList(sets.Select(s => new { s.Id, s.Name }), "Id", "Name");
            return PartialView("Dialogs/NewPrereq", new ProblemSetData());
        }

        [HttpPost]
        public ActionResult NewPrereq(ProblemSetData model)
        {
            return PartialView("EditorTemplates/PrereqRow", model);
        }
    }
}
