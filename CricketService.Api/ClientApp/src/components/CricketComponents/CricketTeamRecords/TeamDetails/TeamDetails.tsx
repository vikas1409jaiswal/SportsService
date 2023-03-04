import axios, { AxiosResponse } from 'axios';
import React, { useEffect, useState, useContext } from 'react';
import { useQuery } from 'react-query';
import { ReactTable } from '../../../common/ReactTable';
import { CricketFormat } from '../../CricketMatchRecords/Models/Interface';
import { CricketTeamData } from '../Models/Interface';
import { RecordAgainsOpponents } from './RecordAgainstOpponents';

import './TeamDetails.scss';
import { TeamDetailsTable } from './TeamDetailsTable';

export interface TeamDetailsProps {
	teamName: string,
	cricketFormat: CricketFormat,
	flagUrl: string
}

export const TeamDetails: React.FunctionComponent<TeamDetailsProps> = ({ cricketFormat, teamName, flagUrl }) => {

    return (
		<div className="team-details-container">
			<div className='team-info-container'>
				<h1>
					{
					  teamName
					} national cricket team
				</h1>
				<img className='flag-image' src={flagUrl} />
				<div>
					<RecordAgainsOpponents teamName={teamName} format={cricketFormat} />
				</div>
			</div>
			<TeamDetailsTable
				teamName={teamName}
				cricketFormat={cricketFormat} />
        </div>
    );
};
