import React, { useEffect } from "react";
import "./Playing11ScoreInfo.scss";
import {
  PlayerShortInfo,
  useAllPlayersUuids,
} from "../../hooks/useAllPlayersUuids";
import { Team } from "../../../models/espn-cricinfo-models/CricketMatchModels";
import { PlayerScoreCards } from "./elements/PlayerScoreCards";
import playerPictures from "../../../data/StaticData/playerPictures.json";
import { RotatingCylinder } from "../../common/RotatingCylinder";
import { speakText } from "../../common/SpeakText";

interface Playing11ScoreInfoProps {
  team1: Team;
  team2: Team;
  selectedIndex: number;
}

export const Playing11ScoreInfo: React.FC<Playing11ScoreInfoProps> = ({
  team1,
  team2,
  selectedIndex,
}) => {
  const { isLoading, playerInfos } = useAllPlayersUuids();

  const playing11Team1 = team1.battingScorecard
    .map((x) => x.playerName)
    .concat(team1.didNotBat);

  const playing11Team2 = team2.battingScorecard
    .map((x) => x.playerName)
    .concat(team2.didNotBat);

  const getTotalScoreArr = (team: Team) => {
    const totalRunsByBatsman = team.battingScorecard
      .map((bs) => bs.runsScored || 0)
      .reduce((a, b) => a + b, 0);
    const totalExtras = team.extras
      .replace("(", "")
      .replace(")", "")
      .split(", ")
      .map((x) => parseInt(x.split(" ")[1]))
      .reduce((a, b) => a + b, 0);
    const totalRuns = totalRunsByBatsman + totalExtras;

    const totalBalls =
      team.battingScorecard
        .map((x) => x.ballsFaced || 0)
        .reduce((a, b) => a + b, 0) -
      (team.extras?.includes("nb")
        ? parseInt(team.extras.split("nb ")[1].split(",")[0])
        : 0);

    const totalOvers = `${Math.floor(totalBalls / 6)}.${totalBalls % 6} overs`;
    const falledWickets = team.fallOfWickets.filter((x) =>
      /^[1-9]|10/.test(x)
    ).length;
    return [totalRuns, falledWickets, totalOvers];
  };

  const getTotalScore = (team: Team) => {
    const [totalRuns, falledWickets, totalOvers] = getTotalScoreArr(team);
    return `${totalRuns}/${falledWickets} (${totalOvers})`;
  };

  // const getTotalScoreSpeech = (team: Team) => {
  //   const totalScore = getTotalScore(team);
  //   const runs = totalScore.split("/")[0];
  //   const wickets = totalScore.split("/")[1]

  //   return totalScore.split("");
  // };

  useEffect(() => {
    if (selectedIndex === 3) {
      const [totalRuns, falledWickets, totalOvers] = getTotalScoreArr(team1);
      speakText(
        `${team1.teamName} total inning score ${totalRuns} at loss of ${falledWickets} wickets, after ${totalOvers}`
      );
    }
    if (selectedIndex === 4) {
      const [totalRuns, falledWickets, totalOvers] = getTotalScoreArr(team2);
      speakText(
        `${team2.teamName} total inning score ${totalRuns} at loss of ${falledWickets} wickets, after ${totalOvers}`
      );
      // speakText(`Ireland women win by 79 runs.`);
    }

    return () => window.speechSynthesis.cancel();
  }, [selectedIndex]);

  return (
    <div
      style={{
        height: 860,
        background: 'url("https://wallpapercave.com/wp/wp3049846.jpg")',
        backgroundSize: "100% 100%",
      }}
    >
      {selectedIndex === 3 && (
        <div className="playing-11" style={{ background: "none" }}>
          <h1>
            <span style={{ fontSize: 35 }}>{team1.teamName}</span>{" "}
            {getTotalScore(team1)}
          </h1>
          <PlayerScoreCards
            playerInfos={playerInfos as PlayerShortInfo[]}
            playing11Team={playing11Team1}
            battingScoreCard={team1.battingScorecard}
            bowlingScoreCard={team2.bowlingScorecard}
            teamName={team1.teamName}
          />
        </div>
      )}
      {selectedIndex === 4 && (
        <div className="playing-11" style={{ background: "none" }}>
          <h1>
            <span style={{ fontSize: 35 }}>{team2.teamName}</span>{" "}
            {getTotalScore(team2)}
          </h1>
          <PlayerScoreCards
            playerInfos={playerInfos as PlayerShortInfo[]}
            playing11Team={playing11Team2}
            battingScoreCard={team2.battingScorecard}
            bowlingScoreCard={team1.bowlingScorecard}
            teamName={team2.teamName}
          />
        </div>
      )}
    </div>
  );
};
