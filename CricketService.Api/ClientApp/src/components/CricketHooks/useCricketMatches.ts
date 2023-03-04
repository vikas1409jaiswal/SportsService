import axios, { AxiosResponse } from 'axios';
import { useQueries, useQuery } from 'react-query';

export interface ApiData {
  data: any;
  meta: any;
}

export interface ApiResponse {
  data: ApiData;
  status: number;
  config: any;
  headers: any;
  request: any;
  statusText: string;
}

interface MatchDetails {
    team1: string;
    team2: string;
    winner: string;
    margin: string;
    ground: string;
    matchDate: string;
    matchNo: string;
    href: string;
}

export interface CricketMatchesBySeason {
    season: number;
    matchDetails: MatchDetails[];
}

export interface CricketMatch {
    season: string,
    series: string,
    playerOfTheMatch: string,
    matchNo: string,
    matchDays: string,
    matchTitle: string;
    venue: string;
    matchDate: string;
    tossWinner: string;
    tossDecision: string;
    result: string;
    team1: Team;
    team2: Team;
    tvUmpire: string,
    matchReferee: string,
    reserveUmpire: string,
    umpires: string[],
    internationalDebut: string[],
    formatDebut: string[]
}

export interface Team {
    teamName: string;
    battingScorecard: Batsman[];
    bowlingScorecard: Bowler[];
    extras: string,
    fallOfWickets: string[],
    didNotBat: Player[]
}

interface Player {
    name: string,
    href: string
}

interface Batsman {
    playerName: Player;
    outStatus: string;
    runsScored: number;
    ballsFaced: number;
    minutes: number;
    fours: number;
    sixes: number;
}

interface Bowler {
    playerName: Player;
    oversBowled: number;
    maidens: number;
    runsConceded: number;
    wickets: number;
    wideBall: number;
    noBall: number;
    dots: number;
    fours: number;
    sixes: number;
}

const fetchCricketMatches = (year: number): Promise<AxiosResponse<ApiData>> => {
    return axios.get(`https://stats.espncricinfo.com/ci/engine/records/team/match_results.html?class=1;id=${year};type=year`);
};

const fetchCricketMatch = (url: string): Promise<AxiosResponse<ApiData>> => {
    return axios.get(`https://stats.espncricinfo.com${url}`);
};

export const useCricketMatchesBySeason = (year: number): CricketMatchesBySeason => {
    const { data } = useQuery(['odi', year], () => fetchCricketMatches(year));

    const divElement = document.createElement('div');

    divElement.innerHTML = data?.data.toString() as string;

    const allTableRows = divElement.querySelector('.div630Pad table.engineTable')?.querySelectorAll('tr.data1');

    const cricketMatches: CricketMatchesBySeason = {
        season: year,
        matchDetails: []
    }

    allTableRows?.forEach(x => {
        const tdSelector = x?.querySelectorAll('td');
        cricketMatches.matchDetails.push({
            team1: tdSelector[0]?.textContent,
            team2: tdSelector[1]?.textContent,
            winner: tdSelector[2]?.textContent,
            margin: tdSelector[3]?.textContent,
            ground: tdSelector[4]?.textContent,
            matchDate: tdSelector[5]?.textContent,
            matchNo: tdSelector[6]?.textContent,
            href: tdSelector[6]?.querySelector('a')?.getAttribute('href')
        } as MatchDetails)
    });

    //console.log(cricketMatches.matchDetails);

    //console.log(cricketMatches)
    return cricketMatches;
};



