import React, { useEffect, useState, useContext } from "react";
import { CricketContext } from "../../../CricketHomePage";
import {
  CricketMatch,
  CricketMatchesBySeason,
  useCricketMatches,
  useCricketMatchesBySeasonDetails,
} from "../../../CricketHooks/useCricketMatches";
import { MatchDetails } from "../MatchDetails/MatchDetails";

import "./MatchDetailsBySeason.scss";

export interface MatchDetailsBySeasonProps {
  matchDataBySeason: CricketMatchesBySeason;
}

export const MatchDetailsBySeason: React.FunctionComponent<
  MatchDetailsBySeasonProps
> = ({ matchDataBySeason }) => {
  const { season, matchDetails } = matchDataBySeason;
  const [currentSelectedMatch, setCurrentSelectedMatch] = useState(
    matchDetails[0]
  );

  const cricketContext = useContext(CricketContext);

  const cricketMatch = useCricketMatches(currentSelectedMatch?.href);

  console.log(useCricketMatchesBySeasonDetails(matchDataBySeason.matchDetails));

  useEffect(() => {
    cricketContext.setCurrentMatchDetails({
      ...cricketMatch,
      matchDate: currentSelectedMatch?.matchDate,
    });
  }, [cricketMatch]);

  const downloadJsonData = () => {
    const filepath = `${cricketContext.currentMatchDetails.matchNo.replace(
      " no. ",
      "_no._"
    )}.json`;

    // Create a blob containing the data as JSON
    const blob = new Blob(
      [JSON.stringify(cricketContext.currentMatchDetails, null, 2)],
      { type: "application/json" }
    );

    // Create a URL for the blob
    const url = URL.createObjectURL(blob);

    // Create an <a> element and set its href and download attributes
    const link = document.createElement("a");
    link.href = url;
    link.download = filepath;

    // Programmatically click the link to trigger the download
    link.click();

    // Release the URL object
    URL.revokeObjectURL(url);
  };

  return (
    <div>
      <h2>Cricket Matches of {season}</h2>
      <table>
        <thead>
          <tr>
            <th>Match No.</th>
            <th>Teams</th>
            <th>Winner</th>
            <th>Margin</th>
            <th>Ground</th>
            <th>Match Date</th>
            <th>Match URL</th>
            <th>Download File</th>
          </tr>
        </thead>
        <tbody>
          {matchDetails.map((match, index) => (
            <tr key={index}>
              <td>{match.matchNo}</td>
              <td>
                {match.team1} vs {match.team2}
              </td>
              <td>{match.winner}</td>
              <td>{match.margin}</td>
              <td>{match.ground}</td>
              <td>{match.matchDate}</td>
              <td>
                <button onClick={() => setCurrentSelectedMatch(match)}>
                  Show Match Details
                </button>
              </td>
              <td>
                <button onClick={downloadJsonData}>
                  Download{" "}
                  {cricketContext.currentMatchDetails &&
                    `${cricketContext.currentMatchDetails.matchNo.replace(
                      " no. ",
                      "_no._"
                    )}.json`}
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {cricketMatch && (
        <MatchDetails matchData={cricketContext.currentMatchDetails} />
      )}
    </div>
  );
};
