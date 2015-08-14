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
using System.Collections.Generic;
using System.Linq;
using Testility.WebUI.Areas.Setup.Models;
using Testility.WebUI.Model;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    public class SourceCodesController : Controller
    {
        private ISetupRepository setupRepository;
        private ICompiler compilerRepository;
        public int PageSize { get; set; }

        public SourceCodesController(ISetupRepository setupRepositor, ICompiler compilerRepositor)
        {
            setupRepository = setupRepositor;
            compilerRepository = compilerRepositor;
            PageSize = 3;
        }

        public ActionResult Index(int? selecttedSourceCode, int page = 1)
        {
            ViewBag.SelecttedSourceCode = selecttedSourceCode;
            ProcjetsIndexData data = new ProcjetsIndexData()
            {
                List = setupRepository.GetAllSourceCodes()                    
                    .OrderBy(p => p.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = setupRepository.GetAllSourceCodes().Count()
                }
            };

            return View(data);
        }

        public ActionResult Create()
        {
            return View("CreateAndEdit", new Item());
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item sourceCode = setupRepository.GetSourceCode(id);
            if (sourceCode == null)
            {
                return HttpNotFound();
            }
            return View("CreateAndEdit", sourceCode  );
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "Id, Code, Name, Language, ReferencedAssemblies")]Item model)
        {
            bool codeChanged = model.Id == 0;
            if (model.Id != 0)
            {
                Item code = setupRepository.GetSourceCode(model.Id, false);
                if (code != null)
                {
                    codeChanged = code.Code != model.Code;
                    Mapper.Map(model, code);
                }
                model = code;
            }
            if (model == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (codeChanged)
                    {
                        Input input = Mapper.Map<Input>(model);
                        Result result = compilerRepository.Compile(input);

                        if (result.Errors.Count > 0)
                        {
                            ModelState.AddModelError(String.Empty, "An error occurred when compiling solution");
                            return View("CreateAndEdit", model);
                        }
                        Mapper.Map<Result, Item>(result, model);
                    }
                    setupRepository.Save(model);

                    TempData["savemessage"] = string.Format("{0} has been edited", model.Name);
                    return RedirectToAction("Index");
                }
                catch (Exception /* ex */ )
                {
                    ModelState.AddModelError(String.Empty, string.Format("An error occurred when updating {0}", model.Name));
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
            Item sourceCode = setupRepository.GetSourceCode(id);
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
