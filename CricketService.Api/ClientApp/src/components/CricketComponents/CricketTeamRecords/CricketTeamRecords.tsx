import axios, { AxiosResponse } from 'axios';
import React, { useContext, useEffect, useState } from 'react';
import { CricketFormat } from '../CricketMatchRecords/Models/Interface';
import { ODIRecords } from './ODIRecords/ODIRecords';
import { T20Records } from './T20Records/T20Records';
import { CricketTeamData } from './Models/Interface';

import './CricketTeamRecords.scss';
import { useQuery } from 'react-query';

const fetchAllTeams = (): Promise<AxiosResponse<CricketTeamData[]>> => {
	return axios.get(`http://localhost:5104/cricketteam/teams/all/records`);
};

export const useTeamRecords = () => {
	const { isLoading, data } = useQuery(['cricket-team'], () => fetchAllTeams(), { cacheTime: 60 * 60 * 1000 });

	console.log(data)

	return { data: data?.data, isLoading: false };
};

export interface CricketTeamRecordsProps {
}

export const CricketTeamRecords: React.FunctionComponent<CricketTeamRecordsProps> = () => {

	const [teamData, setTeamData] = useState([]);

	const [selectedFormat, setSelectedFormat] = useState<CricketFormat>(CricketFormat.T20I);

	const { isLoading, data } = useTeamRecords();

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
			{
				selectedFormat === CricketFormat.T20I && <T20Records
					isLoading={isLoading}
					teamData={fakeData as any} />
			}
			{
				selectedFormat === CricketFormat.ODI && <ODIRecords
					isLoading={isLoading}
					teamData={data as CricketTeamData[]} />
			}
		</>
	);
};

