using AutoMapper;
using CricketService.Data.Entities;
using CricketService.Data.Extensions;
using CricketService.Data.Utils.Helpers;
using CricketService.Domain.Enums;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Utils
{
    public class MatchPDFHandler
    {
        private readonly IMapper mapper;
        private readonly ILogger<MatchPDFHandler> logger;
        private readonly string htmlString = string.Empty;

        public MatchPDFHandler(IMapper mapper, ILogger<MatchPDFHandler> logger)
        {
            this.mapper = mapper;
            this.logger = logger;

            StreamReader headerReader = new(@"D:\MyYoutubeRepos\repo\CricketService\CricketService.Data\Utils\CricketMatch.cshtml");
            htmlString = headerReader.ReadToEnd();
        }

        public async Task AddPDFLimitedOverCricketMatch(IEnumerable<LimitedOverInternationalMatchInfoDTO> cricketMatches, CricketFormat format)
        {
            var theme = new MatchTheme(
                BaseColorHelper.LIGHT_KHAKI,
                BaseColorHelper.PEACH_ORANGE,
                BaseColorHelper.MEDIUM_SPRING_BUD,
                BaseColorHelper.BLIZZARD_BLUE);

            var matchesDto = cricketMatches;

            foreach (var matchDto in matchesDto)
            {
                FileHandler.WriteObjectToJsonFile($"D:/CricketData/{format}Matches/{matchDto.MatchNumber.Replace(" ", "_")}_{matchDto.MatchTitle}.json", matchDto);

                Document document = new Document()
                {
                    PageCount = 1,
                };

                document.SetMargins(10f, 10f, 10f, 25f);

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream($"D:/CricketData/{format}Matches/{matchDto.MatchNumber.Replace(" ", "_")}_{matchDto.MatchTitle}.pdf", FileMode.Create));

                //writer.PageEvent = new PdfWriterEvents("Vikas Jaiswal");

                document.Open();

                writer.PageEvent = new PageEventHelper();

                logger.LogInformation(string.Format("printing data for {0}", matchDto.MatchNumber));

                if (true)//document.NewPage())
                {
                    var matchResponse = mapper.Map<LimitedOverInternationalMatchInfoDTO>(matchDto).ToDomain(mapper);

                    var htmlLocalString = htmlString;

                    htmlLocalString = htmlLocalString.Replace("*#*MatchTitle*#*", matchDto.MatchTitle);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader(htmlLocalString));

                    document.AddMatchDetailsSection(
                        theme,
                        matchDto.Team1.Team.Name,
                        matchDto.Team2.Team.Name,
                        matchResponse);

                    PdfPTable table = new(2)
                    {
                        WidthPercentage = 100,
                        PaddingTop = 20,
                    };

                    table.AddScorBoardHeader($"{matchDto.Team1.Team.Name} Batting Scoreboard", matchDto.Team1.Team.Name, writer);

                    table.AddScorBoardHeader($"{matchDto.Team2.Team.Name} Batting Scoreboard", matchDto.Team2.Team.Name, writer);

                    table.AddCell(PDFHandlerExtensions.AddBattingScoreCard(
                        matchResponse.Team1.BattingScoreCard,
                        matchResponse.Team1.TotalInningDetails,
                        matchResponse.Team1.FallOfWickets,
                        matchResponse.Team1.DidNotBat,
                        theme));

                    table.AddCell(PDFHandlerExtensions.AddBattingScoreCard(
                        matchResponse.Team2.BattingScoreCard,
                        matchResponse.Team2.TotalInningDetails,
                        matchResponse.Team2.FallOfWickets,
                        matchResponse.Team2.DidNotBat,
                        theme));

                    table.AddScorBoardHeader($"{matchDto.Team2.Team.Name} Bowling Scoreboard", matchDto.Team2.Team.Name, writer);

                    table.AddScorBoardHeader($"{matchDto.Team1.Team.Name} Bowling Scoreboard", matchDto.Team1.Team.Name, writer);

                    table.AddCell(PDFHandlerExtensions.AddBowlingScoreCard(matchResponse.Team1.BowlingScoreCard, theme));

                    table.AddCell(PDFHandlerExtensions.AddBowlingScoreCard(matchResponse.Team2.BowlingScoreCard, theme));

                    table.AddCell(new PdfPCell(new Phrase(matchDto.Result, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE)))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 10,
                        BackgroundColor = BaseColor.RED,
                        BorderColor = BaseColor.WHITE,
                        BorderWidth = 2,
                        Colspan = 2,
                    });

                    table.Summary = "ODI Match PDF";

                    document.Add(table);

                    //logger.LogInformation(string.Format("printing matches{0}/{1}", document.PageNumber, matchesDto.Count()));
                }

                document.Close();
            }
        }

        public async Task AddPDFTestCricketMatch(IEnumerable<TestCricketMatchInfoDTO> cricketMatches)
        {
            var theme = new MatchTheme(
               BaseColorHelper.LIGHT_KHAKI,
               BaseColorHelper.PALE_LAVENDAR,
               BaseColorHelper.LIGHT_GRAY,
               BaseColorHelper.BRIGHT_UBE);

            var matchesDto = cricketMatches;

            foreach (var matchDto in matchesDto)
            {
                FileHandler.WriteObjectToJsonFile($"D:/CricketData/{CricketFormat.TestCricket}Matches/{matchDto.MatchNumber.Replace(" ", "_")}_{matchDto.MatchTitle}.json", matchDto);

                Document document = new Document()
                {
                    PageCount = matchesDto.Count(),
                };

                document.SetMargins(10f, 10f, 10f, 25f);

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream($"D:/CricketData/{CricketFormat.TestCricket}Matches/{matchDto.MatchNumber.Replace(" ", "_")}_{matchDto.MatchTitle}.pdf", FileMode.Create));

                document.Open();

                writer.PageEvent = new PageEventHelper();

                logger.LogInformation(string.Format("printing data for {0}", matchDto.MatchNumber));

                if (true)
                {
                    var matchResponse = mapper.Map<TestCricketMatchInfoDTO>(matchDto).ToDomain(mapper);

                    var htmlLocalString = htmlString;

                    htmlLocalString = htmlLocalString.Replace("*#*MatchTitle*#*", matchDto.MatchTitle);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader(htmlLocalString));

                    document.AddMatchDetailsSection(
                       theme,
                       matchDto.Team1.Team.Name,
                       matchDto.Team2.Team.Name,
                       matchResponse);

                    PdfPTable table = new(2)
                    {
                        WidthPercentage = 100,
                        PaddingTop = 20,
                    };

                    table.AddCell(new PdfPCell(new Phrase("First Inning ScoreBoard", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE)))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 10,
                        BackgroundColor = BaseColor.BLUE,
                        BorderColor = BaseColor.WHITE,
                        BorderWidth = 2,
                        Colspan = 2,
                    });

                    table.AddScorBoardHeader($"{matchDto.Team1.Team.Name} Batting Scoreboard", matchDto.Team1.Team.Name, writer);

                    table.AddScorBoardHeader($"{matchDto.Team2.Team.Name} Batting Scoreboard", matchDto.Team2.Team.Name, writer);

                    table.AddCell(PDFHandlerExtensions.AddBattingScoreCard(
                        matchResponse.Team1.Inning1.BattingScorecboard,
                        matchResponse.Team1.Inning1.TotalInningDetails,
                        matchResponse.Team1.Inning1.FallOfWickets,
                        matchResponse.Team1.Inning1.DidNotBat,
                        theme));

                    table.AddCell(PDFHandlerExtensions.AddBattingScoreCard(
                        matchResponse.Team2.Inning1.BattingScorecboard,
                        matchResponse.Team2.Inning1.TotalInningDetails,
                        matchResponse.Team2.Inning1.FallOfWickets,
                        matchResponse.Team2.Inning1.DidNotBat,
                        theme));

                    table.AddScorBoardHeader($"{matchDto.Team2.Team.Name} Bowling Scoreboard", matchDto.Team2.Team.Name, writer);

                    table.AddScorBoardHeader($"{matchDto.Team1.Team.Name} Bowling Scoreboard", matchDto.Team1.Team.Name, writer);

                    table.AddCell(PDFHandlerExtensions.AddBowlingScoreCard(matchResponse.Team1.Inning1.BowlingScoreboard, theme));

                    table.AddCell(PDFHandlerExtensions.AddBowlingScoreCard(matchResponse.Team2.Inning1.BowlingScoreboard, theme));

                    document.Add(table);

                    if (document.NewPage())
                    {
                        PdfPTable table2 = new PdfPTable(2)
                        {
                            WidthPercentage = 100,
                            PaddingTop = 20,
                        };

                        var runDiffFirstInning = matchResponse.Team1.Inning1.TotalInningDetails.Runs - matchResponse.Team2.Inning1.TotalInningDetails.Runs;

                        var leadingTeamName = (runDiffFirstInning > 0) ? matchDto.Team1.Team.Name : matchDto.Team2.Team.Name;

                        var trailString = $"{leadingTeamName} leading by {Math.Abs(runDiffFirstInning)} runs.";

                        table2.AddCell(new PdfPCell(new Phrase(trailString, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 10,
                            BackgroundColor = BaseColor.RED,
                            BorderColor = BaseColor.WHITE,
                            BorderWidth = 2,
                            Colspan = 2,
                        });

                        table2.AddCell(new PdfPCell(new Phrase("Second Inning ScoreBoard", new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 10,
                            BackgroundColor = BaseColor.BLUE,
                            BorderColor = BaseColor.WHITE,
                            BorderWidth = 2,
                            Colspan = 2,
                        });

                        table2.AddScorBoardHeader($"{matchDto.Team1.Team.Name} Batting Scoreboard", matchDto.Team1.Team.Name, writer);

                        table2.AddScorBoardHeader($"{matchDto.Team2.Team.Name} Batting Scoreboard", matchDto.Team2.Team.Name, writer);

                        table2.AddCell(PDFHandlerExtensions.AddBattingScoreCard(
                            matchResponse.Team1.Inning2.BattingScorecboard,
                            matchResponse.Team1.Inning2.TotalInningDetails,
                            matchResponse.Team1.Inning2.FallOfWickets,
                            matchResponse.Team1.Inning2.DidNotBat,
                            theme));

                        table2.AddCell(PDFHandlerExtensions.AddBattingScoreCard(
                            matchResponse.Team2.Inning2.BattingScorecboard,
                            matchResponse.Team2.Inning2.TotalInningDetails,
                            matchResponse.Team2.Inning2.FallOfWickets,
                            matchResponse.Team2.Inning2.DidNotBat,
                            theme));

                        table2.AddScorBoardHeader($"{matchDto.Team2.Team.Name} Bowling Scoreboard", matchDto.Team2.Team.Name, writer);

                        table2.AddScorBoardHeader($"{matchDto.Team1.Team.Name} Bowling Scoreboard", matchDto.Team1.Team.Name, writer);

                        table2.AddCell(PDFHandlerExtensions.AddBowlingScoreCard(matchResponse.Team1.Inning2.BowlingScoreboard, theme));

                        table2.AddCell(PDFHandlerExtensions.AddBowlingScoreCard(matchResponse.Team2.Inning2.BowlingScoreboard, theme));

                        table2.AddCell(new PdfPCell(new Phrase(matchDto.Result, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 10,
                            BackgroundColor = BaseColor.RED,
                            BorderColor = BaseColor.WHITE,
                            BorderWidth = 2,
                            Colspan = 2,
                        });

                        document.Add(table2);
                    }

                    logger.LogInformation(string.Format("printing matches{0}/{1}", document.PageNumber, matchesDto.Count()));
                }

                document.Close();
            }

        }

        public async Task AddPDFT20CricketMatch(IEnumerable<T20MatchInfoDTO> cricketMatches)
        {
            var theme = new MatchTheme(
               BaseColorHelper.LIGHT_KHAKI,
               BaseColorHelper.PEACH_ORANGE,
               BaseColorHelper.MEDIUM_SPRING_BUD,
               BaseColorHelper.BLIZZARD_BLUE);

            var matchesBySeason = cricketMatches
                .GroupBy(x => x.Season)
                .Select(x => new
                {
                    Season = x.Key,
                    Matches = x.ToList(),
                });

            foreach (var matchBySeason in matchesBySeason)
            {
                foreach (var matchDto in matchBySeason.Matches)
                {
                    Document document = new Document()
                    {
                        PageCount = 1,
                    };

                    document.SetMargins(10f, 10f, 10f, 25f);

                    var matchNumber = (matchBySeason.Matches.Where(x => x.Season.Equals(matchDto.Season)).ToList().IndexOf(matchDto) + 1).ToString();

                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream($"D:/CricketData/IPLMatches/IPL_{matchBySeason.Season.Replace("/", "-")}_MatchNumber-{matchNumber}_{matchDto.MatchTitle}.pdf", FileMode.Create));

                    //writer.PageEvent = new PdfWriterEvents("Vikas Jaiswal");

                    document.Open();

                    writer.PageEvent = new PageEventHelper();

                    logger.LogInformation(string.Format("printing data for {0}-{1}", matchDto.Season, matchDto.MatchTitle));

                    if (true)//document.NewPage())
                    {
                        var matchResponse = mapper.Map<T20MatchInfoDTO>(matchDto).ToDomain(mapper);

                        var htmlLocalString = htmlString;

                        htmlLocalString = htmlLocalString.Replace("*#*MatchTitle*#*", matchDto.MatchTitle);

                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader(htmlLocalString));

                        document.AddMatchDetailsSection(
                          theme,
                          matchDto.Team1.Team.Name,
                          matchDto.Team2.Team.Name,
                          matchResponse);

                        PdfPTable table = new PdfPTable(2)
                        {
                            WidthPercentage = 100,
                            PaddingTop = 20,
                        };

                        var team1Name = string.Join(string.Empty, matchDto.Team1.Team.Name.Split(" ").Select(x => x[0].ToString()));

                        var team2Name = string.Join(string.Empty, matchDto.Team2.Team.Name.Split(" ").Select(x => x[0].ToString()));

                        table.AddScorBoardHeader($"{team1Name} Batting Scoreboard", matchDto.Team1.Team.Name, writer);

                        table.AddScorBoardHeader($"{team2Name} Batting Scoreboard", matchDto.Team2.Team.Name, writer);

                        table.AddCell(PDFHandlerExtensions.AddBattingScoreCard(
                            matchResponse.Team1.BattingScoreCard,
                            matchResponse.Team1.TotalInningDetails,
                            matchResponse.Team1.FallOfWickets,
                            matchResponse.Team1.DidNotBat,
                            theme));

                        table.AddCell(PDFHandlerExtensions.AddBattingScoreCard(
                            matchResponse.Team2.BattingScoreCard,
                            matchResponse.Team2.TotalInningDetails,
                            matchResponse.Team2.FallOfWickets,
                            matchResponse.Team2.DidNotBat,
                            theme));

                        table.AddScorBoardHeader($"{team2Name} Bowling Scoreboard", matchDto.Team2.Team.Name, writer);

                        table.AddScorBoardHeader($"{team1Name} Bowling Scoreboard", matchDto.Team1.Team.Name, writer);

                        table.AddCell(PDFHandlerExtensions.AddBowlingScoreCard(matchResponse.Team1.BowlingScoreCard, theme));

                        table.AddCell(PDFHandlerExtensions.AddBowlingScoreCard(matchResponse.Team2.BowlingScoreCard, theme));

                        table.AddCell(new PdfPCell(new Phrase(matchDto.Result, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 10,
                            BackgroundColor = BaseColor.RED,
                            BorderColor = BaseColor.WHITE,
                            BorderWidth = 2,
                            Colspan = 2,
                        });

                        document.Add(table);

                        logger.LogInformation(string.Format("printing matches{0}/{1}", document.PageNumber, matchesBySeason.Count()));
                    }

                    document.Close();
                }
            }
        }
    }
}
