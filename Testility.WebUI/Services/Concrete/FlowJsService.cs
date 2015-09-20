using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Testility.WebUI.Model;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Services.Concrete
{
    public class FlowJsService : IFlowJsService
    {
        private readonly IFilesPath filesPath;
        public FlowJsService(IFilesPath filesPath)
        {
            this.filesPath = filesPath;
        }
                
        public bool ChunkExists(HttpRequest request)
        {
            var identifier = request.QueryString["flowIdentifier"];
            var chunkNumber = int.Parse(request.QueryString["flowChunkNumber"]);
            var chunkFullPathName = GetChunkFilename(chunkNumber, identifier);
            return File.Exists(Path.Combine(filesPath.GetFlowJsTempDirectory(), chunkFullPathName));
        }

        public FlowJsPostChunkResponse PostChunk(FlowChunk chunk)
        {
            return PostChunk(chunk, null);
        }

        public FlowJsPostChunkResponse PostChunk(FlowChunk chunk, IFlowValidationRules validationRules)
        {
            var errResponse = new FlowJsPostChunkResponse();
            errResponse.Status = PostChunkStatus.Error;
            errResponse.ErrorMessages.Add("damaged");

            List<string> errorMessages = null;
            var file = chunk.Files[0];

            var response = new FlowJsPostChunkResponse { FileName = chunk.FileName, Size = chunk.TotalSize };

            var chunkIsValid = true;
            if (validationRules != null)
                chunkIsValid = validationRules.ValidateBusinessRules(chunk, out errorMessages);

            if (!chunkIsValid)
            {
                response.Status = PostChunkStatus.Error;
                response.ErrorMessages = errorMessages;
                return response;
            }

            var chunkFullPathName = GetChunkFilename(chunk.Number, chunk.Identifier);
            try
            {
                file.SaveAs(chunkFullPathName);
            }
            catch (Exception)
            {
                throw;
            }

            for (int i = 1, l = chunk.TotalChunks; i <= l; i++)
            {
                var chunkNameToTest = GetChunkFilename(i, chunk.Identifier);
                var exists = File.Exists(chunkNameToTest);
                if (!exists)
                {
                    response.Status = PostChunkStatus.PartlyDone;
                    return response;
                }
            }

            var fileAry = new List<string>();
            for (int i = 1, l = chunk.TotalChunks; i <= l; i++)
            {
                fileAry.Add("flow-" + chunk.Identifier + "." + i);
            }

            response.FilePath = MergeChunks(fileAry, chunk.FileName);

            for (int i = 0, l = fileAry.Count; i < l; i++)
            {
                try
                {
                    File.Delete(Path.Combine(filesPath.GetFlowJsTempDirectory(), fileAry[i]));
                }
                catch (Exception)
                {
                }
            }

            response.Status = PostChunkStatus.Done;
            return response;
        }

        private string MergeChunks(IEnumerable<string> fileAry, string destFile)
        {
            string path = Path.Combine(filesPath.GetFlowJsTempDirectory(), destFile);
            using (var destStream = File.Create(path))
            {
                foreach (string filePath in fileAry)
                {
                    using (var sourceStream = File.OpenRead(Path.Combine(filesPath.GetFlowJsTempDirectory(), filePath)))
                        sourceStream.CopyTo(destStream); 
                }
            }
            return path;
        }

        private string GetChunkFilename(int chunkNumber, string identifier)
        {
            string folder = filesPath.GetFlowJsTempDirectory();
            return Path.Combine(folder, "flow-" + identifier + "." + chunkNumber);
        }
    }
}