import axios, { AxiosResponse } from 'axios';
import React, { useEffect, useState, useContext } from 'react';
import { useQuery } from 'react-query';
import { GridLoader } from '../../../common/Loader';
import { CricketFormat } from '../../CricketMatchRecords/Models/Interface';
import { PlayerData } from '../Models/Interface';

export interface PlayerDetailsTableProps {
    playerData: PlayerData,
    isLoading: boolean,
    cricketFormat: CricketFormat
}

export const PlayerDetailsTable: React.FunctionComponent<PlayerDetailsTableProps> = ({ playerData, isLoading, cricketFormat }) => {

    const results = cricketFormat === CricketFormat.ODI ? playerData?.careerDetails.odiCareer : playerData?.careerDetails.t20Career;
    const battingStats = results?.battingStatistics;
    const bowlingStats = results?.bowlingStatistics;

	return (
        <div>
            <table className='player-details-table'>
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
                                <p>{results?.debutDetails?.date} vs {results?.debutDetails?.opponent}</p>
                                <p>{results?.debutDetails?.venue}</p>
                                <p>{results?.debutDetails?.matchNo}</p>
                            </td>
                        </tr>
                    </tbody>
                }
            </table>
            <table className='player-details-table'>
                <thead>
                </thead>
                {
                    isLoading && <GridLoader/>
                }
                {
                    !isLoading && <tbody>
                        <tr>
                            <th>Matches</th>
                            <td>{battingStats?.matches}</td>
                        </tr>
                        <tr>
                            <th>Innings</th>
                            <td>{battingStats?.innings}</td>
                        </tr>
                        <tr>
                            <th>Runs</th>
                            <td>{battingStats?.runs}</td>
                        </tr>
                        <tr>
                            <th>50s</th>
                            <td>{battingStats?.halfCenturies}</td>
                        </tr>
                        <tr>
                            <th>100s</th>
                            <td>{battingStats?.centuries}</td>
                        </tr>
                        <tr>
                            <th>Highest Score</th>
                            <td>{battingStats?.highestScore}</td>
                        </tr>
                        <tr>
                            <th>4s</th>
                            <td>{battingStats?.fours}</td>
                        </tr>
                        <tr>
                            <th>6s</th>
                            <td>{battingStats?.sixes}</td>
                        </tr>
                        <tr>
                            <th>Strike Rate</th>
                            <td>{battingStats?.strikeRate.toPrecision(5)}</td>
                        </tr>
                    </tbody>
                }
            </table>
            <table className='player-details-table'>
                <thead>
                </thead>
                {
                    isLoading && <GridLoader />
                }
                {
                    !isLoading && <tbody>
                        <tr>
                            <th>Matches</th>
                            <td>{bowlingStats?.matches}</td>
                        </tr>
                        <tr>
                            <th>Innings</th>
                            <td>{bowlingStats?.innings}</td>
                        </tr>
                        <tr>
                            <th>Wickets</th>
                            <td>{bowlingStats?.wickets}</td>
                        </tr>
                        <tr>
                            <th>Overs</th>
                            <td>{bowlingStats?.overs.overs}</td>
                        </tr>
                        <tr>
                            <th>Economy</th>
                            <td>{bowlingStats?.economy.toPrecision(4)}</td>
                        </tr>
                    </tbody>
                }
            </table>
        </div>
	);
};
