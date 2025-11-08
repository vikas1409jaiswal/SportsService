import React, { useEffect } from "react";
import { RotatingCylinder } from "../common/RotatingCylinder";
import logos from "../../data/StaticData/teamLogos.json";
import { AnimatedValueContent } from "../cricket-players/cricket-body/PlayerInfo/AnimatedValueContent";
import { motion, useAnimation } from "framer-motion";

import "./MatchScheduleInfo.scss";
import "./../../components/CommonCss.scss";
import { speakText } from "../common/SpeakText";

interface MatchScheduleInfoProps {
  href: string;
  matchNumber: number;
  matchTitle: string;
  matchDate: string;
  matchVenue: string;
  matchSeries: string;
}

export const MatchScheduleInfo: React.FC<MatchScheduleInfoProps> = ({
  href,
  matchNumber,
  matchTitle,
  matchDate,
  matchVenue,
  matchSeries,
}) => {
  const [team1, team2] = matchTitle.split(" vs ");

  const logoControl = useAnimation();
  const labelControl = useAnimation();

  useEffect(() => {
    if (matchNumber === 1) {
      speakText("S A T twenty league 2024 schedule");
    }
    speakText(
      `Match Number ${matchNumber} ${matchTitle
        ?.replace("vs", "versus")
        ?.replace("St ", "Saint")}`?.replace("MI", "M I")
    );

    return () => window.speechSynthesis.cancel();
  }, [matchNumber]);

  useEffect(() => {
    logoControl.start({
      scale: [0, 1],
      transition: {
        duration: 3,
      },
    });

    labelControl.start({
      opacity: [0, 1],
      transition: {
        duration: 3,
      },
    });
  }, [matchNumber]);

  const getSeriesSubTitle = () => {
    if (matchNumber === 31) {
      return "- Qualifier 1";
    }
    if (matchNumber === 32) {
      return "- Eliminator";
    }
    if (matchNumber === 33) {
      return "- Qualifier 2";
    }
    // if (matchNumber === 46 || matchNumber === 47) {
    //   return "- Semi Final";
    // }
    if (matchNumber === 34) {
      return "- Final";
    }
    return "";
  };

  return (
    <div
      className="match-basic-info-container"
      style={{
        height: 860,
        background: 'url("https://wallpapercave.com/wp/wp3049846.jpg")',
        backgroundSize: "100% 100%",
      }}
    >
      <div className="match-number">
        {`Match Number `}
        <AnimatedValueContent
          value={matchNumber}
          duration={2000}
          player={null}
        />
      </div>
      <div className="match-series">{`${matchSeries} ${getSeriesSubTitle()}`}</div>
      <div className="match-title" style={{ background: "none" }}>
        <motion.div style={{ background: "none" }} animate={logoControl}>
          <RotatingCylinder
            images={Array.from(
              { length: 5 },
              () =>
                logos.find((t) => t.teamName === team1)?.logoUrl ||
                "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a5/SA20-logo.svg/1024px-SA20-logo.svg.png"
            )}
            width={250}
            height={250}
            rotationSpeed={8}
            translateZ={170}
          />
          <h1 className="text-3d">{team1}</h1>
        </motion.div>
        <div className="vs-sign text-3d">
          <a
            href={`https://stats.espncricinfo.com${href}`}
            rel="noreferrer"
            target="_blank"
            style={{ textDecoration: "none" }}
          >
            vs
          </a>
        </div>
        <motion.div style={{ background: "none" }} animate={logoControl}>
          <RotatingCylinder
            images={Array.from(
              { length: 5 },
              () =>
                logos.find((t) => t.teamName === team2)?.logoUrl ||
                "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a5/SA20-logo.svg/1024px-SA20-logo.svg.png"
            )}
            width={250}
            height={250}
            rotationSpeed={8}
            translateZ={170}
          />
          <h1 className="text-3d">{team2}</h1>
        </motion.div>
      </div>
      <motion.div className="match-date" animate={labelControl}>
        {matchDate}
      </motion.div>
      <motion.div className="match-venue" animate={labelControl}>
        {matchVenue}
      </motion.div>
    </div>
  );
};
