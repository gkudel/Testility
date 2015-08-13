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
using Testility.WebUI.Areas.Setup.Models;

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
            return View(setupRepository.GetAllSourceCodes().ToList());
        }

        public ActionResult Create()
        {
            TempData["header"] = string.Format("Create");
            return View("CreateAndEdit", new EditViewModel() { SourceCode = new SourceCode() });
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
            return View("CreateAndEdit", new EditViewModel() { SourceCode = sourceCode } );
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(EditViewModel model)
        {
            if (model.SourceCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (model.SourceCode.Id != 0)
            {
                SourceCode code = setupRepository.GetSourceCode(model.SourceCode.Id, false);
                if (code != null)
                {
                    Mapper.Map(model.SourceCode, code);
                }
                model.SourceCode = code;
            }
            if (model.SourceCode == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.UploadedFile != null)
                    {
                        Input input = fileRepository.CreateInputClass(model.SourceCode, model.UploadedFile);
                        Result result = compilerRepository.Compile(input);

                        if (result.Errors.Count > 0)
                        {
                            ModelState.AddModelError(String.Empty, string.Format("An error occurred when compiling attached file {0}", model.UploadedFile.FileName));
                            return View("CreateAndEdit", model);
                        }
                        model.SourceCode.Code = input.Code;
                        Mapper.Map<Result, SourceCode>(result, model.SourceCode);
                    }
                    setupRepository.Save(model.SourceCode);

                    TempData["savemessage"] = string.Format("{0} has been edited", model.SourceCode.Name);
                    return RedirectToAction("Index");
                }
                catch (Exception /* ex */ )
                {
                    ModelState.AddModelError(String.Empty, string.Format("An error occurred when updating {0}", model.SourceCode.Name));
                    return View("CreateAndEdit", model);
                }
            }
            else
            {
                return View("CreateAndEdit", model);
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
