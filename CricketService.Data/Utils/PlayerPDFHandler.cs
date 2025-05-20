using System.Globalization;
using AutoMapper;
using CricketService.Data.Entities;
using CricketService.Data.Utils.Helpers;
using CricketService.Domain;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Extensions.Logging;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace CricketService.Data.Utils
{
    public class PlayerPDFHandler
    {
        private readonly ILogger<PlayerPDFHandler> logger;
        private readonly IMapper mapper;

        public PlayerPDFHandler(ILogger<PlayerPDFHandler> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddPDFCricketPlayerRecords(IEnumerable<CricketTeamPlayerInfos> cricketTeamsPlayersInfos)
        {
            var playersInfo = cricketTeamsPlayersInfos.GroupBy(x => x.PlayerUuid).Where(x => x.Key.Equals(new Guid("0995954d-3b12-4a98-8e2b-f50995d50694")))
                .Select(x => new
                {
                    PlayerName = x.Select(y => y.PlayerName).First(),
                    TeamsInfo = x.AsEnumerable(),
                    PlayerInfo = x.Select(y => y.PlayerInfo).First(),
                });

            StreamReader r1 = new StreamReader(@"D:\MyYoutubeRepos\repo\CricketService\CricketService.Data\Utils\CricketMatch.cshtml");
            var htmlString = r1.ReadToEnd();

            foreach (var playerInfo in playersInfo)
            {
                Document document = new Document()
                {
                    PageCount = 1,
                };

                document.SetMargins(10f, 10f, 25f, 25f);

                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream($"D:/CricketData/Players/{playerInfo.PlayerName}.pdf", FileMode.Create));

                document.Open();
                writer.PageEvent = new PageEventHelper();
                logger.LogInformation(string.Format("printing data for {0}", playerInfo.PlayerName));

                await AddPlayerImage(playerInfo.PlayerInfo.ImageUrl, playerInfo.PlayerInfo.Href);

                if (true)//document.NewPage())
                {
                    var htmlLocalString = htmlString;

                    htmlLocalString = htmlLocalString.Replace("*#*MatchTitle*#*", playerInfo.PlayerName);

                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader(htmlLocalString));

                    PdfPTable headerTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100,
                        PaddingTop = 10,
                    };

                    headerTable.DefaultCell.FixedHeight = 100;

                    headerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    headerTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    headerTable.SetWidths(new float[] { 2f, 3f });

                    headerTable.AddPlayerImage(writer, playerInfo.PlayerInfo.Href.Split("/").Last(), playerInfo.TeamsInfo.First().TeamName);

                    headerTable.AddCell(new PdfPCell(PDFHandlerExtensions.AddPlayerDetails(playerInfo.PlayerInfo)));

                    document.Add(headerTable);

                    foreach (var content in playerInfo.PlayerInfo.Contents)
                    {
                        var originalPhrase = new Phrase(content, new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL, BaseColor.BLACK));

                        var newPhrase = new Phrase();

                        foreach (var originalChunk in originalPhrase.Chunks)
                        {
                            var newChunk = new Chunk(originalChunk.Content, originalChunk.Font);
                            newChunk.SetBackground(BaseColor.YELLOW);
                            newPhrase.Add(newChunk);
                        }

                        var paragraph = new Paragraph(newPhrase)
                        {
                            FirstLineIndent = 5f,
                            IndentationLeft = 10f,
                            IndentationRight = 3f,
                            KeepTogether = true,
                            PaddingTop = 10,
                            Alignment = Element.ALIGN_JUSTIFIED,
                        };

                        document.Add(paragraph);
                    }

                    Dictionary<string, BaseColor> cellBgColors = new Dictionary<string, BaseColor>()
                    {
                        { "T20 International", CricketColorHelper.T20I_PLAYER_RECORD_CELL_BGCOLOR},
                        { "One-day International", CricketColorHelper.ODI_PLAYER_RECORD_CELL_BGCOLOR},
                        { "Test Cricket", CricketColorHelper.TEST_PLAYER_RECORD_CELL_BGCOLOR },
                    };

                    PdfPTable recordsTable = new PdfPTable(1)
                    {
                        WidthPercentage = 100,
                        PaddingTop = 30,
                        SplitRows = true,
                    };

                    //BATTING RECORDS

                    PdfPTable battingRecordsTable = new PdfPTable(15)
                    {
                        WidthPercentage = 100,
                        PaddingTop = 20,
                    };

                    battingRecordsTable.SetTotalWidth(new float[] {3f, 2f, 1f, 1f, 1f, 2f, 1f, 2f, 2f, 2f, 1f, 1f, 1f, 1f, 1f });

                    battingRecordsTable.AddCell(new PdfPCell(new Phrase("Batting Records", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.WHITE)))
                    {
                        Colspan = 15,
                        Padding = 10,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.BLACK,
                    });

                    AddPlayerRecordsCell(battingRecordsTable, "Subtitle", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "Span", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "Matches", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "Inn", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "NO", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "Runs", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "HS", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "Avg", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "BF", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "SR", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "100s", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "50s", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "0s", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "4s", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(battingRecordsTable, "6s", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);

                    foreach (var format in new string[] { "T20 International", "One-day International", "Test Cricket" })
                    {
                        var formatRecords = AddFormatHeader(battingRecordsTable, format, playerInfo.TeamsInfo, 15);

                        var battingStatsByGroup = formatRecords.BattingStatistics.GroupBy(x => x.Title);

                        var cellBgColor = cellBgColors[format];

                        foreach (var bsbg in battingStatsByGroup)
                        {
                            battingRecordsTable.AddCell(new PdfPCell(new Phrase(bsbg.Key.Replace("_", " ").ToLower(), new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.BLACK)))
                            {
                                Colspan = 15,
                                Padding = 10,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                            });

                            foreach (var bs in bsbg)
                            {
                                AddPlayerRecordsCell(battingRecordsTable, bs.SubTitle.ToString(), BaseColor.DARK_GRAY, Font.BOLD, 20, BaseColor.WHITE);
                                AddPlayerRecordsCell(battingRecordsTable, (bs.Span ?? string.Empty)!, BaseColor.DARK_GRAY, Font.BOLD, 20, BaseColor.WHITE);
                                AddPlayerRecordsCell(battingRecordsTable, bs.Matches.ToString(), cellBgColor);
                                AddPlayerRecordsCell(battingRecordsTable, bs.Innings.ToString(), cellBgColor);
                                AddPlayerRecordsCell(battingRecordsTable, bs.NotOut.ToString(), cellBgColor);
                                AddPlayerRecordsCell(battingRecordsTable, bs.Runs.ToString(), cellBgColor, Font.BOLD);
                                AddPlayerRecordsCell(battingRecordsTable, bs.HighestScore, cellBgColor, Font.BOLD);
                                AddPlayerRecordsCell(battingRecordsTable, Math.Round(bs.Average, 2).ToString(), cellBgColor);
                                AddPlayerRecordsCell(battingRecordsTable, bs.BallsFaced.ToString(), cellBgColor);
                                AddPlayerRecordsCell(battingRecordsTable, Math.Round(bs.StrikeRate, 2).ToString(), cellBgColor);
                                AddPlayerRecordsCell(battingRecordsTable, bs.Centuries.ToString(), cellBgColor, Font.BOLD);
                                AddPlayerRecordsCell(battingRecordsTable, bs.HalfCenturies.ToString(), cellBgColor, Font.BOLD);
                                AddPlayerRecordsCell(battingRecordsTable, bs.Ducks.ToString(), cellBgColor);
                                AddPlayerRecordsCell(battingRecordsTable, bs.Fours.ToString(), cellBgColor);
                                AddPlayerRecordsCell(battingRecordsTable, bs.Sixes.ToString(), cellBgColor);
                            }
                        }
                    }

                    recordsTable.AddCell(battingRecordsTable);

                    //BOWLING RECORDS
                    PdfPTable bowlingRecordsTable = new PdfPTable(15)
                    {
                        WidthPercentage = 100,
                        PaddingTop = 20,
                    };

                    bowlingRecordsTable.SetTotalWidth(new float[] { 3f, 2f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f });

                    bowlingRecordsTable.AddCell(new PdfPCell(new Phrase("Bowling Records", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.WHITE)))
                    {
                        Colspan = 15,
                        Padding = 10,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.BLACK,
                    });

                    AddPlayerRecordsCell(bowlingRecordsTable, "Subtitle", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "Span", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "Matches", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "Inn", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "Overs", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "Mdns", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "Runs", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "Wickets", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "BBI", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "BBM", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "Avg", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "Eco", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "SR", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "5w", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(bowlingRecordsTable, "10w", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);

                    foreach (var format in new string[] { "T20 International", "One-day International", "Test Cricket" })
                    {
                        var formatRecords = AddFormatHeader(bowlingRecordsTable, format, playerInfo.TeamsInfo, 15);

                        var bowlingStatsByGroup = formatRecords.BowlingStatistics.GroupBy(x => x.Title);

                        var cellBgColor = cellBgColors[format];

                        foreach (var bsbg in bowlingStatsByGroup)
                        {
                            bowlingRecordsTable.AddCell(new PdfPCell(new Phrase(bsbg.Key.Replace("_", " ").ToLower(), new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.BLACK)))
                            {
                                Colspan = 15,
                                Padding = 10,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                            });

                            foreach (var bs in bsbg)
                            {
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.SubTitle.ToString(), BaseColor.DARK_GRAY, Font.BOLD, 20, BaseColor.WHITE);
                                AddPlayerRecordsCell(bowlingRecordsTable, (bs.Span ?? string.Empty)!, BaseColor.DARK_GRAY, Font.BOLD, 20, BaseColor.WHITE);
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.Matches.ToString(), cellBgColor);
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.Innings.ToString(), cellBgColor);
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.Overs.Overs.ToString(), cellBgColor);
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.Maidens.ToString(), cellBgColor);
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.RunsConceded.ToString(), cellBgColor);
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.Wickets.ToString(), cellBgColor, Font.BOLD);
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.BestBowlingInning.ToString(), cellBgColor);
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.BestBowlingMatch.ToString(), cellBgColor);
                                AddPlayerRecordsCell(bowlingRecordsTable, Math.Round(bs.Average, 2).ToString(), cellBgColor);
                                AddPlayerRecordsCell(bowlingRecordsTable, Math.Round(bs.Economy, 2).ToString(), cellBgColor);
                                AddPlayerRecordsCell(bowlingRecordsTable, Math.Round(bs.StrikeRate, 2).ToString(), cellBgColor);
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.FiveWicketsHaul.ToString(), cellBgColor, Font.BOLD);
                                AddPlayerRecordsCell(bowlingRecordsTable, bs.TenWicketsHaul.ToString(), cellBgColor, Font.BOLD);
                            }
                        }
                    }

                    recordsTable.AddCell(bowlingRecordsTable);

                    //FIELDING RECORDS
                    PdfPTable fieldingRecordsTable = new PdfPTable(9)
                    {
                        WidthPercentage = 100,
                        PaddingTop = 20,
                    };

                    fieldingRecordsTable.SetTotalWidth(new float[] { 2f, 2f, 1f, 1f, 1f, 1f, 1f, 1f, 1f });

                    fieldingRecordsTable.AddCell(new PdfPCell(new Phrase("Fielding Records", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.WHITE)))
                    {
                        Colspan = 9,
                        Padding = 10,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.BLACK,
                    });

                    AddPlayerRecordsCell(fieldingRecordsTable, "Subtitle", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(fieldingRecordsTable, "Span", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(fieldingRecordsTable, "Matches", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(fieldingRecordsTable, "Inn", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(fieldingRecordsTable, "Dismissal", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(fieldingRecordsTable, "Caught", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(fieldingRecordsTable, "Stumping", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(fieldingRecordsTable, "CWK", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);
                    AddPlayerRecordsCell(fieldingRecordsTable, "CF", BaseColor.BLUE, Font.BOLD, 30, BaseColor.WHITE);

                    foreach (var format in new string[] { "T20 International", "One-day International", "Test Cricket" })
                    {
                        var formatRecords = AddFormatHeader(fieldingRecordsTable, format, playerInfo.TeamsInfo, 9);

                        var fieldingStatsByGroup = formatRecords.FieldingStatistics.GroupBy(x => x.Title);

                        var cellBgColor = cellBgColors[format];

                        foreach (var fsbg in fieldingStatsByGroup)
                        {
                            fieldingRecordsTable.AddCell(new PdfPCell(new Phrase(fsbg.Key.Replace("_", " ").ToLower(), new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.BLACK)))
                            {
                                Colspan = 9,
                                Padding = 10,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                            });

                            foreach (var bs in fsbg)
                            {
                                AddPlayerRecordsCell(fieldingRecordsTable, bs.SubTitle.ToString(), BaseColor.DARK_GRAY, Font.BOLD, 20, BaseColor.WHITE);
                                AddPlayerRecordsCell(fieldingRecordsTable, (bs.Span ?? string.Empty)!, BaseColor.DARK_GRAY, Font.BOLD, 20, BaseColor.WHITE);
                                AddPlayerRecordsCell(fieldingRecordsTable, bs.Matches.ToString(), cellBgColor);
                                AddPlayerRecordsCell(fieldingRecordsTable, bs.Innings.ToString(), cellBgColor);
                                AddPlayerRecordsCell(fieldingRecordsTable, bs.Dismissal.ToString(), cellBgColor, Font.BOLD);
                                AddPlayerRecordsCell(fieldingRecordsTable, bs.Caught.ToString(), cellBgColor, Font.BOLD);
                                AddPlayerRecordsCell(fieldingRecordsTable, bs.Stumpings.ToString(), cellBgColor, Font.BOLD);
                                AddPlayerRecordsCell(fieldingRecordsTable, bs.CaughtKeeper.ToString(), cellBgColor);
                                AddPlayerRecordsCell(fieldingRecordsTable, bs.CaughtFielder.ToString(), cellBgColor);
                            }
                        }
                    }

                    recordsTable.AddCell(fieldingRecordsTable);

                    document.Add(recordsTable);
                }

                document.Close();
            }
        }

        private static CareerInfo AddFormatHeader(PdfPTable table, string format, IEnumerable<CricketTeamPlayerInfos> teamsInfo, int colSpan)
        {
            CareerInfo formatRecords = teamsInfo.First().CareerStatistics.T20Career;
            BaseColor headerBgColor = CricketColorHelper.T20I_PLAYER_RECORD_CELL_BGCOLOR;

            if (format == "One-day International")
            {
                formatRecords = teamsInfo.First().CareerStatistics.ODICareer;
                headerBgColor = CricketColorHelper.ODI_PLAYER_RECORD_CELL_BGCOLOR;
            }

            if (format == "Test Cricket")
            {
                formatRecords = teamsInfo.First().CareerStatistics.TestCareer;
                headerBgColor = CricketColorHelper.TEST_PLAYER_RECORD_CELL_BGCOLOR;
            }

            table.AddCell(new PdfPCell(new Phrase(format, new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.BLACK)))
            {
                Colspan = colSpan,
                Padding = 10,
                HorizontalAlignment = Element.ALIGN_CENTER,
                BackgroundColor = headerBgColor,
            });

            return formatRecords;
        }

        private static void AddPlayerRecordsCell(PdfPTable table, string cellText, BaseColor? bgColor = null!, int font = Font.NORMAL, int fixedHeight = 20, BaseColor? color = null!)
        {
            table.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, font, color ?? BaseColor.BLACK)))
            {
                BackgroundColor = bgColor ?? BaseColorHelper.LIGHT_KHAKI,
                BorderColor = new BaseColor(0, 128, 0),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                FixedHeight = fixedHeight,
            });
        }

        public static async Task AddPlayerImage(string imageUrl, string playerUrl)
        {
            string filePath = $"D:/MyYoutubeRepos/repo/CricketService/CricketService.Data/StaticData/PlayerImage/{playerUrl.Split("/").Last()}.png";

            if (!File.Exists(filePath))
            {
                HttpClient client = new HttpClient();

                Console.WriteLine(imageUrl);
                Console.WriteLine($"Calling https://img1.hscicdn.com/image/upload/f_auto,t_ds_square_w_640,q_50/lsci{imageUrl}");
                HttpResponseMessage response = await client.GetAsync($"https://img1.hscicdn.com/image/upload/f_auto,t_ds_square_w_640,q_50/lsci{imageUrl}");

                if (response.IsSuccessStatusCode)
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                    FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

                    await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);

                    fileStream.Close();

                    Console.WriteLine("Image downloaded successfully.");
                }
                else
                {
                    File.WriteAllText($"D:/MyYoutubeRepos/repo/CricketService/CricketService.Data/StaticData/failedUrls.txt", imageUrl);
                    Console.WriteLine("Image downloaded Failed.");
                }
            }
        }
    }
}
