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

namespace Testility.WebUI.Controllers
{
    public class SourceCodesController : Controller
    {
        private ISetupRepository SetupRepository;

        public SourceCodesController(ISetupRepository setupRepositor)
        {
            SetupRepository = setupRepositor;
        }

        public ActionResult Index()
        {
            return View(SetupRepository.Files);
        }

        // GET: SourceCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SourceCode sourceCode = SetupRepository.GetSourceCode(id);
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
        public ActionResult Create([Bind(Include = "Id,Name,Code,Language,ReferencedAssemblies")] SourceCode sourceCode)
        {
            if (ModelState.IsValid)
            {
                 SetupRepository.SaveSourceCode(sourceCode);
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
            SourceCode sourceCode = SetupRepository.GetSourceCode(id);
            if (sourceCode == null)
            {
                return HttpNotFound();
            }
            return View(sourceCode);
        }

        // POST: SourceCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code,Language,ReferencedAssemblies")] SourceCode sourceCode)
        {
            if (ModelState.IsValid)
            {
                SetupRepository.SourceCode(sourceCode);
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
            SourceCode sourceCode = SetupRepository.GetSourceCode(id);
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
            SetupRepository.DeleteSourceCode(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    db.Dispose();
            //}
            //base.Dispose(disposing);
        }
    }
}
