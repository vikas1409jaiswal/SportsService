import React, { useEffect, useState, useContext } from "react";
import { CricketMatch } from "./../useCricketMatches";

import "./MatchDetails.scss";
import { TeamScoreCard } from "./TeamScoreCard/TeamScoreCard";

export interface MatchDetailsProps {
  matchData: CricketMatch;
}

export const MatchDetails: React.FunctionComponent<MatchDetailsProps> = ({
  matchData,
}) => {
  const downloadJsonData = () => {
    const filepath = `${matchData.matchNo
      .replace(" ", "_")
      .replace(" ", "_")}.json`;

    // Create a blob containing the data as JSON
    const blob = new Blob([JSON.stringify(matchData, null, 2)], {
      type: "application/json",
    });

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
    <div className="match-details-container">
      <button onClick={downloadJsonData}>Download</button>
      <table className="match-details-table">
        <thead>
          <tr>
            <th className="match-title">{matchData?.matchTitle}</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>Series:</td>
            <td>{matchData?.series}</td>
          </tr>
          <tr>
            <td>Match Number:</td>
            <td>{matchData?.matchNo}</td>
          </tr>
          <tr>
            <td>Match Days:</td>
            <td>{matchData?.matchDays}</td>
          </tr>
          <tr>
            <td>Venue:</td>
            <td>{matchData?.venue}</td>
          </tr>
          <tr>
            <td>Toss Winner:</td>
            <td>{matchData?.tossWinner}</td>
          </tr>
          <tr>
            <td>Toss Decision:</td>
            <td>{matchData?.tossDecision}</td>
          </tr>
          <tr>
            <td>Result:</td>
            <td>{matchData?.result}</td>
          </tr>
          <tr>
            <td>Player of the Match:</td>
            <td>{matchData?.playerOfTheMatch}</td>
          </tr>
          <tr>
            <td>Team 1:</td>
            <td>{matchData?.team1.teamName}</td>
          </tr>
          <tr>
            <td>Team 2:</td>
            <td>{matchData?.team2.teamName}</td>
          </tr>
        </tbody>
      </table>
      <TeamScoreCard
        teamScoreCard={matchData?.team1}
        opponent={matchData?.team2.teamName}
      />
      <TeamScoreCard
        teamScoreCard={matchData?.team2}
        opponent={matchData?.team1.teamName}
      />
    </div>
  );
};
