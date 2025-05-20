import React, { useState } from "react";
import { MatchBasicInfo } from "./screens/MatchBasicInfo";
import $ from "jquery";
import { MatchFinalInfo } from "./screens/MatchFinalInfo";
import {
  Batsman,
  Bowler,
  Player,
} from "../../models/espn-cricinfo-models/CricketMatchModels";
import { useFetchTestMatchByUrl } from "../../hooks/espn-cricinfo-hooks/useFetchTestMatchByUrl";
import { TestPlaying11ScoreInfo } from "./screens/TestPlaying11ScoreInfo";
import { MomentCaptures } from "./screens/elements/MomentCaptures";
import { H2HMatchRecords } from "./screens/H2HMatchRecords";
import { CricketFormat } from "../../models/enums/CricketFormat";

interface TestCricketMatchProps {
  selectedMatchUrl: string;
  selectedScreenIndex: number;
}

export const TestCricketMatch: React.FC<TestCricketMatchProps> = ({
  selectedMatchUrl,
  selectedScreenIndex,
}) => {
  const {
    matchNo,
    matchTitle,
    matchDays,
    result,
    venue,
    series,
    tossWinner,
    tossDecision,
    team1,
    team2,
    playerOfTheMatch,
  } = useFetchTestMatchByUrl(selectedMatchUrl);

  return (
    <div>
      {selectedScreenIndex === 0 && (
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
      {selectedScreenIndex === 1 && (
        <H2HMatchRecords
          team1Captain={
            team1.inning1.battingScorecard
              .map((x) => x.playerName)
              .concat(team1.inning1.didNotBat)
              .find(
                (x) =>
                  x.name?.includes("(c)") && !x.name?.includes("Did not bat")
              ) as Player
          }
          team2Captain={
            team2.inning1.battingScorecard
              .map((x) => x.playerName)
              .concat(team2.inning1.didNotBat)
              .find(
                (x) =>
                  x.name?.includes("(c)") && !x.name?.includes("Did not bat")
              ) as Player
          }
          format={CricketFormat.Test}
          team1Name={team1.teamName}
          team2Name={team2.teamName}
        />
      )}
      {[2, 3, 4, 5].includes(selectedScreenIndex) && (
        <TestPlaying11ScoreInfo
          team1={team1}
          team2={team2}
          selectedIndex={selectedScreenIndex}
        />
      )}
      {selectedScreenIndex === 6 && (
        <MatchFinalInfo
          matchResult={result}
          playerOfTheMatch={playerOfTheMatch}
          potmBattingStats={team1.inning1.battingScorecard
            .concat(team2.inning1.battingScorecard)
            .concat(team1.inning2.battingScorecard)
            .concat(team2.inning2.battingScorecard)
            .filter((x) => x.playerName.name.trim() === playerOfTheMatch)}
          potmBowlingStats={team1.inning1.bowlingScorecard
            .concat(team2.inning1.bowlingScorecard)
            .concat(team1.inning2.bowlingScorecard)
            .concat(team2.inning2.bowlingScorecard)
            .filter((x) => x.playerName.name === playerOfTheMatch)}
        />
      )}
      {selectedScreenIndex === 7 && <MomentCaptures />}
    </div>
  );
};
