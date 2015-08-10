using System;
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

        // GET: SourceCodes/Create
        public ActionResult Create()
        {
            TempData["header"] = string.Format("Create");
            TempData["action"] = "Create";
            return View("CreateAndEdit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Code,Language,ReferencedAssemblies")] SourceCode sourceCode, HttpPostedFileBase uploadedFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Input input =  fileRepository.CreateInputClass(sourceCode, uploadedFile);
                    Result result = compilerRepository.compile(input);
                        
                     if (result.Errors.Count > 0)
                         {
                             TempData["action"] = "Create";
                             TempData["errormessage"] = string.Format("An error occurred when compiling attached file {0}", uploadedFile.FileName);
                             return View("CreateAndEdit", sourceCode);
                         }

                    Mapper.Map(result, sourceCode);

                    setupRepository.SaveSourceCode(sourceCode);
                    TempData["savemessage"] = string.Format("{0} has been saved", sourceCode.Name);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["errormessage"] = string.Format("An error occurred when saving {0}", sourceCode.Name);
                    return RedirectToAction("Index");
                }
            }

            TempData["action"] = "Create";
            return View("CreateAndEdit", sourceCode);
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
            TempData["action"] = "Edit";
            return View("CreateAndEdit", sourceCode);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, HttpPostedFileBase uploadedFile)
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
            if (TryUpdateModel(sourceCode, "", new string[] { "Name", "Code", "Language", "ReferencedAssemblies" }))
            {
                try
                {
                    Input input = fileRepository.CreateInputClass(sourceCode, uploadedFile);
                    Result result = compilerRepository.compile(input);

                    if (result.Errors.Count > 0)
                    {
                        TempData["action"] = "Edit";
                        TempData["errormessage"] = string.Format("An error occurred when compiling attached file {0}", uploadedFile.FileName);
                        return View("CreateAndEdit", sourceCode);
                    }

                    Mapper.Map(result, sourceCode);


                    setupRepository.SaveSourceCode(sourceCode);
                    TempData["savemessage"] = string.Format("{0} has been edited", sourceCode.Name);
                    return RedirectToAction("Index");
                }
                catch( Exception ex)
                {
                    TempData["errormessage"] = string.Format("An error occurred when updating {0}", sourceCode.Name);
                }
            }
            return View("CreateAndEdit", sourceCode);
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
                TempData["errormessage"] = string.Format("An error occurred when deleting");
            } 
            
            return RedirectToAction("Index");
        }
    }
}
