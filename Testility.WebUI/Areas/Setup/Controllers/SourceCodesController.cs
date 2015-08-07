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
                setupRepository.SaveSourceCode(sourceCode);
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
        [HttpPost , ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Code,Language,ReferencedAssemblies")] SourceCode sourceCode)
        {
            if (ModelState.IsValid)
            {
                setupRepository.SaveSourceCode(sourceCode);
                return RedirectToAction("Index");
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
