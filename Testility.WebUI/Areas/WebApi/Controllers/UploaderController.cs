using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Testility.WebUI.Infrastructure.Binders;
using Testility.WebUI.Model;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Areas.WebApi.Controllers
{

    [RoutePrefix("api/File")]
    public class UploaderController : ApiController
    {
        private readonly IFileService fileService;
        private readonly IFlowJsService flowJsService;

        public UploaderController(IFileService fileService, IFlowJsService flowJsService)
        {
            this.fileService = fileService;
            this.flowJsService = flowJsService;
        }

        [HttpGet]
        [Route("Upload")]
        public IHttpActionResult Get()
        {
            var request = HttpContext.Current.Request;

            var chunkExists = flowJsService.ChunkExists(request);
            if (chunkExists) return Ok();
            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        [HttpPost]
        [Route("Upload")]
        public IHttpActionResult Post([ModelBinder(typeof(FlowChunkBinder))]FlowChunk chunk)
        {
            var validationRules = new BaseValidationRules();
            validationRules.AcceptedExtensions.AddRange(new List<string> { "dll" });
            validationRules.MaxFileSize = 5000000;

            try
            {
                var status = flowJsService.PostChunk(chunk, validationRules);

                if (status.Status == PostChunkStatus.Done)
                {
                    //await fileService.UploadFile(status.FilePath);
                    return Ok(new { FileName = status.FileName, FilePath = status.FilePath });
                }

                if (status.Status == PostChunkStatus.PartlyDone)
                {
                    return Ok();
                }

                status.ErrorMessages.ForEach(x => ModelState.AddModelError("file", x));
                return BadRequest(ModelState);
            }
            catch (Exception)
            {
                ModelState.AddModelError("file", "exception");
                return BadRequest(ModelState);
            }
        }
    }
}
