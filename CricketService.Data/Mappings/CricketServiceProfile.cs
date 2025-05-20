using AutoMapper;
using CricketService.Data.Entities;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.RequestDomains;
using CricketService.Domain.ResponseDomains;

namespace CricketService.Data.Mappings
{
    public class CricketServiceProfile : Profile
    {
        public CricketServiceProfile()
        {
            CreateMap<InternationalCricketMatchRequest, LimitedOverInternationalMatchInfoDTO>()
                .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.MatchUuid));

            CreateMap<LimitedOverInternationalMatchInfoDTO, InternationalCricketMatchResponse>()
               .ForMember(dest => dest.MatchUuid, opt => opt.MapFrom(src => src.Uuid));

            CreateMap<TestCricketMatchRequest, TestCricketMatchInfoDTO>()
                .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.MatchUuid));

            CreateMap<TestCricketMatchInfoDTO, TestCricketMatchResponse>()
                .ForMember(dest => dest.MatchUuid, opt => opt.MapFrom(src => src.Uuid));

            CreateMap<DomesticCricketMatchRequest, T20MatchInfoDTO>()
                .ForMember(dest => dest.Uuid, opt => opt.MapFrom(src => src.MatchUuid));

            CreateMap<T20MatchInfoDTO, DomesticCricketMatchResponse>()
                .ForMember(dest => dest.MatchUuid, opt => opt.MapFrom(src => src.Uuid));

            CreateMap<SingleInningTeamScoreboardRequest, SingleInningTeamScoreboardResponse>();

            CreateMap<BattingScoreboardRequest, BattingScoreboardResponse>();

            CreateMap<BowlingScoreboardRequest, BowlingScoreboardResponse>();

            CreateMap<DoubleInningTeamScoreboardRequest, DoubleInningTeamScoreboardResponse>();

            CreateMap<InningScoreboard<BattingScoreboardRequest, BowlingScoreboardRequest>, InningScoreboardResponse>();
        }
    }
}
