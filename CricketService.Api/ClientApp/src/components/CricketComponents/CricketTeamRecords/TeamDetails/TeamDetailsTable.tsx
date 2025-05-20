import axios, { AxiosResponse } from "axios";
import React, { useEffect, useState, useContext } from "react";
import { useQuery } from "react-query";
import { GridLoader } from "../../../common/Loader";
import { CricketTeamData } from "../Models/Interface";
import { CricketFormat } from "../../../../models/enums/CricketFormat";

const fetchTeamDataByTeamUuid = (
  teamUuid: string
): Promise<AxiosResponse<CricketTeamData>> => {
  return axios.get(`http://localhost:5104/cricketteam/team/${teamUuid}`);
};

export const useTeamByUuid = (teamUuid: string) => {
  const { isLoading, data } = useQuery(
    [teamUuid, "team-data"],
    () => fetchTeamDataByTeamUuid(teamUuid),
    { cacheTime: 0 }
  );

  console.log(data);

  return { teamData: data?.data, isLoading };
};

export interface TeamDetailsTableProps {
  teamName: string;
  teamUuid: string;
  cricketFormat: CricketFormat;
}

export const TeamDetailsTable: React.FunctionComponent<
  TeamDetailsTableProps
> = ({ cricketFormat, teamName, teamUuid }) => {
  const { isLoading, teamData } = useTeamByUuid(teamUuid);

  const results =
    cricketFormat === CricketFormat.ODI
      ? teamData?.teamRecordDetails.odiResults
      : teamData?.teamRecordDetails.t20IResults;
  const mileStones = results?.teamMileStones;
  const inningDetails = mileStones?.inningRecordDetails;

  return (
    <div>
      <table className="team-details-table">
        <thead></thead>
        {isLoading && <GridLoader />}
        {!isLoading && (
          <tbody>
            <tr>
              <th>Matches</th>
              <td>{results?.matches}</td>
            </tr>
            <tr>
              <th>Won</th>
              <td>{results?.won}</td>
            </tr>
            <tr>
              <th>Lost</th>
              <td>{results?.lost}</td>
            </tr>
            <tr>
              <th>Tied</th>
              <td>{results?.tied}</td>
            </tr>
            <tr>
              <th>No Result</th>
              <td>{results?.noResult}</td>
            </tr>
            <tr>
              <th>Win Percentage</th>
              <td>{results?.winPercentage.toPrecision(4)}</td>
            </tr>
          </tbody>
        )}
      </table>
      <table className="team-details-table">
        <thead></thead>
        {isLoading && <GridLoader />}
        {!isLoading && (
          <tbody>
            <tr>
              <th>Debut</th>
              <td>
                <p>
                  {results?.debut.date} vs {results?.debut.opponent}
                </p>
                <p>{results?.debut.matchNo}</p>
                <p>{results?.debut.venue}</p>
              </td>
            </tr>
            <tr>
              <th>Total Runs</th>
              <td>{results?.teamMileStones.careerRuns}</td>
            </tr>
            <tr className="milestones-section">
              <tr>
                <th>50s</th>
                <th>100s</th>
                <th>150s</th>
                <th>200s</th>
              </tr>
              <tr>
                <td>{results?.teamMileStones.careerHalfCenturies}</td>
                <td>{results?.teamMileStones.careerCenturies}</td>
                <td>{results?.teamMileStones.careerOneAndHalfCenturies}</td>
                <td>{results?.teamMileStones.careerDoubleCenturies}</td>
              </tr>
            </tr>
            <tr>
              <th>Higest Total</th>
              <td>{`${inningDetails?.highestTotal.runs}/
                                 ${inningDetails?.highestTotal.wickets}
                                 (${inningDetails?.highestTotal.overs.overs} overs)`}</td>
            </tr>
            <tr>
              <th>Lowest Total</th>
              <td>{`
                                 ${inningDetails?.lowestTotal.runs}/
                                 ${inningDetails?.lowestTotal.wickets}
                                 (${inningDetails?.highestTotal.overs.overs} overs)`}</td>
            </tr>
          </tbody>
        )}
      </table>
      <table className="team-details-table">
        <thead></thead>
        {isLoading && <GridLoader />}
        {!isLoading && (
          <tbody>
            <tr>
              <th>Most Innings</th>
              <td>
                <p>{mileStones?.mostInnings.key}</p>
                <p>
                  <b>{mileStones?.mostInnings.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Most Runs</th>
              <td>
                <p>{mileStones?.mostRuns.key}</p>
                <p>
                  <b>{mileStones?.mostRuns.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Most Wickets</th>
              <td>
                <p>{mileStones?.mostWickets.key}</p>
                <p>
                  <b>{mileStones?.mostWickets.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Highest Individual Score</th>
              <td>
                <p>{mileStones?.highestIndividualScore.key}</p>
                <p>
                  <b>{mileStones?.highestIndividualScore.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Best Bowling Inning</th>
              <td>
                <p>{mileStones?.bestBowlingInning.key}</p>
                <p>
                  <b>{mileStones?.bestBowlingInning.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Best Bowling Match</th>
              <td>
                <p>{mileStones?.bestBowlingMatch.key}</p>
                <p>
                  <b>{mileStones?.bestBowlingMatch.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Most Sixes</th>
              <td>
                <p>{mileStones?.mostSixes.key}</p>
                <p>
                  <b>{mileStones?.mostSixes.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Most Fours</th>
              <td>
                <p>{mileStones?.mostFours.key}</p>
                <p>
                  <b>{mileStones?.mostFours.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Most 50s</th>
              <td>
                <p>{mileStones?.most50s.key}</p>
                <p>
                  <b>{mileStones?.most50s.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Most 100s</th>
              <td>
                <p>{mileStones?.most100s.key}</p>
                <p>
                  <b>{mileStones?.most100s.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Most 150s</th>
              <td>
                <p>{mileStones?.most150s.key}</p>
                <p>
                  <b>{mileStones?.most150s.value}</b>
                </p>
              </td>
            </tr>
            <tr>
              <th>Most 200s</th>
              <td>
                <p>{mileStones?.most200s.key}</p>
                <p>
                  <b>{mileStones?.most200s.value}</b>
                </p>
              </td>
            </tr>
          </tbody>
        )}
      </table>
    </div>
  );
};
