import React from "react";
import "./Playing11ScoreInfo.scss";
import {
  PlayerShortInfo,
  useAllPlayersUuids,
} from "../../hooks/useAllPlayersUuids";
import {
  InningDetail,
  TestTeam,
} from "../../../models/espn-cricinfo-models/CricketMatchModels";
import { PlayerScoreCards } from "./elements/PlayerScoreCards";
import playerPictures from "../../../data/StaticData/playerPictures.json";

interface TestPlaying11ScoreInfoProps {
  team1: TestTeam;
  team2: TestTeam;
  selectedIndex: number;
}

export const TestPlaying11ScoreInfo: React.FC<TestPlaying11ScoreInfoProps> = ({
  team1,
  team2,
  selectedIndex,
}) => {
  const { isLoading, playerInfos } = useAllPlayersUuids();

  const playing11Team1 = team1.inning1.battingScorecard
    .map((x) => x.playerName)
    .concat(team1.inning1.didNotBat);

  const playing11Team2 = team2.inning1.battingScorecard
    .map((x) => x.playerName)
    .concat(team2.inning1.didNotBat);

  const getTotalScore = (inningDetail: InningDetail) => {
    const totalRunsByBatsman = inningDetail.battingScorecard
      .map((bs) => bs.runsScored || 0)
      .reduce((a, b) => a + b, 0);
    const totalExtras = inningDetail.extras
      .replace("(", "")
      .replace(")", "")
      .split(", ")
      .map((x) => parseInt(x.split(" ")[1]))
      .reduce((a, b) => a + b, 0);
    const totalRuns = totalRunsByBatsman + totalExtras;

    const totalBalls =
      inningDetail.battingScorecard
        .map((x) => x.ballsFaced || 0)
        .reduce((a, b) => a + b, 0) -
      (inningDetail.extras?.includes("nb")
        ? parseInt(inningDetail.extras.split("nb ")[1].split(",")[0])
        : 0);

    const totalOvers = `${Math.floor(totalBalls / 6)}.${totalBalls % 6} overs`;
    const falledWickets = inningDetail.fallOfWickets.filter((x) =>
      /^[1-9]|10/.test(x)
    ).length;
    return `${totalRuns}/${falledWickets} (${totalOvers})`;
  };

  const obj: { href: string; profilePics: string[] }[] = [];

  playing11Team1.forEach((x) =>
    obj.push({
      href: x.href,
      profilePics: [
        playerPictures?.find((p) => p.href === x.href)?.profilePics[0] ||
          playerInfos?.find((p) => p.href === x.href)?.imageUrl ||
          "https://t4.ftcdn.net/jpg/02/23/50/73/360_F_223507349_F5RFU3kL6eMt5LijOaMbWLeHUTv165CB.jpg",
      ],
    })
  );

  playing11Team2.forEach((x) =>
    obj.push({
      href: x.href,
      profilePics: [
        playerPictures?.find((p) => p.href === x.href)?.profilePics[0] ||
          playerInfos?.find((p) => p.href === x.href)?.imageUrl ||
          "https://t4.ftcdn.net/jpg/02/23/50/73/360_F_223507349_F5RFU3kL6eMt5LijOaMbWLeHUTv165CB.jpg",
      ],
    })
  );

  console.log(obj);
  return (
    <div>
      {selectedIndex === 2 && (
        <div className="playing-11">
          <h1>
            <i>
              1<sup>st</sup> Inning -{" "}
            </i>
            <span style={{ fontSize: 35 }}>{team1.teamName}</span>{" "}
            {getTotalScore(team1.inning1)}
          </h1>
          <PlayerScoreCards
            playerInfos={playerInfos as PlayerShortInfo[]}
            playing11Team={playing11Team1}
            battingScoreCard={team1.inning1.battingScorecard}
            bowlingScoreCard={team2.inning1.bowlingScorecard}
          />
        </div>
      )}
      {selectedIndex === 3 && (
        <div className="playing-11">
          <h1>
            <i>
              2<sup>nd</sup> Inning -{" "}
            </i>
            <span style={{ fontSize: 35 }}>{team2.teamName}</span>{" "}
            {getTotalScore(team2.inning1)}
          </h1>
          <PlayerScoreCards
            playerInfos={playerInfos as PlayerShortInfo[]}
            playing11Team={playing11Team2}
            battingScoreCard={team2.inning1.battingScorecard}
            bowlingScoreCard={team1.inning1.bowlingScorecard}
          />
        </div>
      )}
      {selectedIndex === 4 && (
        <div className="playing-11">
          <h1>
            <i>
              3<sup>rd</sup> Inning -{" "}
            </i>
            <span style={{ fontSize: 35 }}>{team1.teamName}</span>{" "}
            {getTotalScore(team1.inning2)}
          </h1>
          <PlayerScoreCards
            playerInfos={playerInfos as PlayerShortInfo[]}
            playing11Team={playing11Team1}
            battingScoreCard={team1.inning2.battingScorecard}
            bowlingScoreCard={team2.inning2.bowlingScorecard}
          />
        </div>
      )}
      {selectedIndex === 5 && (
        <div className="playing-11">
          <h1>
            <i>
              4<sup>th</sup> Inning -{" "}
            </i>
            <span style={{ fontSize: 35 }}>{team2.teamName}</span>{" "}
            {getTotalScore(team2.inning2)}
          </h1>
          <PlayerScoreCards
            playerInfos={playerInfos as PlayerShortInfo[]}
            playing11Team={playing11Team2}
            battingScoreCard={team2.inning2.battingScorecard}
            bowlingScoreCard={team1.inning2.bowlingScorecard}
          />
        </div>
      )}
    </div>
  );
};
