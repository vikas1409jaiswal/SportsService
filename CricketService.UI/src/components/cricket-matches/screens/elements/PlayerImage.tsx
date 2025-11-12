import React from "react";
import { PlayerShortInfo } from "../../../hooks/useAllPlayersUuids";
import { motion, useAnimation } from "framer-motion";

interface PlayerImageProps {
  alt: string;
  href: string;
  playerInfos: PlayerShortInfo[];
  className?: string;
  height?: number;
  width?: number;
  teamName?: string;
  imgStyle?: React.CSSProperties;
}

export const PlayerImage: React.FC<PlayerImageProps> = ({
  alt,
  href,
  className,
  width,
  height,
  teamName,
  imgStyle,
}) => {
  const control = useAnimation();

  return (
    <motion.img
      animate={control}
      className={`${href} ${className || ""}`}
      alt={alt}
      src={
        teamName !== "Thailandx"
          ? `http://localhost:3013/images/${teamName
              ?.toLowerCase()
              ?.replaceAll(" ", "-")}/${href?.split("/")[2]}.png`
          : "https://www.atlantaopentennis.com/-/media/alias/player-gladiator/M0NP"
      }
      height={height || 500}
      width={width || 400}
      style={imgStyle}
    />
  );
};
