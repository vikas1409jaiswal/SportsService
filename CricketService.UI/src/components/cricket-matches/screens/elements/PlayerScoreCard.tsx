import React, { useEffect } from "react";
import { PlayerImage } from "./PlayerImage";
import { BattingShortInfo } from "./BattingShortInfo";
import { BowlingShortInfo } from "./BowlingShortInfo";
import RotatingCircle from "../../../cricket-records/common/RotatingCircle";
import {
  Batsman,
  Bowler,
  Player,
} from "../../../../models/espn-cricinfo-models/CricketMatchModels";
import { PlayerShortInfo } from "../../../hooks/useAllPlayersUuids";
import { speakText } from "../../../common/SpeakText";
import { useInView } from "framer-motion";
import { CountryContent } from "../../../cricket-players/cricket-body/PlayerInfo/CountryContent";

interface PlayerScoreCardProps {
  player: Player;
  playerInfos: PlayerShortInfo[];
  battingScoreCard: Batsman[];
  bowlingScoreCard: Bowler[];
  index: number;
  teamName?: string;
  cardColor?: string;
}

export const PlayerScoreCard: React.FC<PlayerScoreCardProps> = ({
  player,
  playerInfos,
  battingScoreCard,
  bowlingScoreCard,
  index,
  teamName,
  cardColor,
}) => {
  useEffect(() => {
    const runs = battingScoreCard.find(
      (x) => x.playerName.href === player.href
    )?.runsScored;
    speakText(
      `${player.name?.replace("(c)", "captain")} ${
        runs || (runs === 0 ? 0 : "did not bat")
      }`
    );

    return () => window.speechSynthesis.cancel();
  }, []);
  return (
    <div
      className="player-card"
      style={{ backgroundColor: cardColor || "white" }}
    >
      <PlayerImage
        alt={player.name}
        href={player.href}
        playerInfos={playerInfos}
        //className={player.name?.includes("x") ? "flipped-image" : ""}
        teamName={teamName}
      />
      <h5>
        {player.name.length < 20
          ? player.name.toUpperCase()
          : player.name
              .split(" ")
              .map((x, i) => (i === 0 ? `${x?.trim()[0]}.` : x?.trim()))
              .join(" ")
              .toUpperCase()}
      </h5>
      <BattingShortInfo
        battingScoreCard={battingScoreCard}
        href={player.href}
      />
      <BowlingShortInfo
        bowlingScoreCard={bowlingScoreCard}
        href={player.href}
      />
      <RotatingCircle number={index + 1} />
      {/* <CountryContent
        countryName={teamName || ""}
        hideName
        className="team-logo-cylinder"
        height={60}
        width={60}
        translateZ={40}
      /> */}
    </div>
  );
};
