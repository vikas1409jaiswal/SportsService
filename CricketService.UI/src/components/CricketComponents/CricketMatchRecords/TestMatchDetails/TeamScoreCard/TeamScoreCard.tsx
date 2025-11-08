import React, { useEffect, useState, useContext } from "react";
import { ExpandableSection } from "../../../../common/ExpandableSection";
import {
  CricketTeam,
  InningScoreBoard,
  TeamScoreBoard,
  TestTeamScoreBoard,
} from "./../../Models/Interface";

import "./TeamScoreCard.scss";

export interface TeamScoreCardProps {
  teamData: CricketTeam;
  teamScoreCard: InningScoreBoard;
  opponent: string;
  teamName: string;
}

export const TeamScoreCard: React.FunctionComponent<TeamScoreCardProps> = ({
  teamData,
  teamScoreCard,
  opponent,
  teamName,
}) => {
  const totalScoreStr = `${teamScoreCard?.totalInningDetails.runs}/${teamScoreCard?.fallOfWickets.length}  (${teamScoreCard?.totalInningDetails.overs.overs} overs)`;

  return (
    <div className="team-scorecard">
      <ExpandableSection
        title={`${teamName} Batting Scorecard`}
        isExpanded={true}
      >
        <table>
          <thead>
            <tr>
              <th>Player Name</th>
              <th>Runs Scored</th>
              <th>Balls Faced</th>
              <th>Minutes</th>
              <th>Fours</th>
              <th>Sixes</th>
              <th>Strike Rate</th>
            </tr>
          </thead>
          <tbody>
            {teamScoreCard?.battingScoreCard.map((bsc, index) => (
              <tr
                key={index}
                style={{
                  backgroundColor:
                    bsc.outStatus.indexOf("not out") !== -1
                      ? "yellow"
                      : "white",
                }}
              >
                <td
                  style={{
                    backgroundColor:
                      bsc.outStatus.indexOf("not out") !== -1
                        ? "orange"
                        : "pink",
                  }}
                >
                  <b>{bsc.playerName.name}</b> <h6>{bsc.outStatus}</h6>
                </td>
                <td>
                  <b>{bsc.runsScored}</b>
                </td>
                <td>{bsc.ballsFaced}</td>
                <td>{bsc.minutes}</td>
                <td>{bsc.fours}</td>
                <td>{bsc.sixes}</td>
                <td>{bsc.strikeRate?.toPrecision(5)}</td>
              </tr>
            ))}
            {teamScoreCard?.didNotBat.map((d) => (
              <tr>
                <td>
                  <b>{d.name}</b>
                  <h6>did not bat</h6>
                </td>
                <td>-</td>
                <td>-</td>
                <td>-</td>
                <td>-</td>
                <td>-</td>
                <td>-</td>
              </tr>
            ))}
            <tr className="extras-row">
              <td>Extras</td>
              <td>{teamScoreCard?.totalInningDetails.extras.totalExtras}</td>
              <td colSpan={3}>{teamScoreCard?.extras}</td>
            </tr>
            <tr className="total-score">
              <td>Total</td>
              <td>
                <b>{totalScoreStr}</b>
              </td>
            </tr>
          </tbody>
        </table>
        <div className="extra-batting-info">
          {teamScoreCard?.fallOfWickets.length > 0 && (
            <>
              <h6>
                <b>Fall Of Wickets</b>
              </h6>
              <div className="fall-of-wicket-details">
                {teamScoreCard?.fallOfWickets.map((fow) => (
                  <span>{fow}</span>
                ))}{" "}
              </div>
            </>
          )}
          {teamScoreCard?.didNotBat.length > 0 && (
            <>
              <h6>
                <b>Did not Bat</b>
              </h6>
              <div className="did-not-bat-details">
                {teamScoreCard?.didNotBat.map((dnb) => (
                  <span key={dnb.name}>{dnb.name}</span>
                ))}{" "}
              </div>
            </>
          )}
        </div>
      </ExpandableSection>
      <ExpandableSection
        title={`${opponent} Bowling Scorecard`}
        isExpanded={true}
      >
        <table>
          <thead>
            <tr>
              <th>Player Name</th>
              <th>Overs Bowled</th>
              <th>Runs Conceded</th>
              <th>Wickets</th>
              <th>Economy</th>
              <th>Wide Balls</th>
              <th>No Balls</th>
            </tr>
          </thead>
          <tbody>
            {teamScoreCard?.bowlingScoreCard.map((player, index) => (
              <tr key={index}>
                <td style={{ backgroundColor: "pink" }}>
                  {player.playerName.name}
                </td>
                <td>{player.oversBowled}</td>
                <td>{player.runsConceded}</td>
                <td>{player.wickets}</td>
                <td>{player.economy.toPrecision(3)}</td>
                <td>{player.wideBall}</td>
                <td>{player.noBall}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </ExpandableSection>
    </div>
  );
};
