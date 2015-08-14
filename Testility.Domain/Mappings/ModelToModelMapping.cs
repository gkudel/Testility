using AutoMapper;
using Testility.Domain.Entities;


namespace Testility.Domain.Mappings
{
    public class ModelToModelMapping : Profile
    {        
         public override string ProfileName
        {
            get { return "ModelToModelMappings"; }
        }
         protected override void Configure()
         {
             Mapper.CreateMap<Item, Item>();
         }
    }

}