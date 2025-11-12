import React, { useEffect, useMemo } from "react";
import playersRolesInfo from "./players-roles-info.json";
import { motion, useAnimation } from "framer-motion";
import { ESPNTableRow } from "../../hook/useCustomESPNTable";
import $ from "jquery";
import { StatRow } from "./StatRow";
import { PlayerImageContainer } from "../../common/PlayerImageContainer";
import { speeches } from "../../speech-management/SpeechManagement";
import engToHiJson from "../../../../data/StaticData/englishToHindi.json";
import {
  ESPNPlayerInfo,
  useESPNPlayerInfo,
} from "../../../../hooks/espn-cricinfo-hooks/usePlayerInfo";
import { config } from "../../../../configs";
import { getNameFromHref } from "../../../../utils/ReusableFuctions";
import { SingleStats } from "../../common/SingleStats";

import "./IndividualPage.scss";

type IndividualPageProps = {
  row: ESPNTableRow;
  selectedRowIndex: number;
  setSelectedRowIndex: (i: number) => void;
};

export const IndividualPage: React.FunctionComponent<IndividualPageProps> = ({
  row,
  selectedRowIndex,
  setSelectedRowIndex,
}) => {
  const cricketPositions = (engToHiJson as any)["cricket-positions"];
  const playerHref =
    row?.data.find((x) => x.key === "Player Href")?.value || "";
  const teamName =
    row?.data.find((x) => x.key === "Team Name")?.value || "";
  const playerName =
    playerHref
      ?.split("/")[2]
      ?.split("-")
      ?.filter((x) => Number.isNaN(parseInt(x)))
      ?.join(" ") || "";

  const isFirstPlayer = selectedRowIndex === 0;
  const isLastPlayer = selectedRowIndex === 9;

  const playerDetailsControl = useAnimation();

  const { teamNames, playingRole: apiPlayingRole, battingStyle: apiBattingStyle, bowlingStyle: apiBowlingStyle } =
    useESPNPlayerInfo(playerHref) as ESPNPlayerInfo;

  // Fallback to JSON if API values are undefined
  const { playingRole, battingStyle, bowlingStyle } = useMemo(() => {
    if (apiPlayingRole && apiBattingStyle && apiBowlingStyle) {
      return {
        playingRole: apiPlayingRole,
        battingStyle: apiBattingStyle,
        bowlingStyle: apiBowlingStyle,
      };
    }
    const match = playersRolesInfo.find(
      (p) => p.playerHref && playerHref && p.playerHref === playerHref
    );
    return {
      playingRole: apiPlayingRole || match?.playingRole || "",
      battingStyle: apiBattingStyle || match?.battingStyle || "",
      bowlingStyle: apiBowlingStyle || match?.bowlingStyle || "",
    };
  }, [apiPlayingRole, apiBattingStyle, apiBowlingStyle, playerHref]);

  useEffect(() => {
    config.isAnimation &&
      playerDetailsControl.start({
        x: ["1000px", "0px"],
        transition: {
          type: "spring",
          stiffness: 10,
          damping: 100,
        },
      });
  }, [selectedRowIndex]);

  // // Press => for next player & <= for previous player.
  // // Press Shift + F for flipping body columns.
  $(document).on({
    keydown: (event) => {
      if (event.originalEvent?.key === "ArrowRight" && !isLastPlayer) {
        setSelectedRowIndex(selectedRowIndex + 1);
      }
      if (event.originalEvent?.key === "ArrowLeft" && !isFirstPlayer) {
        setSelectedRowIndex(selectedRowIndex - 1);
      }
    },
  });

  const playerDetailsContainer = (
    <div className="cricket-player-details">
      <motion.p
        className="player-header"
        animate={playerDetailsControl}
        whileTap={{
          scale: 1.05,
          backgroundColor: "pink",
        }}
      >
        <a href={`https://www.espncricinfo.com/${playerHref}`}>
          {config.language === "hindi"
            ? getNameFromHref(playerHref, "hindi")
            : playerName?.toUpperCase()}
        </a>
      </motion.p>
      <motion.div
        className="cricket-player-details-section"
        animate={playerDetailsControl}
      >
        <div style={{ display: "grid", gridTemplateColumns: "repeat(1, 1fr)" }}>
          <SingleStats
            singleStat={{
              key: row?.data[3].key,
              value: row?.data[3].value,
            }}
          />
          {/* <SingleStats
            singleStat={{
              key: row?.data[3].key,
              value: row?.data[3].value,
            }}
          /> */}
        </div>
        {row?.data?.slice(4).map((x) => (
          <StatRow
            singleStat={{
              key: x.key,
              value: x.value,
            }}
            isAnimation={
              ![
                "Team",
                "Against",
                "Venue",
                "Date",
                "Span",
                "H.Score",
                "BBI",
                "Dis/Inn",
                "Match Date",
                "1st Ing Score",
                "2nd Ing Score",
              ].includes(x.key)
            }
          />
        ))}
      </motion.div>
    </div>
  );

  useEffect(() => {
    if (selectedRowIndex === 9) {
      speeches["top-10-players-intro"]();
    }

    if (row?.data) {
      speeches["most-single-test-runs-speech"](
        selectedRowIndex,
        playerName,
        row,
        teamNames[0]
      );
    }

    if (selectedRowIndex === 1) {
      speeches["video-end-message"]();
    }

    return () => window.speechSynthesis.cancel();
  }, [selectedRowIndex]);

  return (
    <div
      className="cricket-player-container"
      style={{
        height: 860,
        background: 'url("http://localhost:3013/textures/1012.png")',
        backgroundSize: "100% 100%",
      }}
    >
      <PlayerImageContainer
        playerHref={playerHref}
        selectedRowIndex={selectedRowIndex}
        teamName={teamName}
        extraInfo={(() => {
          if (config.language === "hindi") {
            return [
              <p>{cricketPositions[playingRole]}</p>,
              <p>{cricketPositions[battingStyle]}</p>,
              <p>{bowlingStyle
                .split(", ")
                .map((x) => cricketPositions[x])
                .join(", ")}</p>,
            ];
          } else {
            return [
              <p>{playingRole}</p>,
              <p>{battingStyle}</p>,
              <p>{bowlingStyle}</p>,
            ];
          }
        })()}
      />
      {playerDetailsContainer}
    </div>
  );
};
