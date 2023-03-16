import React, { useEffect, useState, useContext } from "react";
import { Team } from "../../useCricketMatches";

import "./TeamScoreCard.scss";

export interface TeamScoreCardProps {
  teamScoreCard: Team;
  opponent: string;
}

export const TeamScoreCard: React.FunctionComponent<TeamScoreCardProps> = ({
  teamScoreCard,
  opponent,
}) => {
  return (
    <div className="cricket-data">
      <h1>{teamScoreCard.teamName} Batting Scorecard</h1>
      <table>
        <thead>
          <tr>
            <th>Player Name</th>
            <th>Runs Scored</th>
            <th>Balls Faced</th>
            <th>Minutes</th>
            <th>Fours</th>
            <th>Sixes</th>
          </tr>
        </thead>
        <tbody>
          {teamScoreCard.battingScorecard.map((player, index) => (
            <tr key={index}>
              <td>{player.playerName.name}</td>
              <td>{player.runsScored}</td>
              <td>{player.ballsFaced}</td>
              <td>{player.minutes}</td>
              <td>{player.fours}</td>
              <td>{player.sixes}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <h1>{opponent} Bowling Scorecard</h1>
      <table>
        <thead>
          <tr>
            <th>Player Name</th>
            <th>Overs Bowled</th>
            <th>Maidens</th>
            <th>Runs Conceded</th>
            <th>Wickets</th>
            <th>Dots</th>
            <th>Fours</th>
            <th>Sixes</th>
            <th>Wide Balls</th>
            <th>No Balls</th>
          </tr>
        </thead>
        <tbody>
          {teamScoreCard.bowlingScorecard.map((player, index) => (
            <tr key={index}>
              <td>{player.playerName.name}</td>
              <td>{player.oversBowled}</td>
              <td>{player.maidens}</td>
              <td>{player.runsConceded}</td>
              <td>{player.wickets}</td>
              <td>{player.dots}</td>
              <td>{player.fours}</td>
              <td>{player.sixes}</td>
              <td>{player.wideBall}</td>
              <td>{player.noBall}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <h2>Extras: {teamScoreCard.extras}</h2>

      <h2>Fall of Wickets</h2>
      <ul>
        {teamScoreCard.fallOfWickets.map((wicket, index) => (
          <li key={index}>{wicket}</li>
        ))}
      </ul>
    </div>
  );
};
