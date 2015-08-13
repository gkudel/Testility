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
using System.Collections.Generic;
using System.Linq;

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
            return View(setupRepository.GetAllSourceCodes(false));
        }

        public ActionResult Create()
        {
            TempData["header"] = string.Format("Create");
            return View("CreateAndEdit", new SourceCode());
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
        public ActionResult EditPost([Bind(Include = "Id, Name, Language, ReferencedAssemblies")]SourceCode sourceCode, HttpPostedFileBase uploadedFile, int Id = 0)
        {
            if (sourceCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (sourceCode.Id != 0)
            {
                sourceCode = setupRepository.GetSourceCode(sourceCode.Id, false);
            }
            if (sourceCode == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Input input = fileRepository.CreateInputClass(sourceCode, uploadedFile);
                    Result result = compilerRepository.Compile(input);

                    if (result.Errors.Count > 0)
                    {
                        ModelState.AddModelError(String.Empty, string.Format("An error occurred when compiling attached file {0}", uploadedFile.FileName));
                        return View("CreateAndEdit", sourceCode);
                    }
                    sourceCode.Code = input.Code;
                    Mapper.Map<Result, SourceCode>(result, sourceCode);

                    setupRepository.Save(sourceCode);

                    TempData["savemessage"] = string.Format("{0} has been edited", sourceCode.Name);
                    return RedirectToAction("Index");
                }
                catch (Exception /* ex */ )
                {
                    ModelState.AddModelError(String.Empty, string.Format("An error occurred when updating {0}", sourceCode.Name));
                    return View("CreateAndEdit", sourceCode);
                }
            }
            else
            {
                return View("CreateAndEdit", sourceCode);
            }
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
                setupRepository.Delete(id);
                TempData["savemessage"] = string.Format("SourceCoude has been deleted");
            }
            catch (Exception /*ex*/ )
            {
                ModelState.AddModelError(String.Empty, string.Format("An error occurred when deleting"));
            } 
            
            return RedirectToAction("Index");
        }
    }
}
