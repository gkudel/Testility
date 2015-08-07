using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Testility.Domain.Abstract;
using Testility.Domain.Concrete;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class SourceCodesController : Controller
    {
        private ISetupRepository setupRepository;

        public SourceCodesController(ISetupRepository setupRepositor)
        {
            setupRepository = setupRepositor;
        }

        public ActionResult Index()
        {
            return View(setupRepository.SourceCodes);
        }

        // GET: SourceCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SourceCode sourceCode = setupRepository.GetSourceCode(id);
            if (sourceCode == null)
            {
                return HttpNotFound();
            }
            return View(sourceCode);
        }

        // GET: SourceCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SourceCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Code,Language,ReferencedAssemblies")] SourceCode sourceCode)
        {
            if (ModelState.IsValid)
            {
<<<<<<< HEAD
                setupRepository.SaveSourceCode(sourceCode);
=======
                SetupRepository.SaveSourceCode(sourceCode);
>>>>>>> 00cdf21638cfd1ff18c084182c9c535188464a53
                return RedirectToAction("Index");
            }

            return View(sourceCode);
        }

        // GET: SourceCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SourceCode sourceCode = setupRepository.GetSourceCode(id);
            if (sourceCode == null)
            {
                return HttpNotFound();
            }
           
            return View(sourceCode);
        }

        // POST: SourceCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
<<<<<<< HEAD
        [HttpPost , ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Code,Language,ReferencedAssemblies")] SourceCode sourceCode)
=======
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
>>>>>>> 00cdf21638cfd1ff18c084182c9c535188464a53
        {
            if (id == null)
            {
<<<<<<< HEAD
                setupRepository.SaveSourceCode(sourceCode);
                return RedirectToAction("Index");
=======
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SourceCode sourceCode = SetupRepository.GetSourceCode(id);
            if (sourceCode == null)
            {
                return HttpNotFound();
            }
            if (TryUpdateModel(sourceCode, "", new string[] { "Name", "Code", "Language", "ReferencedAssemblies" }))
            {
                //try
                //{
                    SetupRepository.SaveSourceCode(sourceCode);
                    return RedirectToAction("Index");
                //}
                //catch ()
                //{ }
>>>>>>> 00cdf21638cfd1ff18c084182c9c535188464a53
            }
            return View(sourceCode);
        }

        // GET: SourceCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SourceCode sourceCode = setupRepository.GetSourceCode(id);
            if (sourceCode == null)
            {
               return HttpNotFound();
            }
            return View(sourceCode);
        }

        // POST: SourceCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            setupRepository.DeleteSourceCode(id);
            return RedirectToAction("Index");
        }
    }
}
