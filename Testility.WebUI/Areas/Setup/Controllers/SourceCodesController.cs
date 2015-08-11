using System;
using System.Data.Entity.Validation;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.Engine.Abstract;
using Testility.Engine.Model;
using Testility.WebUI.Services;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class SourceCodesController : Controller
    {
        private ISetupRepository setupRepository;
        private ICreateInputClassFromFile fileRepository;
        private ICompiler compilerRepository;

        public SourceCodesController(ISetupRepository setupRepositor, ICreateInputClassFromFile fileRepositor, ICompiler compilerRepositor)
        {
            setupRepository = setupRepositor;
            fileRepository = fileRepositor;
            compilerRepository = compilerRepositor;
        }

        public ActionResult Index()
        {
            return View(setupRepository.SourceCodes);
        }

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

        public ActionResult Create()
        {
            TempData["header"] = string.Format("Create");
            return View("CreateAndEdit");
        }

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
            TempData["header"] = string.Format("Edit");
            return View("CreateAndEdit", sourceCode);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "Name,Code,Language,ReferencedAssemblies")]SourceCode sourceCode, HttpPostedFileBase uploadedFile , int Id = 0)
        {
            if (sourceCode == null)
            {
                return HttpNotFound();
            }
            //if (TryUpdateModel(sourceCode, "", new string[] { "Name", "Language", "ReferencedAssemblies" }))
            //{
                try
                {
                    Input input = fileRepository.CreateInputClass(sourceCode, uploadedFile);
                    Result result = compilerRepository.Compile(input);

                    if (result.Errors.Count > 0)
                    {
                        ModelState.AddModelError(String.Empty,string.Format("An error occurred when compiling attached file {0}", uploadedFile.FileName));
                        return View("CreateAndEdit", sourceCode);
                    }

                    sourceCode.Code = input.Code;
                    setupRepository.DeleteSourceCode(Id);

                    foreach (Engine.Model.TestedClass testedClass in result.TestedClasses)
                    {
                        Domain.Entities.TestedClass modelTestedClass = new Domain.Entities.TestedClass() { Description = testedClass.Description, Name = testedClass.Name };
                       
                        foreach (Engine.Model.TestedMethod testedMethod in testedClass.Methods)
                        {
                            Domain.Entities.TestedMethod modelTestedMethod = new Domain.Entities.TestedMethod() { Name = testedMethod.Name, Description = testedMethod.Description };

                            foreach (Engine.Model.Test test in testedMethod.Tests)
                            {
                                Domain.Entities.Test modelTest = new Domain.Entities.Test() { Name = test.Name, Description = test.Description, Fail = test.Fail };
                                setupRepository.AddTestsToDb(modelTestedMethod, modelTest);
                            }
                            setupRepository.AddMethodsToDb(modelTestedClass, modelTestedMethod);
                        }
                        setupRepository.AddResultToDb(sourceCode, modelTestedClass);
                    }

                    TempData["savemessage"] = string.Format("{0} has been edited", sourceCode.Name);
                    return RedirectToAction("Index");
                }
                catch( Exception ex)
                {
                    ModelState.AddModelError(String.Empty, string.Format("An error occurred when updating {0}", sourceCode.Name));
                    return View("CreateAndEdit", sourceCode);
                }
            //}
            //return View("CreateAndEdit", sourceCode);
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                setupRepository.DeleteSourceCode(id);
                TempData["savemessage"] = string.Format("SourceCoude has been deleted");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, string.Format("An error occurred when deleting"));
            } 
            
            return RedirectToAction("Index");
        }
    }
}
