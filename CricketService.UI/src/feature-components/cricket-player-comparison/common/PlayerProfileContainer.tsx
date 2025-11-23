import React, { useState } from "react";
import { PlayerImageContainer } from "../../common/PlayerImageContainer";
import { PlayerExtraInfo } from "../../common/PlayerExtraInfo";
import { HamburgerMenu, MenuItem } from "../../../components/common";
import "./PlayerProfileContainer.scss";
import { SidePane } from "./SidePane";
import { CricketPlayerResponse } from "../hook/useFetchPlayerAllMatches";
import { BattingInningDetailsTable } from "./side-overlay/BattingInningDetailsTable";

interface PlayerProfileContainerProps {
  playerData: CricketPlayerResponse;
  customHeight?: number;
  scaleTeamCylinder?: number;
  className?: string;
  isMenuOpened?: boolean;
}

export const PlayerProfileContainer: React.FC<PlayerProfileContainerProps> = ({
  playerData,
  customHeight = 845,
  scaleTeamCylinder = 0.85,
  className = "",
  isMenuOpened = false,
}) => {
  const {playerHref, fullName, teamNames} = playerData;
  const teamName = teamNames[0];
  const [isSidePaneOpen, setIsSidePaneOpen] = useState(false);

  const handleBattingInningsClick = () => {
    setIsSidePaneOpen(true);
  };

  const handleCloseSidePane = () => {
    setIsSidePaneOpen(false);
  };

  const menuItems: MenuItem[] = [
    {
      label: "All Batting Innings",
      onClick: handleBattingInningsClick,
      key: "batting-innings"
    }
  ];

  return (
    <>
      <div className={`player-container player-profile-container ${className}`.trim()}>
        <HamburgerMenu 
          menuItems={menuItems}
          isVisible={isMenuOpened}
          className="three-dot-menu"
          ariaLabel="Open player menu"
        />
        <PlayerImageContainer
          playerHref={playerHref}
          selectedRowIndex={0}
          teamName={teamName}
          hideRotatingCircle
          skipAnimation
          extraInfo={[
            <PlayerExtraInfo key="player-extra-info" playerHref={playerHref} />,
          ]}
          customHeight={customHeight}
          scaleTeamCylinder={scaleTeamCylinder}
        />
      </div>

      <SidePane open={isSidePaneOpen} title={fullName} subtitle="All Batting Innings" onClose={handleCloseSidePane}>
        <BattingInningDetailsTable battingInningsStats={playerData.overallStats.playerODIStats.battingInningsStats || []} />
      </SidePane>
    </>
  );
};
