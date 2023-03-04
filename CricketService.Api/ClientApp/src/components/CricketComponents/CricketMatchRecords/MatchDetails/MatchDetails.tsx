import React from 'react';
import { CricketMatch } from '../Models/Interface';
import { TeamScoreCard } from './TeamScoreCard/TeamScoreCard';

import './MatchDetails.scss';

export interface MatchDetailsProps {
    matchData: CricketMatch;
}

export const MatchDetails: React.FunctionComponent<MatchDetailsProps> = ({ matchData }) => {

    const downloadJsonData = () => {
        const filepath = `${matchData.matchNo.replace(" ", "_").replace(" ", "_")}.json`;

        // Create a blob containing the data as JSON
        const blob = new Blob([JSON.stringify(matchData, null, 2)], { type: 'application/json' });

        // Create a URL for the blob
        const url = URL.createObjectURL(blob);

        // Create an <a> element and set its href and download attributes
        const link = document.createElement('a');
        link.href = url;
        link.download = filepath;

        // Programmatically click the link to trigger the download
        link.click();

        // Release the URL object
        URL.revokeObjectURL(url);
    }

    return (
        <div className="match-details-container">
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
                        <td>{matchData?.team1.teamName} {matchData?.team1.playing11.length}</td>
                        <td>{matchData?.team1.playing11.map(u => <p>{u}</p>)}</td>
                    </tr>
                    <tr>
                        <td>{matchData?.team2.teamName} {matchData?.team2.playing11.length}</td>
                        <td>{matchData?.team2.playing11.map(u => <p>{u}</p>)}</td>
                    </tr>
                    <tr>
                        <td>Umpires</td>
                        <td>{matchData?.umpires.map(u => <p>{u}</p>)}</td>
                    </tr>
                    {
                        matchData?.tvUmpire && <tr>
                            <td>TV Umpire:</td>
                            <td>{matchData?.tvUmpire}</td>
                        </tr>
                    }
                    {
                        matchData?.matchReferee && <tr>
                            <td>Match Referee:</td>
                            <td>{matchData?.matchReferee}</td>
                        </tr>
                    }
                    {
                        matchData?.reserveUmpire && <tr>
                            <td>Reserve Umpire:</td>
                            <td>{matchData?.reserveUmpire}</td>
                        </tr>
                    }
                    <tr>
                        <td>Debuts:</td>
                        <td>{matchData?.internationalDebut.map(d => <p>{d}</p>)}</td>
                    </tr>
                </tbody>
            </table>
            <div className='match-scoreboard'>
                <div className='first-inning-scorecard'>
                    <h1>First Inning</h1>
                    <TeamScoreCard teamScoreCard={matchData?.team1} opponent={matchData?.team2.teamName} />
                </div>
                <div className='second-inning-scorecard'>
                    <h1>Second Inning</h1>
                    <TeamScoreCard teamScoreCard={matchData?.team2} opponent={matchData?.team1.teamName} />
                </div>
                <div className='button-panel'>
                    <button onClick={downloadJsonData}>Download</button>
                    <button onClick={downloadJsonData}>Download</button>
                </div>
            </div>         
        </div>
    );
};
