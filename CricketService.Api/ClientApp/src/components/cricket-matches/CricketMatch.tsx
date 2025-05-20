import React, { useEffect } from "react";
import { MatchBasicInfo } from "./screens/MatchBasicInfo";
import { Playing11ScoreInfo } from "./screens/Playing11ScoreInfo";
import { MatchFinalInfo } from "./screens/MatchFinalInfo";
import {
  Batsman,
  Bowler,
  Player,
} from "../../models/espn-cricinfo-models/CricketMatchModels";
import { useFetchMatchByUrl } from "../../hooks/espn-cricinfo-hooks/useFetchMatchByUrl";
import { H2HMatchRecords } from "./screens/H2HMatchRecords";
import { CricketFormat } from "../../models/enums/CricketFormat";
import { MomentCaptures } from "./screens/elements/MomentCaptures";
import { useGeneratePDFForMatch } from "../../hooks/espn-cricinfo-hooks/useGeneratePdfForMatch";
import { Playing11Info } from "./screens/Playing11Info";
import { Top5Players } from "./screens/Top5Players";
import { PointsTable } from "./screens/PointsTable";

interface CricketMatchProps {
  selectedMatchUrl: string;
  selectedScreenIndex: number;
  format: CricketFormat;
}

export const CricketMatch: React.FC<CricketMatchProps> = ({
  selectedMatchUrl,
  selectedScreenIndex,
  format,
}) => {
  const cricketMatch = useFetchMatchByUrl(
    `/series/nepal-tri-nation-t20i-series-2023-24-1403272/nepal-vs-hong-kong-3rd-match-1403290/full-scorecard`
  );

  const {
    matchNo,
    matchTitle,
    matchDays,
    result,
    venue,
    series,
    tossWinner,
    tossDecision,
    matchSquad,
    pointsTable,
    team1,
    team2,
    playerOfTheMatch,
  } = cricketMatch;

  const generatePDFForMatch = useGeneratePDFForMatch(cricketMatch);

  useEffect(() => {
    const handleKeyDown = (event: any) => {
      if (event.key === "d" || event.key === "D") {
        generatePDFForMatch(cricketMatch);
      }
    };

    document.addEventListener("keydown", handleKeyDown);

    return () => {
      document.removeEventListener("keydown", handleKeyDown);
    };
  }, [generatePDFForMatch]);

  // const graph = useFetchMatchStatsByUrl(selectedMatchUrl);

  // $(document).ready(function () {
  //   $(".match-graph").html(graph);
  // });

  const screenIndexes = {
    matchBasicInfo: 0,
    headToHead: 1,
    playing11Info: 2,
    inning1Scores: 3,
    inning2Scores: 4,
    matchResultInfo: 5,
    matchPhotos: 6,
    top5Player: 8,
    pointsTable: 7,
  };

  const tournaments = {
    caribbean_premier_league_2023: "caribbean-premier-league-2023-15350",
  };

  return (
    <div>
      {selectedScreenIndex === screenIndexes.matchBasicInfo && (
        <MatchBasicInfo
          href={selectedMatchUrl}
          matchNumber={matchNo}
          matchTitle={matchTitle}
          matchDate={matchDays}
          matchVenue={venue}
          matchSeries={series}
          tossWinner={tossWinner}
          tossResult={tossDecision?.split(", ")[1]}
        />
      )}
      {[screenIndexes.inning1Scores, screenIndexes.inning2Scores].includes(
        selectedScreenIndex
      ) && (
        <Playing11ScoreInfo
          team1={team1}
          team2={team2}
          selectedIndex={selectedScreenIndex}
        />
      )}
      {selectedScreenIndex === screenIndexes.matchResultInfo && (
        <MatchFinalInfo
          matchResult={result}
          playerOfTheMatch={playerOfTheMatch}
          potmBattingStats={[
            team1.battingScorecard
              .concat(team2.battingScorecard)
              .find((x) =>
                x.playerName.name?.includes(playerOfTheMatch)
              ) as Batsman,
          ]}
          potmBowlingStats={[
            team1.bowlingScorecard
              .concat(team2.bowlingScorecard)
              .find((x) =>
                x.playerName.name?.includes(playerOfTheMatch)
              ) as Bowler,
          ]}
        />
      )}
      {selectedScreenIndex === screenIndexes.headToHead && (
        <H2HMatchRecords
          team1Captain={
            team1.battingScorecard
              .map((x) => x.playerName)
              .concat(team1.didNotBat)
              .find(
                (x) =>
                  x.name?.includes("(c)") && !x.name?.includes("Did not bat")
              ) as Player
          }
          team2Captain={
            team2.battingScorecard
              .map((x) => x.playerName)
              .concat(team2.didNotBat)
              .find(
                (x) =>
                  x.name?.includes("(c)") && !x.name?.includes("Did not bat")
              ) as Player
          }
          format={format}
          team1Name={team1.teamName}
          team2Name={team2.teamName}
        />
      )}
      {selectedScreenIndex === screenIndexes.top5Player && (
        <Top5Players tournamentId={tournaments.caribbean_premier_league_2023} />
      )}
      {selectedScreenIndex === screenIndexes.matchPhotos && <MomentCaptures />}
      {selectedScreenIndex === screenIndexes.playing11Info && matchSquad && (
        <Playing11Info matchSquad={matchSquad} />
      )}
      {selectedScreenIndex === screenIndexes.pointsTable &&
        pointsTable.length > 0 && <PointsTable pointsTableRows={pointsTable} />}
      {selectedScreenIndex === 9 && <div className="match-graph"></div>}
    </div>
  );
};
