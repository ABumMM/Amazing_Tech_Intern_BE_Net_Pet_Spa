using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.Core.Utils;
using PetSpa.ModelViews.MemberShipModelView;
namespace PetSpa.Services.Mapper
{
    public class MemberShipMapper : Profile
    {
        public MemberShipMapper()
        {
            //MemberShip sang GETMemberShipModelView
            CreateMap<MemberShips, GETMemberShipModelView>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
           

            .ForMember(dest => dest.TotalSpent, opt => opt.MapFrom(src => src.TotalSpent))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
        }
    }
}