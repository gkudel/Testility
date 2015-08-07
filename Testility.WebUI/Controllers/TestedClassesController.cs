using System.Net;
using System.Web.Mvc;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;

namespace Testility.WebUI.Controllers
{
    public class TestedClassesController : Controller
    {
        private ITestedClassesRepository TestedClassesService;
        private ITestedMethodsRepository TestedMethodsService;

        public TestedClassesController(ITestedClassesRepository TestedClassesService, ITestedMethodsRepository TestedMethodsService)
        {
            this.TestedClassesService = TestedClassesService;
            this.TestedMethodsService = TestedMethodsService;
        }

        public ActionResult Index()
        {
            return View(TestedClassesService.GetTestetClasses());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TestedClass testedClass = TestedClassesService.DetailsTestedClass(id);

            if (testedClass == null)
            {
                return HttpNotFound();
            }

            return View(testedClass);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,SourceCode")] TestedClass testedClass)
        {
            if (ModelState.IsValid)
            {
                TestedClassesService.AddTestClasses(testedClass);
                return RedirectToAction("Index");
            }


            return View(testedClass);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestedClass testedClass = TestedClassesService.DetailsTestedClass(id);
            if (testedClass == null)
            {
                return HttpNotFound();
            }
            return View(testedClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,SourceCode")] TestedClass testedClass)
        {
            if (ModelState.IsValid)
            {
                TestedClassesService.UpdateTestClasses(testedClass);
                return RedirectToAction("Index");
            }
            return View(testedClass);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestedClass testedClass = TestedClassesService.DetailsTestedClass(id);
            if (testedClass == null)
            {
                return HttpNotFound();
            }
            return View(testedClass);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestedClassesService.DeleteTestClasses(id);
            return RedirectToAction("Index");
        }

       
    }
}
