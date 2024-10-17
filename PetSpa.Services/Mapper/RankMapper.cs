using AutoMapper;
using PetSpa.Contract.Repositories.Entity;
using PetSpa.ModelViews.MemberShipModelView;
using PetSpa.ModelViews.RankModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.Services.Mapper
{
    public class RankMapper : Profile
    {
        public RankMapper() {
            CreateMap<Rank,GetRankViewModel>();
            CreateMap<PostRankViewModel, Rank>();
            CreateMap<UpdateViewModel, Rank>();
        }
    }
}
