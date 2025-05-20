import { motion, useAnimation } from "framer-motion";
import React, { ReactNode, useEffect } from "react";
import { PlayerImage } from "../../cricket-matches/screens/elements/PlayerImage";
import RotatingCircle from "./RotatingCircle";
import { CountryContent } from "../../cricket-players/cricket-body/PlayerInfo/CountryContent";

import "./PlayerImageContainer.scss";

interface PlayerImageContainerProps {
  playerHref: string;
  selectedRowIndex: number;
  hideRotatingCircle?: boolean;
  hideCountryRotatingCylinder?: boolean;
  skipAnimation?: boolean;
  extraInfo?: ReactNode[];
  customHeight?: number;
  teamName: string;
  scaleTeamCylinder?: number;
}

export const PlayerImageContainer: React.FC<PlayerImageContainerProps> = ({
  playerHref,
  customHeight,
  selectedRowIndex,
  hideRotatingCircle,
  hideCountryRotatingCylinder,
  skipAnimation,
  extraInfo,
  teamName,
  scaleTeamCylinder,
}) => {
  const playerImageControl = useAnimation();

  useEffect(() => {
    !skipAnimation &&
      playerImageControl.start({
        translateX: [-1000, 0],
        transition: {
          duration: 1,
        },
      });
  }, [selectedRowIndex, skipAnimation]);

  return (
    <div className="player-image-container">
      {!hideCountryRotatingCylinder && (
        <CountryContent
          countryName={teamName}
          hideName
          className="team-logo-cylinder"
          rotationSpeed={5}
          height={scaleTeamCylinder ? scaleTeamCylinder * 120 : 120}
          width={scaleTeamCylinder ? scaleTeamCylinder * 120 : 120}
          translateZ={scaleTeamCylinder ? scaleTeamCylinder * 80 : 80}
        />
      )}
      <motion.div
        className="cricket-player-image"
        animate={playerImageControl}
        whileTap={{
          backgroundColor: "orange",
        }}
        style={{ height: customHeight || 800 }}
      >
        <PlayerImage
          alt={playerHref}
          href={playerHref}
          playerInfos={[]}
          teamName={teamName}
        />
        {extraInfo?.map((x: any) => (x?.props.children !== "" ? x : null))}
      </motion.div>
      {!hideRotatingCircle && <RotatingCircle number={selectedRowIndex + 1} />}
    </div>
  );
};
