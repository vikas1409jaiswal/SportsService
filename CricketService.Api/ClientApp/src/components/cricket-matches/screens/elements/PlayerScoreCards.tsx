import React from "react";
import {
  Batsman,
  Bowler,
  Player,
} from "../../../../models/espn-cricinfo-models/CricketMatchModels";
import { PlayerShortInfo } from "../../../hooks/useAllPlayersUuids";
import { PlayerScoreCard } from "./PlayerScoreCard";
import teamLogos from "./../../../../data/StaticData/teamLogos.json";
import { MovingTrain } from "../../../common/MovingTrain";

import "./../../../CommonCss.scss";

interface PlayerScoreCardsProps {
  playerInfos: PlayerShortInfo[];
  playing11Team: Player[];
  battingScoreCard: Batsman[];
  bowlingScoreCard: Bowler[];
  teamName?: string;
}

export const PlayerScoreCards: React.FC<PlayerScoreCardsProps> = ({
  playing11Team,
  playerInfos,
  battingScoreCard,
  bowlingScoreCard,
  teamName,
}) => {
  const playerScoreList = playing11Team
    .filter(
      (x) =>
        !x.name?.includes("Did not bat") &&
        !x.name?.includes("Yet to bat") &&
        !x.name?.includes("DRS")
    )
    .map((n, i) => (
      <PlayerScoreCard
        player={n}
        playerInfos={playerInfos}
        battingScoreCard={battingScoreCard}
        bowlingScoreCard={bowlingScoreCard}
        index={i}
        teamName={teamName}
        cardColor={
          teamLogos?.find((x) => x.teamName === teamName)?.primaryColor
        }
      />
    ));

  return (
    <div className="playing-11-list" style={{ background: "none" }}>
      {playerScoreList && (
        <MovingTrain
          bogies={playerScoreList}
          trackLength={3580 - 350}
          delay={3}
          duration={20}
        />
      )}
    </div>
  );
};
