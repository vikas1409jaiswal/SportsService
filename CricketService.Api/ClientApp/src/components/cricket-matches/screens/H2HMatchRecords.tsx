import React, { useEffect } from "react";
import { useFetchH2HSummary } from "../../../hooks/espn-cricinfo-hooks/useFetchH2HSummary";

import "./H2HMatchRecords.scss";
import { Player } from "../../../models/espn-cricinfo-models/CricketMatchModels";
import { PlayerImage } from "./elements/PlayerImage";
import "./../../CommonCss.scss";
import { CricketFormat } from "../../../models/enums/CricketFormat";
import { motion, useAnimation } from "framer-motion";
import { AnimatedValueContent } from "../../cricket-players/cricket-body/PlayerInfo/AnimatedValueContent";
import { CountryFlag } from "./elements/CountryFlag";
import { speakText } from "../../common/SpeakText";
import { H2HPageHeader } from "../../cricket-head-to-head/components/H2HPageHeader";

interface H2HMatchRecordsProps {
  team1Captain: Player;
  team2Captain: Player;
  format: CricketFormat;
  team1Name: string;
  team2Name: string;
}

export const H2HMatchRecords: React.FC<H2HMatchRecordsProps> = ({
  team1Captain,
  team2Captain,
  format,
  team1Name,
  team2Name,
}) => {
  const teamArr = [team1Name, team2Name].sort();
  const matches = useFetchH2HSummary(format, teamArr[0], teamArr[1]);

  const playerImageControl = useAnimation();

  console.log(team2Captain);

  useEffect(() => {
    playerImageControl.start({
      scale: [0.1, 1],
      transition: {
        type: "spring",
        stiffness: 100,
        damping: 10,
      },
    });
  }, []);

  useEffect(() => {
    speakText(`${team1Name} versus ${team2Name} head to head`);
    return () => window.speechSynthesis.cancel();
  }, []);

  return (
    <div
      className="h2h-record-container"
      style={{
        height: 860,
        background: 'url("https://wallpapercave.com/wp/wp3049846.jpg")',
        backgroundSize: "100% 100%",
      }}
    >
      <H2HPageHeader
        team1Name={team1Name}
        team2Name={team2Name}
        span={matches[0]?.span}
      />
      <div className="h2h-team-records" style={{ background: "none" }}>
        <motion.div
          className="h2h-columns team-1-info"
          animate={playerImageControl}
        >
          <PlayerImage
            className="flipped-image"
            alt={team1Captain?.name}
            href={team1Captain?.href}
            playerInfos={[]}
            height={500}
            teamName={team1Name}
          />
          <h1>{team1Captain?.name}</h1>
        </motion.div>
        <div
          className="h2h-columns comparison-info"
          style={{ background: "none" }}
        >
          <div className="h2h-match">
            <span>Matches</span>
            <AnimatedValueContent
              value={matches[0]?.matches}
              duration={5000}
              player={null}
            />
          </div>
          <div className="h2h-match">
            <span>{`${matches[0]?.team
              ?.replace("United States of America", "USA")
              ?.replace("United Arab Emirates", "UAE")} Won`}</span>
            <AnimatedValueContent
              value={matches[0]?.won}
              duration={5000}
              player={null}
            />
          </div>
          <div className="h2h-match">
            <span>{`${matches[1]?.team
              ?.replace("United States of America", "USA")
              ?.replace("United Arab Emirates", "UAE")} Won`}</span>
            <AnimatedValueContent
              value={matches[0]?.lost}
              duration={5000}
              player={null}
            />
          </div>
          {format === CricketFormat.Test && (
            <div className="h2h-match">
              <span>Draw</span>
              <span>{matches[0]?.draw}</span>
            </div>
          )}
          <div className="h2h-match">
            <span>Tied</span>
            <AnimatedValueContent
              value={matches[0]?.tied}
              duration={5000}
              player={null}
            />
          </div>
          <div className="most-records" style={{ background: "none" }}>
            <div className="key">
              <p className="title">Most Runs</p>
              <p className="player">{matches[0]?.mostRuns[0]?.name}</p>
            </div>
            <div className="value">
              <AnimatedValueContent
                value={matches[0]?.mostRuns[0]?.runs}
                duration={5000}
                player={null}
              />
            </div>
            <div className="key">
              <p className="title">Most Wickets</p>
              <p className="player">{matches[0]?.mostWickets[0]?.name}</p>
            </div>
            <div className="value">
              <AnimatedValueContent
                value={matches[0]?.mostWickets[0]?.wickets}
                duration={5000}
                player={null}
              />
            </div>
          </div>
        </div>
        <motion.div
          className="h2h-columns team-2-info"
          animate={playerImageControl}
        >
          <PlayerImage
            alt={team2Captain?.name}
            href={team2Captain?.href || "/cricketers/ruturaj-gaikwad-1060380"}
            playerInfos={[]}
            height={500}
            teamName={team2Name}
          />
          <h1>{team2Captain?.name || "Ruturaj Gaikwad (c)"}</h1>
        </motion.div>
      </div>
    </div>
  );
};