const fakeData = [
	{
		"teamUuid": "3b401028-7196-4614-9da6-9e9a084d4f68",
		"teamName": "Afghanistan",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 110,
				"won": 70,
				"lost": 37,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 0,
				"winPercentage": 63.63636363636363,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 141,
				"won": 70,
				"lost": 63,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 4,
				"winPercentage": 49.645390070921984,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://upload.wikimedia.org/wikipedia/commons/c/cd/Flag_of_Afghanistan_%282013%E2%80%932021%29.svg"
	},
	{
		"teamUuid": "1ea776b1-2c27-4ec9-a03b-947d4d8ce1ea",
		"teamName": "Africa XI",
		"teamRecordDetails": {
			"t20IResults": null,
			"odiResults": {
				"debut": null,
				"matches": 6,
				"won": 1,
				"lost": 4,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 16.666666666666664,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://upload.wikimedia.org/wikipedia/en/thumb/d/d9/International_Cricket_Council_%28logo%29.svg/1024px-International_Cricket_Council_%28logo%29.svg.png"
	},
	{
		"teamUuid": "78919243-e70f-430b-a3a5-bb2f70b80961",
		"teamName": "Asia XI",
		"teamRecordDetails": {
			"t20IResults": null,
			"odiResults": {
				"debut": null,
				"matches": 7,
				"won": 4,
				"lost": 2,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 57.14285714285714,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://upload.wikimedia.org/wikipedia/en/thumb/d/d9/International_Cricket_Council_%28logo%29.svg/1024px-International_Cricket_Council_%28logo%29.svg.png"
	},
	{
		"teamUuid": "4b08a169-2d73-4c26-9624-85c427667f37",
		"teamName": "Australia",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 174,
				"won": 91,
				"lost": 76,
				"tiedAndWon": 1,
				"tiedAndLost": 2,
				"noResult": 4,
				"winPercentage": 52.58620689655172,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 975,
				"won": 592,
				"lost": 339,
				"tiedAndWon": 0,
				"tiedAndLost": 9,
				"noResult": 34,
				"winPercentage": 60.71794871794872,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/au.svg"
	},
	{
		"teamUuid": "8c0b44f0-3344-4432-8046-b107f06e1840",
		"teamName": "Bangladesh",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 144,
				"won": 49,
				"lost": 92,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 3,
				"winPercentage": 34.02777777777778,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 403,
				"won": 146,
				"lost": 250,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 7,
				"winPercentage": 36.22828784119106,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/bd.svg"
	},
	{
		"teamUuid": "e916c70c-4166-4062-8016-07d58b4472e8",
		"teamName": "Bermuda",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 25,
				"won": 12,
				"lost": 10,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 48,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 35,
				"won": 7,
				"lost": 28,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 20,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/bm.svg"
	},
	{
		"teamUuid": "84c5d343-408d-4269-a8c8-58c354ccf089",
		"teamName": "Canada",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 49,
				"won": 26,
				"lost": 19,
				"tiedAndWon": 0,
				"tiedAndLost": 2,
				"noResult": 1,
				"winPercentage": 53.06122448979592,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 77,
				"won": 17,
				"lost": 58,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 2,
				"winPercentage": 22.07792207792208,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ca.svg"
	},
	{
		"teamUuid": "4197ae5e-9512-4b12-add8-1bab11d78727",
		"teamName": "East Africa",
		"teamRecordDetails": {
			"t20IResults": null,
			"odiResults": {
				"debut": null,
				"matches": 3,
				"won": 0,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "http://www.rankflags.com/wp-content/uploads/2015/11/East-African-Community-EAC-Flag.png"
	},
	{
		"teamUuid": "f2ce1c99-38df-465a-b8f3-514463872542",
		"teamName": "England",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 170,
				"won": 90,
				"lost": 72,
				"tiedAndWon": 2,
				"tiedAndLost": 0,
				"noResult": 6,
				"winPercentage": 53.529411764705884,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 776,
				"won": 390,
				"lost": 346,
				"tiedAndWon": 0,
				"tiedAndLost": 9,
				"noResult": 30,
				"winPercentage": 50.25773195876289,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://upload.wikimedia.org/wikipedia/commons/thumb/b/be/Flag_of_England.svg/1200px-Flag_of_England.svg.png?20210524072131"
	},
	{
		"teamUuid": "c31feeca-77b2-430d-8cd5-9048568051f4",
		"teamName": "Hong Kong",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 54,
				"won": 21,
				"lost": 31,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 38.88888888888889,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 26,
				"won": 9,
				"lost": 10,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 34.61538461538461,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/hk.svg"
	},
	{
		"teamUuid": "a2ec3a77-116f-41cf-83a9-2b32f5ddbb1e",
		"teamName": "ICC World XI",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 1,
				"won": 0,
				"lost": 1,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 4,
				"won": 1,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 25,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://upload.wikimedia.org/wikipedia/en/thumb/d/d9/International_Cricket_Council_%28logo%29.svg/1024px-International_Cricket_Council_%28logo%29.svg.png"
	},
	{
		"teamUuid": "8f21fd86-49ce-41a3-bf3e-16507e431847",
		"teamName": "India",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 199,
				"won": 127,
				"lost": 63,
				"tiedAndWon": 2,
				"tiedAndLost": 2,
				"noResult": 5,
				"winPercentage": 64.321608040201,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 1026,
				"won": 537,
				"lost": 435,
				"tiedAndWon": 0,
				"tiedAndLost": 9,
				"noResult": 43,
				"winPercentage": 52.33918128654971,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/in.svg"
	},
	{
		"teamUuid": "0e03b6de-d4bf-4266-9a1d-ee1641909bb1",
		"teamName": "Ireland",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 144,
				"won": 59,
				"lost": 66,
				"tiedAndWon": 1,
				"tiedAndLost": 1,
				"noResult": 7,
				"winPercentage": 41.31944444444444,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 182,
				"won": 75,
				"lost": 92,
				"tiedAndWon": 0,
				"tiedAndLost": 3,
				"noResult": 11,
				"winPercentage": 41.208791208791204,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ie.svg"
	},
	{
		"teamUuid": "58faa4f1-fe75-452c-be90-b392f1ce0da8",
		"teamName": "Kenya",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 67,
				"won": 31,
				"lost": 32,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 3,
				"winPercentage": 46.26865671641791,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 154,
				"won": 42,
				"lost": 107,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 5,
				"winPercentage": 27.27272727272727,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ke.svg"
	},
	{
		"teamUuid": "dacb6917-087c-424c-baaa-b5a26c209675",
		"teamName": "Namibia",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 41,
				"won": 27,
				"lost": 11,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 65.85365853658537,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 43,
				"won": 20,
				"lost": 15,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 46.51162790697674,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/na.svg"
	},
	{
		"teamUuid": "d3408a4e-2411-49b9-9d97-3b3b73e91a12",
		"teamName": "Nepal",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 57,
				"won": 33,
				"lost": 19,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 57.89473684210527,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 42,
				"won": 20,
				"lost": 11,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 1,
				"winPercentage": 47.61904761904761,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/np.svg"
	},
	{
		"teamUuid": "cd9a387b-34f7-4127-998d-54cb19161951",
		"teamName": "Netherlands",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 98,
				"won": 49,
				"lost": 39,
				"tiedAndWon": 0,
				"tiedAndLost": 2,
				"noResult": 3,
				"winPercentage": 50,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 101,
				"won": 34,
				"lost": 61,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 4,
				"winPercentage": 33.663366336633665,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/nl.svg"
	},
	{
		"teamUuid": "fc34abd5-cff0-4175-b6d5-0e1167ad169a",
		"teamName": "New Zealand",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 185,
				"won": 94,
				"lost": 78,
				"tiedAndWon": 1,
				"tiedAndLost": 8,
				"noResult": 4,
				"winPercentage": 51.08108108108108,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 797,
				"won": 366,
				"lost": 382,
				"tiedAndWon": 0,
				"tiedAndLost": 7,
				"noResult": 42,
				"winPercentage": 45.92220828105395,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/nz.svg"
	},
	{
		"teamUuid": "7e6c40bd-eab0-43b3-9cbd-6acb7c6f044d",
		"teamName": "Oman",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 54,
				"won": 23,
				"lost": 28,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 42.592592592592595,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 37,
				"won": 21,
				"lost": 11,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 1,
				"winPercentage": 56.75675675675676,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/om.svg"
	},
	{
		"teamUuid": "663c2fba-d54c-4213-90b4-006326a65c54",
		"teamName": "Pakistan",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 215,
				"won": 131,
				"lost": 76,
				"tiedAndWon": 1,
				"tiedAndLost": 2,
				"noResult": 5,
				"winPercentage": 61.16279069767442,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 948,
				"won": 496,
				"lost": 419,
				"tiedAndWon": 0,
				"tiedAndLost": 9,
				"noResult": 20,
				"winPercentage": 52.320675105485236,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/pk.svg"
	},
	{
		"teamUuid": "151ff82e-f41d-4fd6-98b8-0496cbee8b47",
		"teamName": "Papua New Guinea",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 43,
				"won": 0,
				"lost": 18,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 54,
				"won": 0,
				"lost": 32,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/pg.svg"
	},
	{
		"teamUuid": "9de315a6-3468-400e-9ae4-e00c2ac6c811",
		"teamName": "Scotland",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 83,
				"won": 35,
				"lost": 43,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 3,
				"winPercentage": 42.168674698795186,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 146,
				"won": 63,
				"lost": 67,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 7,
				"winPercentage": 43.15068493150685,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://upload.wikimedia.org/wikipedia/commons/thumb/1/10/Flag_of_Scotland.svg/1200px-Flag_of_Scotland.svg.png?20220822021708"
	},
	{
		"teamUuid": "1767104d-019d-43aa-a7fa-3dae9351e290",
		"teamName": "South Africa",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 165,
				"won": 94,
				"lost": 67,
				"tiedAndWon": 1,
				"tiedAndLost": 0,
				"noResult": 3,
				"winPercentage": 57.27272727272727,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 650,
				"won": 396,
				"lost": 227,
				"tiedAndWon": 0,
				"tiedAndLost": 6,
				"noResult": 21,
				"winPercentage": 60.92307692307693,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/za.svg"
	},
	{
		"teamUuid": "62cf0daf-a7f3-4693-9769-0b9d9e28460c",
		"teamName": "Sri Lanka",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 176,
				"won": 79,
				"lost": 92,
				"tiedAndWon": 1,
				"tiedAndLost": 2,
				"noResult": 2,
				"winPercentage": 45.17045454545455,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 881,
				"won": 399,
				"lost": 438,
				"tiedAndWon": 0,
				"tiedAndLost": 5,
				"noResult": 39,
				"winPercentage": 45.2894438138479,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/lk.svg"
	},
	{
		"teamUuid": "898587b9-8e24-4658-90aa-61bd0328e4ce",
		"teamName": "United Arab Emirates",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 72,
				"won": 0,
				"lost": 36,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 84,
				"won": 0,
				"lost": 48,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ae.svg"
	},
	{
		"teamUuid": "9150ee4f-762e-40ea-b927-3351489355f4",
		"teamName": "United States of America",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 21,
				"won": 0,
				"lost": 7,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 1,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 40,
				"won": 0,
				"lost": 17,
				"tiedAndWon": 0,
				"tiedAndLost": 2,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/us.svg"
	},
	{
		"teamUuid": "3dcdfe53-9799-412a-9836-7a0a1dc8d953",
		"teamName": "West Indies",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 176,
				"won": 71,
				"lost": 92,
				"tiedAndWon": 2,
				"tiedAndLost": 1,
				"noResult": 10,
				"winPercentage": 40.909090909090914,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 852,
				"won": 410,
				"lost": 402,
				"tiedAndWon": 0,
				"tiedAndLost": 10,
				"noResult": 30,
				"winPercentage": 48.12206572769953,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://upload.wikimedia.org/wikipedia/commons/thumb/1/18/WestIndiesCricketFlagPre1999.svg/729px-WestIndiesCricketFlagPre1999.svg.png?20180402193645"
	},
	{
		"teamUuid": "288f2dc3-0b3a-4242-937a-5f3346654b59",
		"teamName": "Zimbabwe",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 123,
				"won": 38,
				"lost": 82,
				"tiedAndWon": 1,
				"tiedAndLost": 1,
				"noResult": 1,
				"winPercentage": 31.300813008130078,
				"teamMileStones": null
			},
			"odiResults": {
				"debut": null,
				"matches": 556,
				"won": 144,
				"lost": 390,
				"tiedAndWon": 1,
				"tiedAndLost": 7,
				"noResult": 13,
				"winPercentage": 25.989208633093526,
				"teamMileStones": null
			},
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/zw.svg"
	},
	{
		"teamUuid": "b52e4aa8-7116-4cfa-9761-472461a964d3",
		"teamName": "Argentina",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 15,
				"won": 7,
				"lost": 7,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 46.666666666666664,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ar.svg"
	},
	{
		"teamUuid": "6adcaabc-0d3d-49d1-9102-3a52197e4d90",
		"teamName": "Austria",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 29,
				"won": 20,
				"lost": 7,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 68.96551724137932,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/at.svg"
	},
	{
		"teamUuid": "dc3eb908-5803-432d-b236-5fd92b5300e1",
		"teamName": "Bahamas",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 12,
				"won": 3,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 25,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/bs.svg"
	},
	{
		"teamUuid": "a56e8a3d-1934-4be5-9e87-403822beb46d",
		"teamName": "Bahrain",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 36,
				"won": 16,
				"lost": 17,
				"tiedAndWon": 2,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 47.22222222222222,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/bh.svg"
	},
	{
		"teamUuid": "378de77e-bd71-41da-be7e-048543c88b1f",
		"teamName": "Belgium",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 23,
				"won": 16,
				"lost": 6,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 69.56521739130434,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/be.svg"
	},
	{
		"teamUuid": "9b84ac39-5324-422b-839f-7c86e2fb2911",
		"teamName": "Belize",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 9,
				"won": 4,
				"lost": 4,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 44.44444444444444,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/bz.svg"
	},
	{
		"teamUuid": "592c764c-753b-49a4-9cf8-d340ce93c25a",
		"teamName": "Bhutan",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 9,
				"won": 4,
				"lost": 5,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 44.44444444444444,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/bt.svg"
	},
	{
		"teamUuid": "954b0707-0d78-4fc0-a99e-a2a4451b2a61",
		"teamName": "Botswana",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 22,
				"won": 8,
				"lost": 13,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 36.36363636363637,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/bw.svg"
	},
	{
		"teamUuid": "b85585fc-e57d-4842-ba51-3a106ba6dacc",
		"teamName": "Brazil",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 4,
				"won": 1,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 25,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/br.svg"
	},
	{
		"teamUuid": "94799e7c-744c-499d-99f2-0a426cd2e698",
		"teamName": "Bulgaria",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 40,
				"won": 13,
				"lost": 22,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 2,
				"winPercentage": 32.5,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/bg.svg"
	},
	{
		"teamUuid": "aa166bed-c013-488f-ab72-1ebba853d4ce",
		"teamName": "Cameroon",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 14,
				"won": 0,
				"lost": 13,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/cm.svg"
	},
	{
		"teamUuid": "6fc4e6c3-b5c3-4674-abc7-6ef3a1937cce",
		"teamName": "Cayman Islands",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 12,
				"won": 0,
				"lost": 5,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ky.svg"
	},
	{
		"teamUuid": "c0dbf106-68a7-4dea-8a3a-1ba1ff0de8a3",
		"teamName": "Chile",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 4,
				"won": 1,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 25,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/cl.svg"
	},
	{
		"teamUuid": "065c503d-ee2f-4d3c-95da-c3f041dac8a0",
		"teamName": "Cook Islands",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 6,
				"won": 3,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 50,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ck.svg"
	},
	{
		"teamUuid": "51ea2726-e80f-4bfb-89a5-90593645edb1",
		"teamName": "Costa Rica",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 3,
				"won": 0,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/cr.svg"
	},
	{
		"teamUuid": "eabc4585-f160-4b10-b046-3ee0b0786ef6",
		"teamName": "Croatia",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 5,
				"won": 2,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 40,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/hr.svg"
	},
	{
		"teamUuid": "beb60a9b-8190-44b1-a35a-aa7c62bda046",
		"teamName": "Cyprus",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 11,
				"won": 6,
				"lost": 5,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 54.54545454545454,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/cy.svg"
	},
	{
		"teamUuid": "f12f9645-cf4b-4826-8fe0-bc7e475290a4",
		"teamName": "Czech Republic",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 33,
				"won": 0,
				"lost": 15,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/cz.svg"
	},
	{
		"teamUuid": "a525f646-027e-4462-a87e-fd0e72d48a90",
		"teamName": "Denmark",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 26,
				"won": 12,
				"lost": 12,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 2,
				"winPercentage": 46.15384615384615,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/dk.svg"
	},
	{
		"teamUuid": "89f10ea1-f6fb-43c7-bbd0-e7943a94a941",
		"teamName": "Estonia",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 12,
				"won": 0,
				"lost": 11,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ee.svg"
	},
	{
		"teamUuid": "4c6ec84a-1ed8-483e-aa25-544eb0e41b86",
		"teamName": "Eswatini",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 13,
				"won": 1,
				"lost": 11,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 7.6923076923076925,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fb/Flag_of_Eswatini.svg/1280px-Flag_of_Eswatini.svg.png"
	},
	{
		"teamUuid": "a795ad27-b460-4a3b-9088-ecceec46fdf9",
		"teamName": "Fiji",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 6,
				"won": 3,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 50,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/fj.svg"
	},
	{
		"teamUuid": "84f6466b-21b8-4f9c-9a74-a324e6504274",
		"teamName": "Finland",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 19,
				"won": 10,
				"lost": 9,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 52.63157894736842,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/fi.svg"
	},
	{
		"teamUuid": "3485077e-d161-46dc-9b87-f67529e59ea6",
		"teamName": "France",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 9,
				"won": 4,
				"lost": 5,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 44.44444444444444,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/fr.svg"
	},
	{
		"teamUuid": "ec7b262c-e5ef-4a88-8161-5e5fe1b044c9",
		"teamName": "Gambia",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 7,
				"won": 1,
				"lost": 6,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 14.285714285714285,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/gm.svg"
	},
	{
		"teamUuid": "20b84e6f-3ed7-4e49-add5-235fc2c0df6a",
		"teamName": "Germany",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 40,
				"won": 24,
				"lost": 15,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 60,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/de.svg"
	},
	{
		"teamUuid": "30f293da-b95e-414d-84f7-485308af1612",
		"teamName": "Ghana",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 25,
				"won": 11,
				"lost": 13,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 44,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/gh.svg"
	},
	{
		"teamUuid": "7e9bb1a5-61d2-4f71-93f1-e454e77b27b9",
		"teamName": "Gibraltar",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 23,
				"won": 3,
				"lost": 18,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 0,
				"winPercentage": 13.043478260869565,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/gi.svg"
	},
	{
		"teamUuid": "c75ced8a-5a81-40f7-ab6e-6e96d82298c8",
		"teamName": "Greece",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 11,
				"won": 3,
				"lost": 7,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 27.27272727272727,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/gr.svg"
	},
	{
		"teamUuid": "fcec60ad-4ed6-48a0-ba49-56a4a3a5e25a",
		"teamName": "Guernsey",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 22,
				"won": 9,
				"lost": 11,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 1,
				"winPercentage": 40.909090909090914,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/gg.svg"
	},
	{
		"teamUuid": "00528004-b3cb-4905-9683-8abe6180fedd",
		"teamName": "Hungary",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 17,
				"won": 6,
				"lost": 8,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 35.294117647058826,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/hu.svg"
	},
	{
		"teamUuid": "1c5d03e5-a265-414e-b095-b955445b5aaf",
		"teamName": "Indonesia",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 7,
				"won": 4,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 57.14285714285714,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/id.svg"
	},
	{
		"teamUuid": "91cd4171-bf91-4578-8596-1cea6b477360",
		"teamName": "Iran",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 3,
				"won": 0,
				"lost": 2,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ir.svg"
	},
	{
		"teamUuid": "a44cfff2-f594-4a78-acb3-c5183af79676",
		"teamName": "Isle of Man",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 16,
				"won": 8,
				"lost": 7,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 50,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/im.svg"
	},
	{
		"teamUuid": "79568219-63a7-4263-8f83-caa70ae098ac",
		"teamName": "Israel",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 4,
				"won": 1,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 25,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/il.svg"
	},
	{
		"teamUuid": "a3654d75-0ca1-4f4c-b424-388525378cc6",
		"teamName": "Italy",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 23,
				"won": 14,
				"lost": 8,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 60.86956521739131,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/it.svg"
	},
	{
		"teamUuid": "cf95ad97-ccb5-40bb-93c0-26a510156f2e",
		"teamName": "Japan",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 7,
				"won": 5,
				"lost": 2,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 71.42857142857143,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/jp.svg"
	},
	{
		"teamUuid": "cf37ea1e-e848-46c7-884a-12dc8208a059",
		"teamName": "Jersey",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 31,
				"won": 20,
				"lost": 9,
				"tiedAndWon": 1,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 66.12903225806451,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/je.svg"
	},
	{
		"teamUuid": "a146153e-a843-4208-a502-8e182e310c51",
		"teamName": "Kuwait",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 27,
				"won": 15,
				"lost": 7,
				"tiedAndWon": 0,
				"tiedAndLost": 3,
				"noResult": 0,
				"winPercentage": 55.55555555555556,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/kw.svg"
	},
	{
		"teamUuid": "a95dcce9-3270-46cd-8458-7cdaef2ad77b",
		"teamName": "Lesotho",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 13,
				"won": 2,
				"lost": 10,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 15.384615384615385,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ls.svg"
	},
	{
		"teamUuid": "7ad0eab4-9902-4a14-999b-52c4cf2acb7f",
		"teamName": "Luxembourg",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 27,
				"won": 11,
				"lost": 11,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 40.74074074074074,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/lu.svg"
	},
	{
		"teamUuid": "1ce42f4e-52ae-4eb8-9283-1380f0aee768",
		"teamName": "Malawi",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 23,
				"won": 14,
				"lost": 6,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 3,
				"winPercentage": 60.86956521739131,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/mw.svg"
	},
	{
		"teamUuid": "e286efbb-0ac5-4f49-9dd1-d7c111912006",
		"teamName": "Malaysia",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 49,
				"won": 28,
				"lost": 17,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 2,
				"winPercentage": 57.14285714285714,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/my.svg"
	},
	{
		"teamUuid": "a8179e59-713b-4666-8dd8-29e752c4182f",
		"teamName": "Maldives",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 24,
				"won": 4,
				"lost": 19,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 16.666666666666664,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/mv.svg"
	},
	{
		"teamUuid": "61b6ff08-a43a-43c3-9edc-cab7beb27939",
		"teamName": "Mali",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 7,
				"won": 0,
				"lost": 5,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 2,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ml.svg"
	},
	{
		"teamUuid": "9c4b1fec-44c3-4c10-8273-33b5aeae1255",
		"teamName": "Malta",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 38,
				"won": 16,
				"lost": 17,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 1,
				"winPercentage": 42.10526315789473,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/mt.svg"
	},
	{
		"teamUuid": "32a359d7-c808-4a35-82d2-77804dc32858",
		"teamName": "Mexico",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 8,
				"won": 3,
				"lost": 5,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 37.5,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/mx.svg"
	},
	{
		"teamUuid": "507ef918-7294-4357-9b7e-83dbb26433b0",
		"teamName": "Mozambique",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 27,
				"won": 13,
				"lost": 13,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 48.148148148148145,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/mz.svg"
	},
	{
		"teamUuid": "9a8a7115-32e8-4b14-bbc7-c042726a2554",
		"teamName": "Nigeria",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 34,
				"won": 14,
				"lost": 18,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 41.17647058823529,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ng.svg"
	},
	{
		"teamUuid": "0c833c01-6b48-4110-8e1f-11417c0a0cbf",
		"teamName": "Norway",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 19,
				"won": 6,
				"lost": 13,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 31.57894736842105,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/no.svg"
	},
	{
		"teamUuid": "d22f4d75-04ee-4a53-81ea-88c74789cca0",
		"teamName": "Panama",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 11,
				"won": 4,
				"lost": 6,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 36.36363636363637,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/pa.svg"
	},
	{
		"teamUuid": "cdef5b78-2093-47a4-88fe-1b29bd76dddf",
		"teamName": "Peru",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 4,
				"won": 2,
				"lost": 2,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 50,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/pe.svg"
	},
	{
		"teamUuid": "e03845fc-64b5-4516-b37d-d27049793d01",
		"teamName": "Philippines",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 9,
				"won": 1,
				"lost": 6,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 11.11111111111111,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ph.svg"
	},
	{
		"teamUuid": "772422ac-9582-4fc7-9ce3-161730626aff",
		"teamName": "Portugal",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 13,
				"won": 9,
				"lost": 4,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 69.23076923076923,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/pt.svg"
	},
	{
		"teamUuid": "76965a88-c29b-4f80-80c7-f0c840965fc2",
		"teamName": "Qatar",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 33,
				"won": 19,
				"lost": 9,
				"tiedAndWon": 2,
				"tiedAndLost": 0,
				"noResult": 2,
				"winPercentage": 60.60606060606061,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/qa.svg"
	},
	{
		"teamUuid": "34bdf647-d83e-43d9-9aa3-db1c750a6a3b",
		"teamName": "Romania",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 28,
				"won": 21,
				"lost": 6,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 75,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ro.svg"
	},
	{
		"teamUuid": "ad9b38ef-4684-4961-9aef-827f9c5affaf",
		"teamName": "Rwanda",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 35,
				"won": 11,
				"lost": 22,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 31.428571428571427,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/rw.svg"
	},
	{
		"teamUuid": "0215586c-fd94-499f-aa86-c23e2d5b5d54",
		"teamName": "Samoa",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 10,
				"won": 2,
				"lost": 6,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 20,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ws.svg"
	},
	{
		"teamUuid": "9e755a94-8acd-46a6-8094-4101d8a80589",
		"teamName": "Saudi Arabia",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 19,
				"won": 8,
				"lost": 10,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 42.10526315789473,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/sa.svg"
	},
	{
		"teamUuid": "4c1c9a07-efdc-4c75-928d-f57a2e56fcfb",
		"teamName": "Serbia",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 18,
				"won": 4,
				"lost": 14,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 22.22222222222222,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/rs.svg"
	},
	{
		"teamUuid": "78240346-0f7c-429b-98c0-7ba67272e135",
		"teamName": "Seychelles",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 13,
				"won": 2,
				"lost": 8,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 3,
				"winPercentage": 15.384615384615385,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/sc.svg"
	},
	{
		"teamUuid": "7fa858b4-c948-48c4-bfd6-b7a4a71fcc9b",
		"teamName": "Sierra Leone",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 17,
				"won": 7,
				"lost": 10,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 41.17647058823529,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/sl.svg"
	},
	{
		"teamUuid": "b005085f-8530-4a53-8ced-54857ec34221",
		"teamName": "Singapore",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 35,
				"won": 11,
				"lost": 19,
				"tiedAndWon": 0,
				"tiedAndLost": 1,
				"noResult": 0,
				"winPercentage": 31.428571428571427,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/sg.svg"
	},
	{
		"teamUuid": "a3ad064c-42c3-4d7d-87d8-a9b31ad6ac18",
		"teamName": "Slovenia",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 4,
				"won": 0,
				"lost": 4,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/si.svg"
	},
	{
		"teamUuid": "eef81a30-b054-46a6-860e-a80d372a82ff",
		"teamName": "South Korea",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 4,
				"won": 0,
				"lost": 4,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/kr.svg"
	},
	{
		"teamUuid": "53a9c567-7704-49d2-bfee-d1f26a113e19",
		"teamName": "Spain",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 33,
				"won": 24,
				"lost": 8,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 1,
				"winPercentage": 72.72727272727273,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/es.svg"
	},
	{
		"teamUuid": "ad473b4f-5fe1-4d17-a89c-7b356df8751f",
		"teamName": "St Helena",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 7,
				"won": 2,
				"lost": 3,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 2,
				"winPercentage": 28.57142857142857,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://upload.wikimedia.org/wikipedia/commons/thumb/0/00/Flag_of_Saint_Helena.svg/1920px-Flag_of_Saint_Helena.svg.png"
	},
	{
		"teamUuid": "858690e9-2dc0-4016-bd44-d9052aacc286",
		"teamName": "Swaziland",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 6,
				"won": 1,
				"lost": 5,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 16.666666666666664,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/sz.svg"
	},
	{
		"teamUuid": "3880325f-88e7-40d4-906a-843b7b7f7589",
		"teamName": "Sweden",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 16,
				"won": 6,
				"lost": 10,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 37.5,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/se.svg"
	},
	{
		"teamUuid": "ccf9473b-8194-4e46-b738-3074bc673520",
		"teamName": "Switzerland",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 11,
				"won": 6,
				"lost": 4,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 54.54545454545454,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ch.svg"
	},
	{
		"teamUuid": "d606057c-fd91-4fb9-98f7-818ff2f1637f",
		"teamName": "Tanzania",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 39,
				"won": 28,
				"lost": 8,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 3,
				"winPercentage": 71.7948717948718,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/tz.svg"
	},
	{
		"teamUuid": "86240a42-9001-4b09-990b-6d286bd7f322",
		"teamName": "Thailand",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 14,
				"won": 1,
				"lost": 13,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 7.142857142857142,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/th.svg"
	},
	{
		"teamUuid": "0b540711-8b80-425b-9146-178920fd668b",
		"teamName": "Turkey",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 8,
				"won": 0,
				"lost": 7,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 0,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/tr.svg"
	},
	{
		"teamUuid": "20d928f6-262c-47e9-9183-1c94f42c1198",
		"teamName": "Uganda",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 54,
				"won": 37,
				"lost": 13,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 3,
				"winPercentage": 68.51851851851852,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/ug.svg"
	},
	{
		"teamUuid": "0aa24b6d-9e9c-4c97-befb-eb07b83c3100",
		"teamName": "Vanuatu",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 20,
				"won": 10,
				"lost": 5,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 50,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://flagcdn.com/vu.svg"
	},
	{
		"teamUuid": "d47642ab-54bc-4db6-99d6-3f3d3e784e4f",
		"teamName": "World-XI",
		"teamRecordDetails": {
			"t20IResults": {
				"debut": null,
				"matches": 3,
				"won": 1,
				"lost": 2,
				"tiedAndWon": 0,
				"tiedAndLost": 0,
				"noResult": 0,
				"winPercentage": 33.33333333333333,
				"teamMileStones": null
			},
			"odiResults": null,
			"testResults": null
		},
		"flagUri": "https://upload.wikimedia.org/wikipedia/en/thumb/d/d9/International_Cricket_Council_%28logo%29.svg/1024px-International_Cricket_Council_%28logo%29.svg.png"
	}
]