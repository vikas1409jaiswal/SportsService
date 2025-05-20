using AutoMapper;
using CricketService.Data.Entities;
using CricketService.Data.Utils.Helpers;
using CricketService.Domain;
using CricketService.Domain.Common;
using CricketService.Domain.Enums;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Extensions.Logging;

namespace CricketService.Data.Utils
{
    public class TeamPDFHandler
    {
        private readonly ILogger<TeamPDFHandler> logger;
        private readonly IMapper mapper;

        public TeamPDFHandler(ILogger<TeamPDFHandler> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddPDFCricketTeamRecords(IEnumerable<CricketTeamPlayerInfos> cricketTeamsPlayersInfos)
        {
            var teamsInfo = cricketTeamsPlayersInfos.GroupBy(x => x.TeamName).Where(x => x.Key.Equals("New Zealand"))
                .Select(x => new
                {
                    TeamName = x.Key,
                    TeamInfo = x.Select(y => y.TeamInfo).First(),
                    PlayersInfo = x.AsEnumerable(),
                });

            StreamReader r1 = new StreamReader(@"D:\MyYoutubeRepos\repo\CricketService\CricketService.Data\Utils\CricketMatch.cshtml");
            var htmlString = r1.ReadToEnd();

            foreach (var teamInfo in teamsInfo)
            {
                Document document = new Document()
                {
                    PageCount = 1,
                };

                document.SetMargins(10f, 10f, 10f, 25f);

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream($"D:/CricketData/Teams/{teamInfo.TeamName}.pdf", FileMode.Create));

                document.Open();
                writer.PageEvent = new PageEventHelper();
                logger.LogInformation(string.Format("printing data for {0}", teamInfo.TeamName));

                if (true)//document.NewPage())
                {
                    var htmlLocalString = htmlString;

                    htmlLocalString = htmlLocalString.Replace("*#*MatchTitle*#*", teamInfo.TeamName);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader(htmlLocalString));

                    PdfPTable headerTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        PaddingTop = 10,
                    };

                    headerTable.DefaultCell.FixedHeight = 100;

                    headerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    headerTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    headerTable.SetWidths(new float[] { 1f, 3f });

                    headerTable.AddLogoImage(teamInfo.TeamName);

                    headerTable.AddCell(new PdfPCell(PDFHandlerExtensions.AddTeamDetails(teamInfo.TeamInfo)));

                    document.Add(headerTable);

                    PdfPTable teamTable = new PdfPTable(8)
                    {
                        WidthPercentage = 100,
                        PaddingTop = 20,
                    };

                    teamTable.SetTotalWidth(new float[] { 3f, 1f, 1f, 1f, 1f, 1f, 1f, 2f });

                    AddTeamCell(teamTable, "Total", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddTeamCell(teamTable, "Matches", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddTeamCell(teamTable, "Won", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddTeamCell(teamTable, "Lost", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddTeamCell(teamTable, "Tied", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddTeamCell(teamTable, "NR/Draw", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddTeamCell(teamTable, "Win %", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddTeamCell(teamTable, "First Match", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);

                    foreach (var format in new string[] { "T20 International", "One-day International", "Test Cricket" })
                    {
                        object formatRecords = teamInfo.TeamInfo.T20IRecords;

                        if (format == "One-day International")
                        {
                            formatRecords = teamInfo.TeamInfo.ODIRecords;
                        }

                        if (format == "Test Cricket")
                        {
                            formatRecords = teamInfo.TeamInfo.TestRecords;
                        }

                        teamTable.AddCell(new PdfPCell(new Phrase(format, new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.BLACK)))
                        {
                            Colspan = 8,
                            Padding = 10,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                        });

                        AddTeamCell(teamTable, "Total", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                        AddTeamCell(teamTable, ((TeamFormatRecordDetails)formatRecords).Matches.ToString(), BaseColor.DARK_GRAY, Font.BOLD, 30, BaseColor.WHITE);
                        AddTeamCell(teamTable, ((TeamFormatRecordDetails)formatRecords).Won.ToString(), BaseColor.DARK_GRAY, Font.BOLD, 30, BaseColor.WHITE);
                        AddTeamCell(teamTable, ((TeamFormatRecordDetails)formatRecords).Lost.ToString(), BaseColor.DARK_GRAY, Font.BOLD, 30, BaseColor.WHITE);
                        AddTeamCell(teamTable, ((TeamFormatRecordDetails)formatRecords).Tied.ToString(), BaseColor.DARK_GRAY, Font.BOLD, 30, BaseColor.WHITE);
                        AddTeamCell(teamTable, ((TeamFormatRecordDetails)formatRecords).NRorDraw.ToString(), BaseColor.DARK_GRAY, Font.BOLD, 30, BaseColor.WHITE);
                        AddTeamCell(teamTable, ((TeamFormatRecordDetails)formatRecords).WinPercentage.ToString(), BaseColor.DARK_GRAY, Font.BOLD, 30, BaseColor.WHITE);
                        AddTeamCell(teamTable, ((TeamFormatRecordDetails)formatRecords).Debut.Date.ToString(), BaseColor.DARK_GRAY, Font.BOLD, 30, BaseColor.WHITE);

                        //foreach (var matchesAgaistTeam in ((TeamFormatRecordDetails)formatRecords).RecordsAgainstTeams)
                        //{
                        //    AddTeamCell(teamTable, matchesAgaistTeam.Opponent, BaseColor.RED, Font.BOLD, 30, BaseColor.WHITE);
                        //    AddTeamCell(teamTable, matchesAgaistTeam.Matches.ToString());
                        //    AddTeamCell(teamTable, matchesAgaistTeam.Won.ToString());
                        //    AddTeamCell(teamTable, matchesAgaistTeam.Lost.ToString());
                        //    AddTeamCell(teamTable, matchesAgaistTeam.Tied.ToString());
                        //    AddTeamCell(teamTable, matchesAgaistTeam.NRorDraw.ToString());
                        //    AddTeamCell(teamTable, matchesAgaistTeam.NRorDraw.ToString());
                        //    AddTeamCell(teamTable, matchesAgaistTeam.MatchesBySeason.First().Matches.First().Date.ToString());
                        //}
                    }

                    document.Add(teamTable);

                    PdfPTable playerTable = new PdfPTable(7)
                    {
                        WidthPercentage = 100,
                        PaddingTop = 20,
                    };

                    playerTable.AddCell(new PdfPCell(new Phrase("Players", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.BLACK)))
                    {
                        Colspan = 7,
                        Padding = 10,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    });

                    playerTable.SetTotalWidth(new float[] { 1f, 3f, 2f, 2f, 2f, 2f, 2f });

                    AddPlayerCell(playerTable, "SN", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerCell(playerTable, "Player Name", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerCell(playerTable, "Formats", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerCell(playerTable, "Date of Birth", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerCell(playerTable, "Playing Role", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerCell(playerTable, "Batting Role", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerCell(playerTable, "Bowling Role", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);

                    foreach (var (player, index) in teamInfo.PlayersInfo.OrderBy(x => string.Join(", ", x.PlayerInfo.Formats)).Select((player, index) => (player, index)))
                    {
                        var extraInfo = player.PlayerInfo.ExtraInfo;

                        AddPlayerCell(playerTable, (index + 1).ToString());
                        AddPlayerCell(playerTable, player.PlayerName, null, Font.BOLD);
                        AddPlayerCell(playerTable, string.Join(", ", player.PlayerInfo.Formats));
                        AddPlayerCell(playerTable, player.PlayerInfo.DateOfBirth.ToString());
                        AddPlayerCell(playerTable, extraInfo.PlayingRole);
                        AddPlayerCell(playerTable, extraInfo.BattingStyle);
                        AddPlayerCell(playerTable, extraInfo.BowlingStyle);
                    }

                    document.Add(playerTable);
                }

                document.Close();
            }
        }

        private static void AddPlayerCell(PdfPTable table, string cellText, BaseColor? bgColor = null!, int font = Font.NORMAL, int fixedHeight = 30, BaseColor? color = null!)
        {
            table.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, font, color ?? BaseColor.BLACK)))
            {
                BackgroundColor = bgColor ?? BaseColorHelper.PALE_LAVENDAR,
                BorderColor = new BaseColor(0, 128, 0),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                FixedHeight = fixedHeight,
            });
        }

        private static void AddTeamCell(PdfPTable table, string cellText, BaseColor? bgColor = null!, int font = Font.NORMAL, int fixedHeight = 30, BaseColor? color = null!)
        {
            table.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 12, font, color ?? BaseColor.BLACK)))
            {
                BackgroundColor = bgColor ?? BaseColorHelper.LIGHT_KHAKI,
                BorderColor = new BaseColor(0, 128, 0),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                FixedHeight = fixedHeight,
            });
        }
    }
}
