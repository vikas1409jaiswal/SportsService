import React from "react";
import { PlayerImage } from "../../cricket-matches/screens/elements/PlayerImage";
import teamLogos from "./../../../data/StaticData/teamLogos.json";
import { AnimatedNumber } from "../../common/AnimatedNumber";

import "./TopPlayerCard.scss";

interface TopPlayerCardProps {
  href: string;
  name: string;
  matches: number;
  stat: number;
  teamShortName: string;
  className?: string;
  isStar?: boolean;
}

export const TopPlayerCard: React.FC<TopPlayerCardProps> = ({
  href,
  name,
  matches,
  stat,
  teamShortName,
  className,
  isStar,
}) => {
  console.log(teamShortName);
  return (
    <div className={`top-player-cards ${className || ""}`}>
      <PlayerImage
        href={href}
        alt={href}
        playerInfos={[]}
        height={570}
        width={400}
        teamName={
          teamLogos.find((tl) => tl.shortName === teamShortName)?.teamName
        }
      />
      <AnimatedNumber value={stat} duration={3000} className="stat-number" />
      <p className="name-header">
        <a
          href={`https://www.espncricinfo.com/${href}`}
          style={{ color: "white", textDecoration: "none" }}
        >
          {name}
        </a>
      </p>
      <img
        className="team-logo"
        src={teamLogos.find((tl) => tl.shortName === teamShortName)?.logoUrl}
        alt={href}
        height={250}
        width={250}
      />
    </div>
  );
};
