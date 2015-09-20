using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Testility.WebUI.Model;

namespace Testility.WebUI.Services.Abstract
{
    public enum PostChunkStatus: byte { Error = 0, Done = 1, PartlyDone = 2 }

    public interface IFlowJsService
    {
        bool ChunkExists(HttpRequest request);
        FlowJsPostChunkResponse PostChunk(FlowChunk chunk);
        FlowJsPostChunkResponse PostChunk(FlowChunk chunk, IFlowValidationRules validationRules);
    }

    public interface IFlowValidationRules
    {
        bool ValidateBusinessRules(FlowChunk chunk, out List<string> errorMessages);
    }

    public class BaseValidationRules : IFlowValidationRules
    {
        public long? MaxFileSize { get; set; }
        public string MaxFileSizeMessage { get; set; }
        public List<string> AcceptedExtensions { get; set; } = new List<string>();
        public string AcceptedExtensionsMessage { get; set; }

        public bool ValidateBusinessRules(FlowChunk chunk, out List<string> errorMessages)
        {
            errorMessages = new List<string>();
            if (MaxFileSize.HasValue && chunk.TotalSize > MaxFileSize.Value)
                errorMessages.Add(MaxFileSizeMessage ?? "size");

            if (AcceptedExtensions.Count > 0 && AcceptedExtensions.SingleOrDefault(x => x == chunk.FileName.Split('.').Last()) == null)
                errorMessages.Add(AcceptedExtensionsMessage ?? "type");

            return errorMessages.Count == 0;
        }
    }

    public class FlowJsPostChunkResponse
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public PostChunkStatus Status { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }

}
