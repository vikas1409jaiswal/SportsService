import axios, { AxiosResponse } from 'axios';
import React, { useEffect, useState, useContext } from 'react';
import { useQuery } from 'react-query';
import { GridLoader } from '../../../common/Loader';
import { CricketFormat } from '../../CricketMatchRecords/Models/Interface';
import { CricketTeamData } from '../Models/Interface';

const fetchTeamDataByName = (teamName: string): Promise<AxiosResponse<CricketTeamData>> => {
	return axios.get(`http://localhost:5104/cricketteam/teams/${teamName}/records`);
};

export const useTeamByName = (teamName: string) => {
	const { isLoading, data } = useQuery([teamName, 'team-data'], () => fetchTeamDataByName(teamName), { cacheTime: 0 });

	console.log(data)

	return { teamData: data?.data, isLoading };
};

export interface TeamDetailsTableProps {
	teamName: string,
	cricketFormat: CricketFormat
}

export const TeamDetailsTable: React.FunctionComponent<TeamDetailsTableProps> = ({ cricketFormat, teamName }) => {

	const { isLoading, teamData } = useTeamByName(teamName);

    const results = cricketFormat === CricketFormat.ODI ? teamData?.teamRecordDetails.odiResults : teamData?.teamRecordDetails.t20IResults;
    const mileStones = results?.teamMileStones;
    const inningDetails = mileStones?.inningRecordDetails;

	return (
        <div>
            <table className='team-details-table'>
                <thead>
                </thead>
                {
                    isLoading && <GridLoader/>
                }
                {
                    !isLoading && <tbody>
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
                            <th>Tied and Won</th>
                            <td>{results?.tiedAndWon}</td>
                        </tr>
                        <tr>
                            <th>Tied and Lost</th>
                            <td>{results?.tiedAndLost}</td>
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
                }
            </table>
            <table className='team-details-table'>
                <thead>
                </thead>
                {
                    isLoading && <GridLoader />
                }
                {
                    !isLoading && <tbody>
                        <tr>
                            <th>Debut</th>
                            <td>
                                <p>{results?.debut.date} vs {results?.debut.opponent}</p>
                                <p>{results?.debut.matchNo}</p>
                                <p>{results?.debut.venue}</p>
                            </td>
                        </tr>
                        <tr>
                            <th>Total Runs</th>
                            <td>{results?.teamMileStones.careerRuns}</td>
                        </tr>
                        <tr className='milestones-section'>
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
                            <td>{
                                `${inningDetails?.highestTotal.runs}/
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
                }
            </table>
            <table className='team-details-table'>
                <thead>
                </thead>
                {
                    isLoading && <GridLoader />
                }
                {
                    !isLoading && <tbody>
                        <tr>
                            <th>Most Runs</th>
                            <td>
                                <p>{mileStones?.mostRuns.key}</p>
                                <p><b>{mileStones?.mostRuns.value}</b></p>
                            </td>
                        </tr>
                        <tr>
                            <th>Most Wickets</th>
                            <td>
                                <p>{mileStones?.mostWickets.key}</p>
                                <p><b>{mileStones?.mostWickets.value}</b></p>
                            </td>
                        </tr>
                        <tr>
                            <th>Highest Individual Score</th>
                            <td>
                                <p>{mileStones?.highestIndividualScore.key}</p>
                                <p><b>{mileStones?.highestIndividualScore.value}</b></p>
                            </td>
                        </tr>
                        <tr>
                            <th>Most Sixes</th>
                            <td>
                                <p>{mileStones?.mostSixes.key}</p>
                                <p><b>{mileStones?.mostSixes.value}</b></p>
                            </td>
                        </tr>
                        <tr>
                            <th>Most Fours</th>
                            <td>
                                <p>{mileStones?.mostFours.key}</p>
                                <p><b>{mileStones?.mostFours.value}</b></p>
                            </td>
                        </tr>
                        <tr>
                            <th>Most 50s</th>
                            <td>
                                <p>{mileStones?.most50s.key}</p>
                                <p><b>{mileStones?.most50s.value}</b></p>
                            </td>
                        </tr>
                        <tr>
                            <th>Most 100s</th>
                            <td>
                                <p>{mileStones?.most100s.key}</p>
                                <p><b>{mileStones?.most100s.value}</b></p>
                            </td>
                        </tr>
                        <tr>
                            <th>Most 150s</th>
                            <td>
                                <p>{mileStones?.most150s.key}</p>
                                <p><b>{mileStones?.most150s.value}</b></p>
                            </td>
                        </tr>
                        <tr>
                            <th>Most 200s</th>
                            <td>
                                <p>{mileStones?.most200s.key}</p>
                                <p><b>{mileStones?.most200s.value}</b></p>
                            </td>
                        </tr>
                    </tbody>
                }
            </table>
        </div>
	);
};
