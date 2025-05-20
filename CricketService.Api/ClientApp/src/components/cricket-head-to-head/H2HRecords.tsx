import React, { useEffect, useState } from "react";
import { useFetchH2HSummary } from "../../hooks/espn-cricinfo-hooks/useFetchH2HSummary";
import { CricketFormat } from "../../models/enums/CricketFormat";
import { useAnimation } from "framer-motion";
import { speakText } from "../common/SpeakText";
import { H2HResultSummary } from "./components/H2HResultSummary";
import $ from "jquery";
import { H2HMostRuns } from "./components/H2HMostRuns";
import { H2HMostWickets } from "./components/H2HMostWickets";
import { H2HHIScores } from "./components/H2HHIScores";

import "./H2HRecords.scss";
import "./../CommonCss.scss";

interface H2HRecordsProps {
  format: CricketFormat;
  team1Name: string;
  team2Name: string;
}

export const H2HRecords: React.FC<H2HRecordsProps> = ({
  format,
  team1Name,
  team2Name,
}) => {
  const teamArr = [team1Name, team2Name].sort();
  const matches = useFetchH2HSummary(format, teamArr[0], teamArr[1]);

  const [selectedScreenIndex, setSelectedScreenIndex] = useState(1);

  $(document).on({
    keydown: (event) => {
      if (
        event.originalEvent?.key === "ArrowRight" &&
        selectedScreenIndex < 9
      ) {
        setSelectedScreenIndex(selectedScreenIndex + 1);
        event.preventDefault();
      }
      if (event.originalEvent?.key === "ArrowLeft" && selectedScreenIndex > 0) {
        setSelectedScreenIndex(selectedScreenIndex - 1);
        event.preventDefault();
      }
    },
  });

  const playerImageControl = useAnimation();

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

  const screenIndexs = {
    resultSummary: 0,
    mostRuns: 1,
    mostWickets: 2,
    hiScores: 3,
  };

  return (
    <div
      className="h2h-match-record-container"
      style={{
        height: "100vw",
        background: 'url("https://wallpapercave.com/wp/wp3049846.jpg")',
        backgroundSize: "100% 100%",
      }}
    >
      {selectedScreenIndex === screenIndexs.resultSummary && (
        <H2HResultSummary
          format={format}
          team1Name={team1Name}
          team2Name={team2Name}
          resultSummary1={matches[0]}
          resultSummary2={matches[1]}
        />
      )}
      {selectedScreenIndex === screenIndexs.mostRuns && (
        <H2HMostRuns
          team1Name={team1Name}
          team2Name={team2Name}
          resultSummary1={matches[0]}
        />
      )}
      {selectedScreenIndex === screenIndexs.mostWickets && (
        <H2HMostWickets
          team1Name={team1Name}
          team2Name={team2Name}
          resultSummary1={matches[0]}
        />
      )}
      {selectedScreenIndex === screenIndexs.hiScores && (
        <H2HHIScores
          team1Name={team1Name}
          team2Name={team2Name}
          resultSummary1={matches[0]}
        />
      )}
    </div>
  );
};