export const useCricketMatches = (url: string): CricketMatch => {
   const { data } = useQuery(['t-20-match', url], () => fetchCricketMatch(url));

   const divElement = document.createElement('div');

   divElement.innerHTML = data?.data.toString() as string;

    const headerData = divElement.querySelector('.ds-w-full .ds-text-compact-xxs');
    const tableData = divElement.querySelector('table.ds-table.ds-table-sm');
    const tableRows = tableData?.querySelectorAll('tr');

   const teamsData = divElement.querySelectorAll('.ds-rounded-lg.ds-mt-2');

    const team1Data = teamsData[0];
    const team2Data = teamsData[1];
    const tablesTeam1Data = team1Data?.querySelectorAll('table.ds-w-full');
    const tablesTeam2Data = team2Data?.querySelectorAll('table.ds-w-full');

   // console.log(tablesTeam1Data, tablesTeam2Data)

    const teamNames = headerData?.querySelectorAll('.ci-team-score > div:first-child');
    const team1Name = teamNames?.item(0)?.getAttribute('title') as string;
    const team2Name = teamNames?.item(1)?.getAttribute('title') as string;
    const matchShortInfo = headerData?.querySelector('.ds-truncate')?.innerHTML as string;

    let tossDetail = '';
    let season = '';
    let series = '';
    let playerOfTheMatch = '';
    let matchNo = '';
    let matchDays = '';
    let tvUmpire = '';
    let matchReferee = '';
    let reserveUmpire = '';
    let umpires: string[] = [];
    let formatDebut: string[] = [];
    let internationalDebut: string[] = [];

    for (let i = 1; i < (tableRows?.length as number); i++) {
        const tableRow = tableRows?.item(i);
        const rowHeader = tableRow?.querySelectorAll('td span')[0]?.innerHTML;
        const rowValue = tableRow?.querySelectorAll('td span')[1]?.textContent as string;
        if (rowHeader === 'Toss') {
            tossDetail = rowValue
        }
        if (rowHeader === 'Season') {
            season = rowValue
        }
        if (rowHeader === 'Series') {
            series = rowValue
        }
        if (rowHeader === 'Player Of The Match') {
            playerOfTheMatch = rowValue
        }
        if (rowHeader === 'Match number') {
            matchNo = rowValue
        }
        if (rowHeader === 'Match days') {
            matchDays = rowValue
        }
        if (rowHeader === 'TV Umpire') {
            tvUmpire = rowValue
        }
        if (rowHeader === 'Reserve Umpire') {
            reserveUmpire = rowValue
        }
        if (rowHeader === 'Match Referee') {
            matchReferee = rowValue
        }
        if (rowHeader === 'Umpires') {
            const umpiresRows = tableRow?.querySelectorAll('td span');
            umpiresRows?.forEach((x, i) => i > 0 && umpires.push(x?.textContent as string));
            umpires = Array.from(new Set(umpires));
        }
        if (rowHeader === 'T20I debut') {
            const iDebutRows = tableRow?.querySelectorAll('td span');
            iDebutRows?.forEach((x, i) => i > 0 && internationalDebut.push(x?.textContent as string));
            internationalDebut = Array.from(new Set(internationalDebut));
        }
        if (rowHeader === 'T20 debut') {
            const debutRows = tableRow?.querySelectorAll('td span');
            debutRows?.forEach((x, i) => i > 0 && formatDebut.push(x?.textContent as string));
            formatDebut = Array.from(new Set(formatDebut));
        }
    }

    const cricketMatch: CricketMatch = {
        season,
        series,
        playerOfTheMatch,
        matchNo,
        matchDays,
        matchTitle: `${team1Name} vs ${team2Name}`,
        venue: tableData?.querySelector('span')?.innerHTML as string,
        matchDate: matchShortInfo,
        tossWinner: tossDetail.split(',')[0],
        tossDecision: tossDetail,
        result: headerData?.querySelector('p > span')?.innerHTML as string,
        team1: {
            teamName: team1Name,
            battingScorecard: [],
            bowlingScorecard: [],
            extras: '',
            fallOfWickets: [],
            didNotBat: []
        },
        team2: {
            teamName: team2Name,
            battingScorecard: [],
            bowlingScorecard: [],
            extras: '',
            fallOfWickets: [],
            didNotBat: []
        },
        tvUmpire,
        matchReferee,
        reserveUmpire,
        umpires,
        formatDebut,
        internationalDebut
    }

    const setTeamScoreData = (tablesTeamData: NodeListOf<Element>, teamName: string) => {
        const teamDetails = [cricketMatch.team1, cricketMatch.team2].find(x => x.teamName === teamName);

        tablesTeamData?.item(0)?.querySelectorAll('tbody tr').forEach(tr => {
            const scoreSelector = tr?.querySelectorAll('td') as NodeListOf<HTMLTableCellElement>;
            const pName = tr?.querySelector('td')?.textContent as string;
            const pHref = tr?.querySelector('td a')?.getAttribute('href') as string;
               !pName.includes('TOTAL')
                && !pName.includes('Fall of wickets:')
                && !pName.includes('Extras')
                && !pName.includes('Did not bat:')
                && pName.length > 0
                && teamDetails?.battingScorecard.push({
                playerName: {
                    name: pName,
                    href: pHref
                },
                outStatus: scoreSelector[1]?.textContent as string,
                runsScored: parseInt(tr?.querySelector('td strong')?.textContent as string),
                ballsFaced: parseInt(scoreSelector[3]?.textContent as string),
                minutes: parseInt(scoreSelector[4]?.textContent as string),
                fours: parseInt(scoreSelector[5]?.textContent as string),
                sixes: parseInt(scoreSelector[6]?.textContent as string),
              } as Batsman)

            if (pName.includes('Extra') && teamDetails) {
                teamDetails['extras'] = scoreSelector?.item(1)?.innerHTML;
            }

            if (pName.includes('Fall of wickets:') && teamDetails) {
                scoreSelector?.item(0)?.querySelectorAll('span').forEach((x, i) => {
                    i === 0 && teamDetails['fallOfWickets'].push(x?.textContent as string);
                    i > 0 && teamDetails['fallOfWickets'].push(x?.textContent?.slice(2) as string);
                })
            }

            if (pName.includes('Did not bat: ') && teamDetails) {
                scoreSelector?.item(0)?.querySelectorAll('a').forEach((x) => {
                    const dnpPlayerName = x?.querySelector('span')?.textContent?.replace(",", "").trim() as string;
                    const href = x?.getAttribute('href') as string;
                    teamDetails['didNotBat'].map(x => x.name)?.indexOf(dnpPlayerName) === -1 && teamDetails['didNotBat'].push({
                        name: dnpPlayerName,
                        href
                    });
                })
            }

        })

        tablesTeamData?.item(1)?.querySelectorAll('tbody tr:not(.ds-hidden)').forEach(tr => {
            const scoreSelector = tr?.querySelectorAll('td') as NodeListOf<HTMLTableCellElement>;
            const pName = tr?.querySelector('td')?.textContent as string;
            const pHref = tr?.querySelector('td a')?.getAttribute('href') as string;
            !pName.includes('Team:') &&
            teamDetails?.bowlingScorecard.push({
                playerName: {
                    name: pName,
                    href: pHref
                },
                oversBowled: parseFloat(scoreSelector[1]?.textContent as string),
                maidens: parseInt(scoreSelector[2]?.textContent as string),
                runsConceded: parseInt(scoreSelector[3]?.textContent as string),
                wickets: parseInt(tr?.querySelector('td strong')?.textContent as string),
                dots: parseInt(scoreSelector[6]?.textContent as string),
                fours: parseInt(scoreSelector[7]?.textContent as string),
                sixes: parseInt(scoreSelector[8]?.textContent as string),
                wideBall: parseInt(scoreSelector[9]?.textContent as string),
                noBall: parseInt(scoreSelector[10]?.textContent as string),
            } as Bowler)
        })
    }

    setTeamScoreData(tablesTeam1Data, team1Name);
    setTeamScoreData(tablesTeam2Data, team2Name);


    //console.log(cricketMatch)
    //console.table(cricketMatch.team1.battingScorecard)
    //console.table(cricketMatch.team1.bowlingScorecard)
    //console.table(cricketMatch.team2.battingScorecard)
    //console.table(cricketMatch.team2.bowlingScorecard)

  return cricketMatch;
};

