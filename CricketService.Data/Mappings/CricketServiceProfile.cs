using AutoMapper;
using CricketService.Data.Entities;
using CricketService.Domain;

namespace CricketService.Data.Mappings
{
    public class CricketServiceProfile : Profile
    {
        public CricketServiceProfile()
        {
            CreateMap<CricketMatchInfoRequest, T20ICricketMatchInfo>();

            CreateMap<CricketMatchInfoRequest, ODICricketMatchInfo>();

            CreateMap<TestCricketMatchInfoRequest, TestCricketMatchInfo>()
                .ForMember(dest => dest.MatchDates, opt => opt.MapFrom(src => src.MatchDate));

            CreateMap<TeamScoreDetailsRequest, TeamScoreDetails>();

            CreateMap<BattingScoreCardRequest, BattingScoreCard>();

            CreateMap<BowlingScoreCardRequest, BowlingScoreCard>();

            CreateMap<TestTeamScoreDetailsRequest, TestTeamScoreDetails>();

            CreateMap<InningScoreCardDetails, InningScoreCard>();

            CreateMap<T20ICricketMatchInfo, CricketMatchInfoResponse>()
                .ForMember(dest => dest.MatchUuid, opt => opt.MapFrom(src => src.Uuid));

            CreateMap<ODICricketMatchInfo, CricketMatchInfoResponse>()
                .ForMember(dest => dest.MatchUuid, opt => opt.MapFrom(src => src.Uuid));

            CreateMap<TestCricketMatchInfo, TestCricketMatchInfoResponse>()
                .ForMember(dest => dest.MatchUuid, opt => opt.MapFrom(src => src.Uuid))
                .ForMember(dest => dest.MatchDate, opt => opt.MapFrom(src => src.MatchDates));
        }
    }
}
