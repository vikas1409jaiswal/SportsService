import React from "react";
import { motion } from "framer-motion";
import { ESPNTableRow } from "../../hook/useCustomESPNTable";
import { config } from "../../../../configs";
import { getNameFromHref } from "../../../../utils/ReusableFuctions";
import { SingleStats } from "../../../common/SingleStats";
import { StatRow } from "./StatRow";

interface PlayerDetailsContainerProps {
  playerName: string;
  playerHref: string;
  row: ESPNTableRow;
  playerDetailsControl: any;
  teamName: string;
}

export const PlayerDetailsContainer: React.FC<PlayerDetailsContainerProps> = ({
  playerName,
  playerHref,
  row,
  playerDetailsControl,
  teamName,
}) => (
  <div className="cricket-player-details">
    <motion.p
      className="player-header"
      animate={playerDetailsControl}
      whileTap={{
        scale: 1.05,
        backgroundColor: "pink",
      }}
      style={{ fontSize: playerName.length > 20 ? "50px" : "60px" }}
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
      </div>
      {row?.data?.slice(4).map((x) => (
        <StatRow
          key={x.key}
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
