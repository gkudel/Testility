using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Web.Mvc;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.WebUI.Models;

namespace Testility.WebUI.Controllers
{
    public class TestedMethodsController : Controller
    {
        private ITestedClassesRepository TestedClassesService;
        private ITestedMethodsRepository TestedMethodsService;

        public TestedMethodsController(ITestedClassesRepository TestedClassesService, ITestedMethodsRepository TestedMethodsService)
        {
            this.TestedClassesService = TestedClassesService;
            this.TestedMethodsService = TestedMethodsService;
        }

        public ActionResult Index()
        {

            List<TestedMetodViewModel> testedMetodsViewModels = new List<TestedMetodViewModel>();
            foreach (TestedMethod item in TestedMethodsService.GetMethods())
            {
                TestedMetodViewModel testedMetodViewModel = Mapper.Map<TestedMetodViewModel>(item);
                testedMetodViewModel.TestedClassName = TestedClassesService.GetClassName(item.Id);
                testedMetodsViewModels.Add(testedMetodViewModel);
            }

            return View(testedMetodsViewModels);
        }

        //// GET: TestedMethods/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TestedMethod testedMethod = db.TestedMethods.Find(id);
        //    if (testedMethod == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(testedMethod);
        //}

        public ActionResult Create()
        {
            TestedMetodViewModel testedViewModel = new TestedMetodViewModel() { TestedClasses = TestedClassesService.GetTestetClasses() };
            return View(testedViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description, TestedClassId")] TestedMetodViewModel testedViewModel)
        {
            if (ModelState.IsValid)
            {
                TestedMethod testedMethod = Mapper.Map<TestedMethod>(testedViewModel);
                TestedMethodsService.SaveTestedMethod(testedMethod);
                return RedirectToAction("Index");
            }

            return View(testedViewModel);
        }

        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //TestedMethod testedMethod = db.TestedMethods.Find(id);
            //if (testedMethod == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] TestedMethod testedMethod)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(testedMethod).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View();
        }

        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //TestedMethod testedMethod = db.TestedMethods.Find(id);
            //if (testedMethod == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //TestedMethod testedMethod = db.TestedMethods.Find(id);
            //db.TestedMethods.Remove(testedMethod);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        
    }
}