export const useCricketMatchesBySeasonDetails = (matchDetails: MatchDetails[]) => {

    const queries = [];

    const queryOptions = {
        refetchOnWindowFocus: false,
        refetchOnMount: false,
        enabled: true,
        cacheTime: 60*60*1000,
        retry: true,
    };

    for (let i = 0; i < matchDetails.length; i++) {
        queries.push({
            queryKey: ['wikipedia', matchDetails[i].href],
            queryFn: () => fetchCricketMatch(matchDetails[i].href),
            ...queryOptions
        });
    }

    const result = useQueries(queries);

    const cricketMatches: CricketMatch[] = [];

    result.map((r, i) => {

        const divElement = document.createElement('div');

        divElement.innerHTML = (r.data as ApiResponse)?.data.toString() as string;

        const headerData = divElement.querySelector('.ds-w-full .ds-text-compact-xxs');
        const tableData = divElement.querySelector('table.ds-table.ds-table-sm');
        const tableRows = tableData?.querySelectorAll('tr');

        const teamsData = divElement.querySelectorAll('.ds-rounded-lg.ds-mt-2');

        const team1Data = teamsData[0];
        const team2Data = teamsData[1];
        const tablesTeam1Data = team1Data?.querySelectorAll('table.ds-w-full');
        const tablesTeam2Data = team2Data?.querySelectorAll('table.ds-w-full');

        //console.log(tablesTeam1Data, tablesTeam2Data)

        const teamNames = headerData?.querySelectorAll('.ci-team-score > div:first-child');
        const team1Name = teamNames?.item(0)?.getAttribute('title') as string;
        const team2Name = teamNames?.item(1)?.getAttribute('title') as string;

        let tossDetail = '';
        let season = '';
        let series = '';
        let playerOfTheMatch = '';
        let matchNo = '';
        let matchDays = '';
        let tvUmpire = '';
        let matchReferee = '';
        let reserveUmpire = '';
        let umpires: string[] = [];
        let formatDebut: string[] = [];
        let internationalDebut: string[] = [];

        for (let i = 1; i < (tableRows?.length as number); i++) {
            const tableRow = tableRows?.item(i);
            const rowHeader = tableRow?.querySelectorAll('td span')[0]?.innerHTML;
            const rowValue = tableRow?.querySelectorAll('td span')[1]?.textContent as string;
            if (rowHeader === 'Toss') {
                tossDetail = rowValue
            }
            if (rowHeader === 'Season') {
                season = rowValue
            }
            if (rowHeader === 'Series') {
                series = rowValue
            }
            if (rowHeader === 'Player Of The Match') {
                playerOfTheMatch = rowValue
            }
            if (rowHeader === 'Match number') {
                matchNo = rowValue
            }
            if (rowHeader === 'Match days') {
                matchDays = rowValue
            }
            if (rowHeader === 'TV Umpire') {
                tvUmpire = rowValue
            }
            if (rowHeader === 'Reserve Umpire') {
                reserveUmpire = rowValue
            }
            if (rowHeader === 'Match Referee') {
                matchReferee = rowValue
            }
            if (rowHeader === 'Umpires') {
                const umpiresRows = tableRow?.querySelectorAll('td span');
                umpiresRows?.forEach((x, i) => i > 0 && umpires.push(x?.textContent as string));
                umpires = Array.from(new Set(umpires));
            }
            if (rowHeader === 'T20I debut') {
                const iDebutRows = tableRow?.querySelectorAll('td span');
                iDebutRows?.forEach((x, i) => i > 0 && internationalDebut.push(x?.textContent as string));
                internationalDebut = Array.from(new Set(internationalDebut));
            }
            if (rowHeader === 'T20 debut') {
                const debutRows = tableRow?.querySelectorAll('td span');
                debutRows?.forEach((x, i) => i > 0 && formatDebut.push(x?.textContent as string));
                formatDebut = Array.from(new Set(formatDebut));
            }
        }

        const cricketMatch: CricketMatch = {
            season,
            series,
            playerOfTheMatch,
            matchNo,
            matchDays,
            matchTitle: `${team1Name} vs ${team2Name}`,
            venue: tableData?.querySelector('span')?.innerHTML as string,
            matchDate: matchDetails[i].matchDate,
            tossWinner: tossDetail.split(',')[0],
            tossDecision: tossDetail,
            result: headerData?.querySelector('p > span')?.innerHTML as string,
            team1: {
                teamName: team1Name,
                battingScorecard: [],
                bowlingScorecard: [],
                extras: '',
                fallOfWickets: [],
                didNotBat: []
            },
            team2: {
                teamName: team2Name,
                battingScorecard: [],
                bowlingScorecard: [],
                extras: '',
                fallOfWickets: [],
                didNotBat: []
            },
            tvUmpire,
            matchReferee,
            reserveUmpire,
            umpires,
            formatDebut,
            internationalDebut
        }

        const setTeamScoreData = (tablesTeamData: NodeListOf<Element>, teamName: string) => {
            const teamDetails = [cricketMatch.team1, cricketMatch.team2].find(x => x.teamName === teamName);

            tablesTeamData?.item(0)?.querySelectorAll('tbody tr').forEach(tr => {
                const scoreSelector = tr?.querySelectorAll('td') as NodeListOf<HTMLTableCellElement>;
                const pName = tr?.querySelector('td')?.textContent as string;
                const pHref = tr?.querySelector('td a')?.getAttribute('href') as string;
                !pName.includes('TOTAL')
                    && !pName.includes('Fall of wickets:')
                    && !pName.includes('Extras')
                    && !pName.includes('Did not bat:')
                    && pName.length > 0
                    && teamDetails?.battingScorecard.push({
                        playerName: {
                            name: pName,
                            href: pHref
                        },
                        outStatus: scoreSelector[1]?.textContent as string,
                        runsScored: parseInt(tr?.querySelector('td strong')?.textContent as string),
                        ballsFaced: parseInt(scoreSelector[3]?.textContent as string),
                        minutes: parseInt(scoreSelector[4]?.textContent as string),
                        fours: parseInt(scoreSelector[5]?.textContent as string),
                        sixes: parseInt(scoreSelector[6]?.textContent as string)
                    } as Batsman)

                if (pName.includes('Extra') && teamDetails) {
                    teamDetails['extras'] = scoreSelector?.item(1)?.innerHTML;
                }

                if (pName.includes('Fall of wickets:') && teamDetails) {
                    scoreSelector?.item(0)?.querySelectorAll('span').forEach((x, i) => {
                        i === 0 && teamDetails['fallOfWickets'].push(x?.textContent as string);
                        i > 0 && teamDetails['fallOfWickets'].push(x?.textContent?.slice(2) as string);
                    })
                }

                if (pName.includes('Did not bat: ') && teamDetails) {
                    scoreSelector?.item(0)?.querySelectorAll('a').forEach((x) => {
                        const dnpPlayerName = x?.querySelector('span')?.textContent?.replace(",", "").trim() as string;
                        const href = x?.getAttribute('href') as string;
                        teamDetails['didNotBat'].map(x => x.name)?.indexOf(dnpPlayerName) === -1 && teamDetails['didNotBat'].push({
                            name: dnpPlayerName,
                            href
                        });
                    })
                }

            })

            tablesTeamData?.item(1)?.querySelectorAll('tbody tr:not(.ds-hidden)').forEach(tr => {
                const scoreSelector = tr?.querySelectorAll('td') as NodeListOf<HTMLTableCellElement>;
                const pName = tr?.querySelector('td')?.textContent as string;
                const pHref = tr?.querySelector('td a')?.getAttribute('href') as string;
                !pName.includes('Team:') &&
                teamDetails?.bowlingScorecard.push({
                    playerName: {
                      name: pName,
                      href: pHref
                    },
                    oversBowled: parseFloat(scoreSelector[1]?.textContent as string),
                    maidens: parseInt(scoreSelector[2]?.textContent as string),
                    runsConceded: parseInt(scoreSelector[3]?.textContent as string),
                    wickets: parseInt(tr?.querySelector('td strong')?.textContent as string),
                    dots: parseInt(scoreSelector[6]?.textContent as string),
                    fours: parseInt(scoreSelector[7]?.textContent as string),
                    sixes: parseInt(scoreSelector[8]?.textContent as string),
                    wideBall: parseInt(scoreSelector[9]?.textContent as string),
                    noBall: parseInt(scoreSelector[10]?.textContent as string),
                } as Bowler)
            })
        }

        setTeamScoreData(tablesTeam1Data, team1Name);
        setTeamScoreData(tablesTeam2Data, team2Name);

        cricketMatches.push(cricketMatch)
    });

    const fetchedLength = cricketMatches.filter(x => x.series.length > 0).length;

    console.log(fetchedLength, '/', cricketMatches.length);

    if (fetchedLength  === cricketMatches.length) {
        console.log(cricketMatches)
    }

    return null;
};