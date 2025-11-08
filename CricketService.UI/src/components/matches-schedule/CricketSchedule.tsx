import React, { useState } from "react";
import { useMatchesSchedule } from "./useMatchesSchedule";
import { MatchScheduleInfo } from "./MatchScheduleInfo";
import $ from "jquery";

interface CricketScheduleProps {}

export const CricketSchedule: React.FC<CricketScheduleProps> = ({}) => {
  const matches = useMatchesSchedule();
  const [selectedMatchIndex, setSelectedMatchIndex] = useState(0);

  //setInterval(() => setSelectedMatchIndex(selectedMatchIndex + 1), 10000);

  // // Press => for next player & <= for previous player.
  $(document).on({
    keydown: (event) => {
      if (
        event.originalEvent?.key === "ArrowRight" &&
        selectedMatchIndex < matches.length
      ) {
        setSelectedMatchIndex(selectedMatchIndex + 1);
        event.preventDefault();
      }
      if (event.originalEvent?.key === "ArrowLeft" && selectedMatchIndex > 0) {
        setSelectedMatchIndex(selectedMatchIndex - 1);
        event.preventDefault();
      }
    },
  });
  return (
    <>
      {matches && matches.length > 0 && (
        <MatchScheduleInfo
          href={""}
          matchNumber={selectedMatchIndex + 1}
          matchTitle={`${matches[selectedMatchIndex].team1} vs ${matches[selectedMatchIndex].team2}`}
          matchDate={matches[selectedMatchIndex].date}
          matchVenue={matches[selectedMatchIndex].venue}
          matchSeries={"SA20 League 2024"}
        />
      )}
    </>
  );
};
