import React from "react";
import logos from "./../../../data/StaticData/teamLogos.json";

import "./TeamImageContainer.scss";

interface TeamImageContainerProps {
  teamName?: string;
}

export const TeamImageContainer: React.FC<TeamImageContainerProps> = ({
  teamName,
}) => {
  return (
    <div className="team-image-container">
      <p>{teamName}</p>
      <img
        alt={teamName}
        src={logos?.find((x) => x.teamName === teamName)?.logoUrl}
        height={400}
        width={400}
      />
    </div>
  );
};
