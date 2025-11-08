import React from "react";
import { CountryContent } from "../../cricket-players/cricket-body/PlayerInfo/CountryContent";

import "./H2HPageHeader.scss";

interface H2HPageHeaderProps {
  team1Name: string;
  team2Name: string;
  span: string;
}

export const H2HPageHeader: React.FC<H2HPageHeaderProps> = ({
  team1Name,
  team2Name,
  span,
}) => {
  return (
    <div className="h2h-result-summary-header" style={{ background: "none" }}>
      <CountryContent
        countryName={team1Name}
        hideName
        className="team-logo-container"
      />
      <div className="h2h-banner">
        <p>HEAD TO HEAD</p>
        <p className="h2h-teams-name">
          {`${team1Name} v ${team2Name} (${span})`}{" "}
        </p>
      </div>
      <CountryContent
        countryName={team2Name}
        hideName
        className="team-logo-container"
      />
    </div>
  );
};
