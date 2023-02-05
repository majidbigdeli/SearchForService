using System;
using AutoMapper;
using SearchForApi.Integrations.Payment;
using SearchForApi.Models;
using SearchForApi.Models.Dtos;
using SearchForApi.Models.Entities;

namespace SearchForApi.Mappers
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<SearchResultModel, SearchResultDto>();
            CreateMap<Plan, PlansDto>();
            CreateMap<RequestTokenDto, RequestPlanDto>();

            CreateMap<History, UserSearchHistoryDto>()
               .ForMember(dest => dest.Keyword, opt => opt.MapFrom(src => src.SearchKeyword));

            CreateMap<SerarchResultItemModel, SharedResultItemModel>();

            CreateMap<UserBookmarkModel, UserBookmarkDto>();

            CreateMap<Feature, FeatureDto>();
            CreateMap<FeatureItem, FeatureItemDto>();

            CreateMap<User, UserProfileDto>();

            CreateMap<Scene, ElasticSubtitleEntity>()
               .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
               .ForMember(dest => dest.SceneId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie.Title));

            CreateMap<Discount, DiscountDto>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.FixedAmount))
            .ForMember(dest => dest.Percent, opt => opt.MapFrom(src => src.Percent));

            CreateMap<AppRelease, AppReleaseDto>()
               .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Id));
        }
    }
}

