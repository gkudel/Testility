using AutoMapper;
using Testility.Domain.Entities;
using Testility.Engine.Model;


namespace Testility.WebUI.Mappings
{
    public class UIModelToEngineModelMappings : Profile
    {        
         public override string ProfileName
        {
            get { return "UiModelToEngineModelMappings"; }
        }
         protected override void Configure()
         {
             Mapper.CreateMap<SourceCode, Input>();
             Mapper.CreateMap<Input,SourceCode>();
             Mapper.CreateMap<Engine.Model.TestedClass, Domain.Entities.TestedClass>();
         }
    }

}