import React, { useContext, useEffect, useState } from 'react';
import axios from 'axios';
import { CricketMatch, CricketFormat } from './Models/Interface';
import { MatchRecordsTable } from './MatchRecordsTable/MatchRecordsTable';

import './CricketMatchRecords.scss';

export interface CricketMatchRecordsProps {
}

export const CricketMatchRecords: React.FunctionComponent<CricketMatchRecordsProps> = () => {

    const [matchData, setMatchData] = useState<CricketMatch[]>([]);
    const [selectedFormat, setSelectedFormat] = useState<CricketFormat>(CricketFormat.ODI);

    const urlsMap = new Map<CricketFormat, string>();

    urlsMap.set(CricketFormat.T20I, 't20Match/all');
    urlsMap.set(CricketFormat.ODI, 'odiMatch/all');

    useEffect(() => {
        const fetchData = async () => {
            const result = await axios(
                `http://localhost:5104/cricketmatch/${urlsMap.get(selectedFormat)}`,
            );
            setMatchData(result.data);
        };
        fetchData();
    }, [selectedFormat]);

    console.log(matchData)

    return (
        <>
            <div className='format-selection'>
                {
                    [CricketFormat.ODI, CricketFormat.T20I, CricketFormat.Test]
                        .map(x => <span
                            key={x}
                            style={{ backgroundColor: selectedFormat === x ? 'blue' : 'black' }}
                            onClick={() => setSelectedFormat(x)}>
                            {x}
                        </span>)
                }
            </div>
            <MatchRecordsTable matchData={matchData as CricketMatch[]} />
        </>
    );
};
