import axios, { AxiosResponse } from 'axios';
import React, { useEffect, useState, useContext } from 'react';
import { useQuery } from 'react-query';
import { Player } from '../../../CricketHooks/usePlayers';
import { CricketFormat } from '../../CricketMatchRecords/Models/Interface';
import { RecordAgainsOpponents } from '../../CricketTeamRecords/TeamDetails/RecordAgainstOpponents';
import { TeamDetailsTable } from '../../CricketTeamRecords/TeamDetails/TeamDetailsTable';
import { PlayerData } from '../Models/Interface';

import './PlayerDetails.scss';
import { PlayerDetailsTable } from './PlayerDetailsTable';

const fetchPlayerDataByName = (teamName: string, playerName: string): Promise<AxiosResponse<PlayerData>> => {
	return axios.get(`http://localhost:5104/cricketplayer/team/${teamName}/player/${playerName}`);
};

export const usePlayerByName = (teamName: string, playerName: string) => {
	const { isLoading, data } = useQuery([playerName, teamName], () => fetchPlayerDataByName(teamName, playerName), { cacheTime: 0 });

	console.log(data)

	return { playerData: data?.data, isLoading };
};

export interface PlayerDetailsProps {
	teamName: string,
	playerName: string,
	format: CricketFormat
}

export const PlayerDetails: React.FunctionComponent<PlayerDetailsProps> = ({ teamName, playerName, format }) => {

	const { playerData, isLoading } = usePlayerByName(teamName, playerName);


    return (
		<div className="player-details-container">
			<div className='player-info-container'>
				<h1>
					{
						playerName
					}
				</h1>
				<img className='player-image' src={playerData?.imageSrc} />
				<div>
					{/*<RecordAgainsOpponents teamName={teamName} format={format} />*/}
				</div>
			</div>
			<PlayerDetailsTable
				isLoading={isLoading}
				playerData={playerData as PlayerData}
				cricketFormat={format} />
		</div>
    );
};
