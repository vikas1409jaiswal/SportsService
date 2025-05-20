using CricketService.Data.Entities;
using CricketService.Data.Utils.Helpers;
using CricketService.Domain.BaseDomains;
using CricketService.Domain.Enums;
using CricketService.Domain.ResponseDomains;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CricketService.Data.Utils
{
    public static class PDFHandlerExtensions
    {
        public static void AddLogoImage(this PdfPTable table, string teamName)
        {
            string basePath = Directory.GetCurrentDirectory();
            string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\"));
            string imagePath = Path.Combine(projectRoot, "CricketService.Data", "StaticData", "TeamImage");

            Image img;
            try
            {
                img = Image.GetInstance(Path.Combine(imagePath, $"{teamName}.jpg"));
            }
            catch
            {
                try
                {
                    img = Image.GetInstance(Path.Combine(imagePath, $"{teamName}.png"));
                }
                catch
                {
                    img = Image.GetInstance(Path.Combine(imagePath, "India.png"));
                }
            }

            table.AddCell(img);
        }

        public static void AddPlayerImage(this PdfPTable table, PdfWriter writer, string playerUrl, string teamName)
        {
            Console.WriteLine(playerUrl);

            Image img;
            try
            {
                img = Image.GetInstance($"D:/MyYoutubeRepos/repo/CricketService/CricketService.Data/StaticData/PlayerImage/{playerUrl}.jpg");
            }
            catch
            {
                try
                {
                    img = Image.GetInstance($"D:/MyYoutubeRepos/repo/CricketService/CricketService.Data/StaticData/PlayerImage/{playerUrl}.png");
                }
                catch
                {
                    img = Image.GetInstance($"D:/MyYoutubeRepos/repo/CricketService/CricketService.Data/StaticData/TeamImage/India.png");
                }
            }

            var width = 350;
            var height = 400;

            PdfTemplate template = PdfTemplate.CreateTemplate(writer, width, height);
            template.Rectangle(0, 0, width, height);
            template.SetColorFill(BaseColorHelper.LIGHT_KHAKI);
            template.Fill();
            template.AddImage(Image.GetInstance($"D:/MyYoutubeRepos/repo/CricketService/CricketService.Data/StaticData/TeamImage/{teamName}.png"), width, 0, 0, height, 0, 0);
            template.AddImage(img, width, 0, 0, height, 0, 0);

            Image pdfImage = Image.GetInstance(new ImgTemplate(template));

            table.AddCell(pdfImage);
        }

        #region CricketMatchHelper
        public static void AddMatchDetailsSection(
          this Document document,
          MatchTheme theme,
          string team1Name,
          string team2Name,
          CricketMatchBase matchDetails)
        {
            PdfPTable headerTable = new(3)
            {
                WidthPercentage = 100,
                PaddingTop = 10,
            };

            headerTable.DefaultCell.FixedHeight = 100;

            headerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            headerTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            headerTable.SetWidths(new float[] { 1f, 4f, 1f });

            headerTable.AddLogoImage(team1Name);

            headerTable.AddCell(new PdfPCell(AddMatchDetails(matchDetails, theme)));

            headerTable.AddLogoImage(team2Name);

            document.Add(headerTable);
        }

        public static PdfPTable AddMatchDetails(CricketMatchBase matchData, MatchTheme theme)
        {
            PdfPTable table = new(2)
            {
                WidthPercentage = 100,
                PaddingTop = 30,
            };
            table.SetWidths(new float[] { 1f, 3f });
            Dictionary<string, string> matchDetailPairs = new()
            {
                { "Match Number", matchData.MatchNumber },
                { "Season", matchData.Season },
                { "Series", matchData.Series },
                { "Match Title", matchData.MatchTitle },
                { "Result", matchData.Result },
                { "Toss", matchData.TossDecision },
                { "Venue", matchData.Venue },
            };

            if (matchData.PlayerOfTheMatch is not null)
            {
                matchDetailPairs.Add("Player Of The Match", matchData.PlayerOfTheMatch.PlayerName);
            }

            foreach (var kvp in matchDetailPairs)
            {
                table.AddCell(new PdfPCell(new Phrase(kvp.Key, new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, BaseColor.BLACK)))
                {
                    BackgroundColor = theme.MatchDetailKeyBGColor,
                    BorderColor = BaseColor.BLACK,
                });
                table.AddCell(new PdfPCell(new Phrase(kvp.Value, new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK)))
                {
                    BackgroundColor = theme.MatchDetailValueBGColor,
                    BorderColor = BaseColor.BLACK,
                });
            }

            return table;
        }

        public static void AddScorBoardHeader(
            this PdfPTable table,
            string headerText,
            string teamName,
            PdfWriter writer)
        {
            PdfPTable headertable = new(3);

            headertable.SetTotalWidth(new float[] { 1f, 8f, 1f });

            headertable.DefaultCell.FixedHeight = 20;

            headertable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            headertable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

            var cell = new PdfPCell(new Phrase(headerText, new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 10,
                BackgroundColor = BaseColor.BLACK,
                BorderColor = BaseColor.WHITE,
                BorderWidth = 2,
            };

            headertable.AddLogoImage(teamName);
            headertable.AddCell(cell);
            headertable.AddLogoImage(teamName);

            table.AddCell(headertable);
        }

        public static PdfPTable AddBattingScoreCard(
            IEnumerable<BattingScoreboardResponse> battingScoreCard,
            TotalInningScore totalInningScore,
            string[] fallOfWickets,
            CricketPlayer[] didNotBat,
            MatchTheme theme)
        {
            PdfPTable battingTable = new(5)
            {
                WidthPercentage = 100,
            };
            battingTable.SetWidths(new float[] { 5f, 1f, 1f, 0.75f, 0.75f });

            battingTable.AddScorboardCell("Player Name", 10, 15, theme.BattingScoreBoardHeaderBGColor);
            battingTable.AddScorboardCell("Runs", 10, 15, theme.BattingScoreBoardHeaderBGColor);
            battingTable.AddScorboardCell("Balls", 10, 15, theme.BattingScoreBoardHeaderBGColor);
            battingTable.AddScorboardCell("4s", 10, 15, theme.BattingScoreBoardHeaderBGColor);
            battingTable.AddScorboardCell("6s", 10, 15, theme.BattingScoreBoardHeaderBGColor);

            if (battingScoreCard.Count() == 0)
            {
                battingTable.AddCell(new PdfPCell(new Phrase("Inning not started", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, BaseColor.BLACK)))
                {
                    BorderWidth = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    FixedHeight = 330,
                    Colspan = 6,
                });
            }

            foreach (var batsman in battingScoreCard)
            {
                var playerCell = new PdfPTable(1)
                {
                   WidthPercentage = 100,
                };
                playerCell.AddCell(new PdfPCell(new Phrase(batsman.PlayerName.Name, new Font(Font.FontFamily.HELVETICA, 6, Font.BOLD, BaseColor.BLACK)))
                {
                    BorderWidth = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                });

                playerCell.AddCell(new PdfPCell(new Phrase(batsman.OutStatus, new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                {
                    BorderWidth = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                });

                var isNotOut = batsman.OutStatus.Contains("not out");
                var outerCellBgColor = isNotOut ? BaseColor.LIGHT_GRAY : null;

                battingTable.AddScorboardCell(string.Empty, 25, 30, null, outerCellBgColor, null, Font.NORMAL, new PdfPCell(playerCell) { FixedHeight = 25, BackgroundColor = BaseColor.CYAN });
                battingTable.AddScorboardCell(batsman.RunsScored.ToString()!, 15, 30, theme.BattingScoreBoardBodyBGColor, outerCellBgColor, null, Font.BOLD);
                battingTable.AddScorboardCell(batsman.BallsFaced.ToString()!, 15, 30, theme.BattingScoreBoardBodyBGColor, outerCellBgColor);
                battingTable.AddScorboardCell(batsman.Fours.ToString()!, 15, 30, theme.BattingScoreBoardBodyBGColor, outerCellBgColor);
                battingTable.AddScorboardCell(batsman.Sixes.ToString()!, 15, 30, theme.BattingScoreBoardBodyBGColor, outerCellBgColor);
            }

            foreach (var player in didNotBat)
            {
                var playerCell = new PdfPTable(1);
                playerCell.AddCell(new PdfPCell(new Phrase(player.Name, new Font(Font.FontFamily.HELVETICA, 6, Font.BOLD, BaseColor.BLACK)))
                {
                    BorderWidth = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                });
                playerCell.AddCell(new PdfPCell(new Phrase("did not bat", new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
                {
                    BorderWidth = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                });
                battingTable.AddScorboardCell(string.Empty, 25, 30, null, null, null, Font.NORMAL, new PdfPCell(playerCell) { FixedHeight = 25, BackgroundColor = BaseColor.YELLOW });
                for (int i = 0; i < 4; i++)
                {
                    battingTable.AddScorboardCell("-", 15, 30, theme.BattingScoreBoardBodyBGColor);
                }
            }

            battingTable.AddScorboardCell("Extras", 10, 15, BaseColorHelper.PALE_LAVENDAR);
            battingTable.AddCell(new PdfPCell(new Phrase(totalInningScore.Extras.TotalExtras.ToString(), new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            {
                Colspan = 5,
                HorizontalAlignment = Element.ALIGN_CENTER,
                FixedHeight = 15,
            });

            battingTable.AddScorboardCell("Total", 10, 15, BaseColor.BLUE, null, BaseColor.WHITE);
            battingTable.AddCell(new PdfPCell(new Phrase($"{totalInningScore.Runs}/{totalInningScore.Wickets} ({totalInningScore.Overs.Overs} overs)", new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            {
                Colspan = 5,
                HorizontalAlignment = Element.ALIGN_CENTER,
                FixedHeight = 15,
                BackgroundColor = BaseColorHelper.PEACH_ORANGE,
            });

            battingTable.AddCell(new PdfPCell(new Phrase(string.Format("fall of wickets: {0}", string.Join(", ", fallOfWickets.Take(10))), new Font(Font.FontFamily.HELVETICA, 6, Font.NORMAL, BaseColor.BLACK)))
            {
                Colspan = 6,
                HorizontalAlignment = Element.ALIGN_LEFT,
            });

            return battingTable;
        }

        public static PdfPTable AddBowlingScoreCard(
            ICollection<BowlingScoreboardResponse> bowlingScoreCard,
            MatchTheme theme)
        {
            PdfPTable bowlingTable = new PdfPTable(6)
            {
                WidthPercentage = 100,
            };
            bowlingTable.SetWidths(new float[] { 3f, 1f, 1f, 1f, 1f, 1f });

            bowlingTable.AddScorboardCell("Player Name", 10, 15, theme.BowlingScoreBoardHeaderBGColor);
            bowlingTable.AddScorboardCell("Wickets", 10, 15, theme.BowlingScoreBoardHeaderBGColor);
            bowlingTable.AddScorboardCell("Overs", 10, 15, theme.BowlingScoreBoardHeaderBGColor);
            bowlingTable.AddScorboardCell("Runs", 10, 15, theme.BowlingScoreBoardHeaderBGColor);
            bowlingTable.AddScorboardCell("WB", 10, 15, theme.BowlingScoreBoardHeaderBGColor);
            bowlingTable.AddScorboardCell("NB", 10, 15, theme.BowlingScoreBoardHeaderBGColor);

            if (bowlingScoreCard.Count == 0)
            {
                bowlingTable.AddCell(new PdfPCell(new Phrase("Inning not started", new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD, BaseColor.BLACK)))
                {
                    BorderWidth = 0,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    FixedHeight = 90,
                    Colspan = 6,
                });
            }

            foreach (var bowler in bowlingScoreCard)
            {
                bowlingTable.AddScorboardCell(bowler.PlayerName.Name, 10, 15, BaseColor.WHITE, null, null, Font.BOLD);
                bowlingTable.AddScorboardCell(bowler.Wickets.ToString(), 10, 15, theme.BowlingScoreBoardBodyBGColor, null, null, Font.BOLD);
                bowlingTable.AddScorboardCell(bowler.OversBowled.ToString(), 10, 15, theme.BowlingScoreBoardBodyBGColor);
                bowlingTable.AddScorboardCell(bowler.RunsConceded.ToString(), 10, 15, theme.BowlingScoreBoardBodyBGColor);
                bowlingTable.AddScorboardCell(bowler.WideBall.ToString(), 10, 15, theme.BowlingScoreBoardBodyBGColor);
                bowlingTable.AddScorboardCell(bowler.NoBall.ToString(), 10, 15, theme.BowlingScoreBoardBodyBGColor);
            }

            return bowlingTable;
        }

        private static void AddScorboardCell(
            this PdfPTable table,
            string cellText,
            int innerCellfixedHeight,
            int outerCellfixedHeight,
            BaseColor? innerBgColor = null,
            BaseColor? outerBgColor = null,
            BaseColor? textColor = null,
            int fontStyle = Font.NORMAL,
            PdfPCell? innerPdfPCell = null)
        {
            // Create outer cell (transparent, no border)
            var outerCell = new PdfPCell
            {
                //Border = PdfPCell.NO_BORDER,
                BackgroundColor = outerBgColor ?? BaseColor.WHITE,
                FixedHeight = outerCellfixedHeight,
                PaddingTop = (outerCellfixedHeight - innerCellfixedHeight) * 0.45f, // This creates the margin between outer and inner cells
                PaddingLeft = 2,
                PaddingRight = 2,
            };

            // Create inner cell with styling
            var innerCell = innerPdfPCell ?? new PdfPCell(new Phrase(cellText,
                new Font(Font.FontFamily.HELVETICA, 6, fontStyle, textColor ?? BaseColor.BLACK)))
            {
                BackgroundColor = innerBgColor ?? BaseColor.YELLOW,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                FixedHeight = innerCellfixedHeight,
                Padding = 1, // Inner padding for text
            };

            // Create a nested table to hold the inner cell
            var innerTable = new PdfPTable(1);
            innerTable.AddCell(innerCell);

            // Add the nested table to the outer cell
            outerCell.AddElement(innerTable);

            // Add the outer cell to the main table
            table.AddCell(outerCell);
        }
        #endregion

        #region CricketTeamHelper
        public static PdfPTable AddTeamDetails(CricketTeamInfoDTO teamInfo)
        {
            PdfPTable table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                PaddingTop = 30,
            };

            table.SetWidths(new float[] { 1f, 2f });

            foreach (var format in teamInfo.Formats)
            {
                if (format == CricketFormat.T20I.ToString())
                {
                    Dictionary<string, string> teamDetailPairs = new Dictionary<string, string>();

                    var mileStones = teamInfo.T20IRecords.TeamMileStones;

                    table.AddCell(new PdfPCell(new Phrase("T20 International", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.BLACK)))
                    {
                        Colspan = 2,
                        Padding = 6,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    });

                    teamDetailPairs.Add("Debut", string.Format("{0} vs {1}", teamInfo.T20IRecords.Debut.Date, teamInfo.T20IRecords.Debut.Opponent));
                    teamDetailPairs.Add("Most Innings", string.Format("{0} ({1}) Runs", mileStones.MostInnings.Key, mileStones.MostInnings.Value));
                    teamDetailPairs.Add("Most Runs", string.Format("{0} ({1}) Runs", mileStones.MostRuns.Key, mileStones.MostRuns.Value));
                    teamDetailPairs.Add("Most Wickets", string.Format("{0} ({1}) Wickets", mileStones.MostWickets.Key, mileStones.MostWickets.Value));
                    teamDetailPairs.Add("Most Sixes", string.Format("{0} ({1}) Sixes", mileStones.MostSixes.Key, mileStones.MostSixes.Value));
                    teamDetailPairs.Add("Most Fours", string.Format("{0} ({1}) Fours", mileStones.MostFours.Key, mileStones.MostFours.Value));
                    teamDetailPairs.Add("Best Bowling", string.Format("{0} ({1})", mileStones.BestBowlingInning.Key, mileStones.BestBowlingInning.Value));
                    teamDetailPairs.Add("Highest Individual Score", string.Format("{0} ({1})", mileStones.HighestIndividualScore.Key, mileStones.HighestIndividualScore.Value));

                    foreach (var kvp in teamDetailPairs)
                    {
                        table.AddCell(new PdfPCell(new Phrase(kvp.Key, new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK)))
                        {
                            BackgroundColor = BaseColorHelper.LIGHT_KHAKI,
                            BorderColor = BaseColor.BLACK,
                        });
                        table.AddCell(new PdfPCell(new Phrase(kvp.Value, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)))
                        {
                            BackgroundColor = BaseColorHelper.PEACH_ORANGE,
                            BorderColor = BaseColor.BLACK,
                        });
                    }
                }

                if (format == CricketFormat.ODI.ToString())
                {
                    Dictionary<string, string> teamDetailPairs = new Dictionary<string, string>();

                    var mileStones = teamInfo.ODIRecords.TeamMileStones;

                    table.AddCell(new PdfPCell(new Phrase("One-day International", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.BLACK)))
                    {
                        Colspan = 2,
                        Padding = 6,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    });

                    teamDetailPairs.Add("Debut", string.Format("{0} vs {1}", teamInfo.ODIRecords.Debut.Date, teamInfo.ODIRecords.Debut.Opponent));
                    teamDetailPairs.Add("Most Innings", string.Format("{0} ({1}) Runs", mileStones.MostInnings.Key, mileStones.MostInnings.Value));
                    teamDetailPairs.Add("Most Runs", string.Format("{0} ({1}) Runs", mileStones.MostRuns.Key, mileStones.MostRuns.Value));
                    teamDetailPairs.Add("Most Wickets", string.Format("{0} ({1}) Wickets", mileStones.MostWickets.Key, mileStones.MostWickets.Value));
                    teamDetailPairs.Add("Most Sixes", string.Format("{0} ({1}) Sixes", mileStones.MostSixes.Key, mileStones.MostSixes.Value));
                    teamDetailPairs.Add("Most Fours", string.Format("{0} ({1}) Fours", mileStones.MostFours.Key, mileStones.MostFours.Value));
                    teamDetailPairs.Add("Best Bowling", string.Format("{0} ({1})", mileStones.BestBowlingInning.Key, mileStones.BestBowlingInning.Value));
                    teamDetailPairs.Add("Highest Individual Score", string.Format("{0} ({1})", mileStones.HighestIndividualScore.Key, mileStones.HighestIndividualScore.Value));

                    foreach (var kvp in teamDetailPairs)
                    {
                        table.AddCell(new PdfPCell(new Phrase(kvp.Key, new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK)))
                        {
                            BackgroundColor = BaseColorHelper.LIGHT_KHAKI,
                            BorderColor = BaseColor.BLACK,
                        });
                        table.AddCell(new PdfPCell(new Phrase(kvp.Value, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)))
                        {
                            BackgroundColor = BaseColorHelper.PEACH_ORANGE,
                            BorderColor = BaseColor.BLACK,
                        });
                    }
                }

                if (format == CricketFormat.TestCricket.ToString())
                {
                    Dictionary<string, string> teamDetailPairs = new Dictionary<string, string>();

                    var mileStones = teamInfo.TestRecords.TeamMileStones;

                    table.AddCell(new PdfPCell(new Phrase("Test Cricket", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.BLACK)))
                    {
                        Colspan = 2,
                        Padding = 6,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                    });

                    teamDetailPairs.Add("Test Debut", string.Format("{0} vs {1}", teamInfo.TestRecords.Debut.Date, teamInfo.TestRecords.Debut.Opponent));
                    teamDetailPairs.Add("Most Innings", string.Format("{0} ({1}) Runs", mileStones.MostInnings.Key, mileStones.MostInnings.Value));
                    teamDetailPairs.Add("Most Runs", string.Format("{0} ({1}) Runs", mileStones.MostRuns.Key, mileStones.MostRuns.Value));
                    teamDetailPairs.Add("Most Wickets", string.Format("{0} ({1}) Wickets", mileStones.MostWickets.Key, mileStones.MostWickets.Value));
                    teamDetailPairs.Add("Most Sixes", string.Format("{0} ({1}) Sixes", mileStones.MostSixes.Key, mileStones.MostSixes.Value));
                    teamDetailPairs.Add("Most Fours", string.Format("{0} ({1}) Fours", mileStones.MostFours.Key, mileStones.MostFours.Value));
                    teamDetailPairs.Add("Best Bowling", string.Format("{0} ({1})", mileStones.BestBowlingInning.Key, mileStones.BestBowlingInning.Value));
                    teamDetailPairs.Add("Highest Individual Score", string.Format("{0} ({1})", mileStones.HighestIndividualScore.Key, mileStones.HighestIndividualScore.Value));

                    foreach (var kvp in teamDetailPairs)
                    {
                        table.AddCell(new PdfPCell(new Phrase(kvp.Key, new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK)))
                        {
                            BackgroundColor = BaseColorHelper.LIGHT_KHAKI,
                            BorderColor = BaseColor.BLACK,
                        });
                        table.AddCell(new PdfPCell(new Phrase(kvp.Value, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)))
                        {
                            BackgroundColor = BaseColorHelper.PEACH_ORANGE,
                            BorderColor = BaseColor.BLACK,
                        });
                    }
                }
            }

            return table;
        }
        #endregion

        #region CricketPlayerHelper
        public static PdfPTable AddPlayerDetails(CricketPlayerInfoDTO cricketPlayerInfo)
        {
            PdfPTable table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                PaddingTop = 30,
            };

            table.SetWidths(new float[] { 1f, 2f });

            Dictionary<string, string> matchDetailPairs = new Dictionary<string, string>
            {
                { "Full Name", cricketPlayerInfo.FullName },
                { "Date Of Birth", new DateTime(cricketPlayerInfo.DateOfBirth!.Year, cricketPlayerInfo.DateOfBirth!.Month ?? 0, cricketPlayerInfo.DateOfBirth.Date ?? 40).ToString("dd MMM yyyy") },
                { "Playing Role", cricketPlayerInfo.ExtraInfo.PlayingRole },
                { "Batting Style", cricketPlayerInfo.ExtraInfo.BattingStyle },
                { "Batting Style1", cricketPlayerInfo.ExtraInfo.BattingStyle },
                { "Batting Style2", cricketPlayerInfo.ExtraInfo.BattingStyle },
                { "Batting Style3", cricketPlayerInfo.ExtraInfo.BattingStyle },
                { "Batting Style4", cricketPlayerInfo.ExtraInfo.BattingStyle },
                { "Batting Style5", cricketPlayerInfo.ExtraInfo.BattingStyle },
                { "Bowling Style6", cricketPlayerInfo.ExtraInfo.BowlingStyle },
                { "Internatinal Career Span", cricketPlayerInfo.ExtraInfo.InternationalCareerSpan },
                { "Height", cricketPlayerInfo.ExtraInfo.Height },
            };

            foreach (var kvp in matchDetailPairs)
            {
                table.AddCell(new PdfPCell(new Phrase(kvp.Key, new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.BLACK)))
                {
                    BackgroundColor = BaseColorHelper.LIGHT_KHAKI,
                    BorderColor = BaseColor.BLACK,
                    Padding = 5,
                });
                table.AddCell(new PdfPCell(new Phrase(kvp.Value, new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK)))
                {
                    BackgroundColor = BaseColorHelper.PEACH_ORANGE,
                    BorderColor = BaseColor.BLACK,
                    Padding = 5,
                });
            }

            return table;

            //foreach (var format in teamInfo.Formats)
            //{
            //    if (format == CricketFormat.T20I.ToString())
            //    {
            //        Dictionary<string, string> teamDetailPairs = new Dictionary<string, string>();

            //        var mileStones = teamInfo.T20IRecords.TeamMileStones;

            //        table.AddCell(new PdfPCell(new Phrase("T20 International", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.BLACK)))
            //        {
            //            Colspan = 2,
            //            Padding = 6,
            //            HorizontalAlignment = Element.ALIGN_CENTER,
            //        });

            //        teamDetailPairs.Add("Debut", string.Format("{0} vs {1}", teamInfo.T20IRecords.Debut.Date, teamInfo.T20IRecords.Debut.Opponent));
            //        teamDetailPairs.Add("Most Innings", string.Format("{0} ({1}) Runs", mileStones.MostInnings.Key, mileStones.MostInnings.Value));
            //        teamDetailPairs.Add("Most Runs", string.Format("{0} ({1}) Runs", mileStones.MostRuns.Key, mileStones.MostRuns.Value));
            //        teamDetailPairs.Add("Most Wickets", string.Format("{0} ({1}) Wickets", mileStones.MostWickets.Key, mileStones.MostWickets.Value));
            //        teamDetailPairs.Add("Most Sixes", string.Format("{0} ({1}) Sixes", mileStones.MostSixes.Key, mileStones.MostSixes.Value));
            //        teamDetailPairs.Add("Most Fours", string.Format("{0} ({1}) Fours", mileStones.MostFours.Key, mileStones.MostFours.Value));
            //        teamDetailPairs.Add("Best Bowling", string.Format("{0} ({1})", mileStones.BestBowlingInning.Key, mileStones.BestBowlingInning.Value));
            //        teamDetailPairs.Add("Highest Individual Score", string.Format("{0} ({1})", mileStones.HighestIndividualScore.Key, mileStones.HighestIndividualScore.Value));

            //        foreach (var kvp in teamDetailPairs)
            //        {
            //            table.AddCell(new PdfPCell(new Phrase(kvp.Key, new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK)))
            //            {
            //                BackgroundColor = BaseColorHelper.LIGHT_KHAKI,
            //                BorderColor = BaseColor.BLACK,
            //            });
            //            table.AddCell(new PdfPCell(new Phrase(kvp.Value, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)))
            //            {
            //                BackgroundColor = BaseColorHelper.PEACH_ORANGE,
            //                BorderColor = BaseColor.BLACK,
            //            });
            //        }
            //    }

            //    if (format == CricketFormat.ODI.ToString())
            //    {
            //        Dictionary<string, string> teamDetailPairs = new Dictionary<string, string>();

            //        var mileStones = teamInfo.ODIRecords.TeamMileStones;

            //        table.AddCell(new PdfPCell(new Phrase("One-day International", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.BLACK)))
            //        {
            //            Colspan = 2,
            //            Padding = 6,
            //            HorizontalAlignment = Element.ALIGN_CENTER,
            //        });

            //        teamDetailPairs.Add("Debut", string.Format("{0} vs {1}", teamInfo.ODIRecords.Debut.Date, teamInfo.ODIRecords.Debut.Opponent));
            //        teamDetailPairs.Add("Most Innings", string.Format("{0} ({1}) Runs", mileStones.MostInnings.Key, mileStones.MostInnings.Value));
            //        teamDetailPairs.Add("Most Runs", string.Format("{0} ({1}) Runs", mileStones.MostRuns.Key, mileStones.MostRuns.Value));
            //        teamDetailPairs.Add("Most Wickets", string.Format("{0} ({1}) Wickets", mileStones.MostWickets.Key, mileStones.MostWickets.Value));
            //        teamDetailPairs.Add("Most Sixes", string.Format("{0} ({1}) Sixes", mileStones.MostSixes.Key, mileStones.MostSixes.Value));
            //        teamDetailPairs.Add("Most Fours", string.Format("{0} ({1}) Fours", mileStones.MostFours.Key, mileStones.MostFours.Value));
            //        teamDetailPairs.Add("Best Bowling", string.Format("{0} ({1})", mileStones.BestBowlingInning.Key, mileStones.BestBowlingInning.Value));
            //        teamDetailPairs.Add("Highest Individual Score", string.Format("{0} ({1})", mileStones.HighestIndividualScore.Key, mileStones.HighestIndividualScore.Value));

            //        foreach (var kvp in teamDetailPairs)
            //        {
            //            table.AddCell(new PdfPCell(new Phrase(kvp.Key, new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK)))
            //            {
            //                BackgroundColor = BaseColorHelper.LIGHT_KHAKI,
            //                BorderColor = BaseColor.BLACK,
            //            });
            //            table.AddCell(new PdfPCell(new Phrase(kvp.Value, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)))
            //            {
            //                BackgroundColor = BaseColorHelper.PEACH_ORANGE,
            //                BorderColor = BaseColor.BLACK,
            //            });
            //        }
            //    }

            //    if (format == CricketFormat.TestCricket.ToString())
            //    {
            //        Dictionary<string, string> teamDetailPairs = new Dictionary<string, string>();

            //        var mileStones = teamInfo.TestRecords.TeamMileStones;

            //        table.AddCell(new PdfPCell(new Phrase("Test Cricket", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD, BaseColor.BLACK)))
            //        {
            //            Colspan = 2,
            //            Padding = 6,
            //            HorizontalAlignment = Element.ALIGN_CENTER,
            //        });

            //        teamDetailPairs.Add("Test Debut", string.Format("{0} vs {1}", teamInfo.TestRecords.Debut.Date, teamInfo.TestRecords.Debut.Opponent));
            //        teamDetailPairs.Add("Most Innings", string.Format("{0} ({1}) Runs", mileStones.MostInnings.Key, mileStones.MostInnings.Value));
            //        teamDetailPairs.Add("Most Runs", string.Format("{0} ({1}) Runs", mileStones.MostRuns.Key, mileStones.MostRuns.Value));
            //        teamDetailPairs.Add("Most Wickets", string.Format("{0} ({1}) Wickets", mileStones.MostWickets.Key, mileStones.MostWickets.Value));
            //        teamDetailPairs.Add("Most Sixes", string.Format("{0} ({1}) Sixes", mileStones.MostSixes.Key, mileStones.MostSixes.Value));
            //        teamDetailPairs.Add("Most Fours", string.Format("{0} ({1}) Fours", mileStones.MostFours.Key, mileStones.MostFours.Value));
            //        teamDetailPairs.Add("Best Bowling", string.Format("{0} ({1})", mileStones.BestBowlingInning.Key, mileStones.BestBowlingInning.Value));
            //        teamDetailPairs.Add("Highest Individual Score", string.Format("{0} ({1})", mileStones.HighestIndividualScore.Key, mileStones.HighestIndividualScore.Value));

            //        foreach (var kvp in teamDetailPairs)
            //        {
            //            table.AddCell(new PdfPCell(new Phrase(kvp.Key, new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK)))
            //            {
            //                BackgroundColor = BaseColorHelper.LIGHT_KHAKI,
            //                BorderColor = BaseColor.BLACK,
            //            });
            //            table.AddCell(new PdfPCell(new Phrase(kvp.Value, new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK)))
            //            {
            //                BackgroundColor = BaseColorHelper.PEACH_ORANGE,
            //                BorderColor = BaseColor.BLACK,
            //            });
            //        }
            //    }
            //}
        }
        #endregion
    }
}
