import React, { useState } from "react";
import { useFetchMatchesBySeason } from "../../hooks/espn-cricinfo-hooks/useFetchMatchesBySeason";
import { TestCricketMatch } from "./TestCricketMatch";
import { CricketMatch } from "./CricketMatch";
import { CricketFormat } from "../../models/enums/CricketFormat";
import $ from "jquery";

interface CricketMatchHomePageProps {
  format: CricketFormat;
}

export const CricketMatchHomePage: React.FC<CricketMatchHomePageProps> = ({
  format,
}) => {
  const { allLoaded, cricketMatchesAll } = useFetchMatchesBySeason(format, [
    2023,
  ]);

  const allMatches = cricketMatchesAll[0]?.matchDetails;

  const [selectedMatchIndex, setSelectedMatchIndex] = useState(125);
  const [selectedScreenIndex, setSelectedScreenIndex] = useState(7);

  // // Press => for next player & <= for previous player.
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
      if (
        event.originalEvent?.key === "ArrowUp" &&
        selectedMatchIndex < allMatches.length
      ) {
        setSelectedMatchIndex(selectedMatchIndex + 1);
        event.preventDefault();
      }
      if (event.originalEvent?.key === "ArrowDown" && selectedMatchIndex > 0) {
        setSelectedMatchIndex(selectedMatchIndex - 1);
        event.preventDefault();
      }
    },
  });
  return (
    <>
      {allLoaded && format === CricketFormat.Test && (
        <TestCricketMatch
          selectedMatchUrl={allMatches[selectedMatchIndex]?.href}
          selectedScreenIndex={selectedScreenIndex}
        />
      )}
      {allLoaded &&
        (format === CricketFormat.ODI || format === CricketFormat.T20I) && (
          <CricketMatch
            selectedMatchUrl={allMatches[selectedMatchIndex]?.href}
            selectedScreenIndex={selectedScreenIndex}
            format={format}
          />
        )}
    </>
  );
};
