import { ArrowDataComparer } from "./common/ArrowDataComparer";
import { CricketPlayerResponse } from "./hook/useFetchPlayerAllMatches";
import { ESPNPlayerAllMatchesInfo } from "./hook/useFetchPlayerAllMatchesV2";
import { useMemo } from "react";
import playersRolesInfo from "./../common/players-roles-info.json";
import { BattingStatistics } from "../../components/CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";

type FeedData = {
  dataName: string;
  data1Text: string;
  data2Text: string;
  speechText: string;
};

export const  useProfileInfoJSX = (
  player1Data: CricketPlayerResponse,
  player2Data: CricketPlayerResponse
): JSX.Element[] => {
  const { playingRole: playingRole1, battingStyle: battingStyle1, bowlingStyle: bowlingStyle1 } = useMemo(() => {
    const match = playersRolesInfo.find(
      (p) => p.playerHref && player1Data?.playerHref && p.playerHref === player1Data?.playerHref
    );
    return {
      playingRole: match?.playingRole || "",
      battingStyle: match?.battingStyle || "",
      bowlingStyle: match?.bowlingStyle || "",
    };
  }, [player1Data?.playerHref]);

  const { playingRole: playingRole2, battingStyle: battingStyle2, bowlingStyle: bowlingStyle2 } = useMemo(() => {
    const match = playersRolesInfo.find(
      (p) => p.playerHref && player2Data?.playerHref && p.playerHref === player2Data?.playerHref
    );
    return {
      playingRole: match?.playingRole || "",
      battingStyle: match?.battingStyle || "",
      bowlingStyle: match?.bowlingStyle || "",
    };
  }, [player2Data?.playerHref]);

  const profileInfoBogies = [
    <ArrowDataComparer
      className="data age-container"
      dataName="Age"
      data1Text={player1Data?.fullName}
      data2Text={player2Data?.fullName}
      speechText={`hello name`}
    />,
    // <ArrowDataComparer
    //   className="data age-container"
    //   dataName="Age"
    //   data1Text={player1Data.age}
    //   data2Text={player2Data.age}
    //   speechText={`${player1Data.name}, Age, ${
    //     player1Data.age?.split("y")[0]
    //   } years, ${player2Data.name}, Age ${
    //     player2Data.age?.split("y")[0]
    //   } years`}
    // />,
    <ArrowDataComparer
      className="data batting-style-container"
      dataName="Batting Style"
      data1Text={battingStyle1}
      data2Text={battingStyle2}
      speechText={`Batting Style: ${player1Data?.fullName}, ${battingStyle1}, ${player2Data?.fullName}, ${battingStyle2}`}
    />,
    <ArrowDataComparer
      className="data bowling-style-container"
      dataName="Bowling Style"
      data1Text={bowlingStyle1}
      data2Text={bowlingStyle2}
      speechText={`Bowling Style: ${player1Data?.fullName}, ${bowlingStyle1}, ${player2Data?.fullName}, ${bowlingStyle2}`}
    />,
    <ArrowDataComparer
      className="data playing-role-container"
      dataName="Playing Role"
      data1Text={playingRole1}
      data2Text={playingRole2}
      speechText={`Playing Role: ${player1Data?.fullName}, ${playingRole1}, ${player2Data?.fullName}, ${playingRole2}`}
    />,
  ];
  return profileInfoBogies;
};

export const useBattingStatsJSX = (
  player1Name: string,
  player2Name: string,
  player1AddData: BattingStatistics,
  player2AddData: BattingStatistics
): JSX.Element[] => {
  const feedBattingStatsData: FeedData[] = [
    {
      dataName: "Matches",
      data1Text: player1AddData?.matches.toString() ?? "",
      data2Text: player2AddData?.matches.toString() ?? "",
      speechText: `let's see batting stats of both players. Matches: ${player1Name}, ${player1AddData?.matches} matches, ${player2Name}, ${player2AddData?.matches} matches`,
    },
    {
      dataName: "Runs",
      data1Text: player1AddData?.runs.toString() ?? "",
      data2Text: player2AddData?.runs.toString() ?? "",
      speechText: `Runs: ${player1Name}, ${player1AddData?.runs} runs, ${player2Name}, ${player2AddData?.runs} runs`,
    },
    {
      dataName: "Hundreds",
      data1Text: player1AddData?.centuries.toString() ?? "",
      data2Text: player2AddData?.centuries.toString() ?? "",
      speechText: `Hundreds: ${player1Name}, ${player1AddData?.centuries} hundreds, ${player2Name}, ${player2AddData?.centuries} hundreds`,
    },
    {
      dataName: "Fifties",
      data1Text: player1AddData?.halfCenturies.toString() ?? "",
      data2Text: player2AddData?.halfCenturies.toString() ?? "",
      speechText: `Fifties: ${player1Name}, ${player1AddData?.halfCenturies} fifties, ${player2Name}, ${player2AddData?.halfCenturies} fifties`,
    },
    {
      dataName: "Not Outs",
      data1Text: player1AddData?.notOuts?.toString() ?? "",
      data2Text: player2AddData?.notOuts?.toString() ?? "",
      speechText: `Not outs: ${player1Name}, ${player1AddData?.notOuts}, ${player2Name}, ${player2AddData?.notOuts}`,
    },
    {
      dataName: "Ducks",
      data1Text: player1AddData?.ducks?.toString() ?? "",
      data2Text: player2AddData?.ducks?.toString() ?? "",
      speechText: `Ducks: ${player1Name}, ${player1AddData?.ducks}, ${player2Name}, ${player2AddData?.ducks}`,
    },
    {
      dataName: "Fours",
      data1Text: player1AddData?.fours.toString() ?? "",
      data2Text: player2AddData?.fours.toString() ?? "",
      speechText: `Fours: ${player1Name}, ${player1AddData?.fours} fours, ${player2Name}, ${player2AddData?.fours} fours`,
    },
    {
      dataName: "Sixes",
      data1Text: player1AddData?.sixes.toString() ?? "",
      data2Text: player2AddData?.sixes.toString() ?? "",
      speechText: `Sixes: ${player1Name}, ${player1AddData?.sixes} sixes, ${player2Name}, ${player2AddData?.sixes} sixes`,
    },
    {
      dataName: "Average",
      data1Text: player1AddData?.average?.toString() ?? "",
      data2Text: player2AddData?.average?.toString() ?? "",
      speechText: `Average: ${player1Name}, ${player1AddData?.average}, ${player2Name}, ${player2AddData?.average}`,
    },
    {
      dataName: "Strike Rate",
      data1Text: player1AddData?.strikeRate?.toString() ?? "",
      data2Text: player2AddData?.strikeRate?.toString() ?? "",
      speechText: `Strike Rate: ${player1Name}, ${player1AddData?.strikeRate}, ${player2Name}, ${player2AddData?.strikeRate}`,
    },
  ];

  const battingStatsBogies = [
    <div className="batting-stats-header">Batting Statistics</div>,
  ];

  for (let i = 0; i < feedBattingStatsData.length; i = i + 2) {
    battingStatsBogies.push(
      <div className="batting-stats">
        <ArrowDataComparer
          className="stats-data"
          dataName={feedBattingStatsData[i].dataName}
          data1Text={feedBattingStatsData[i].data1Text}
          data2Text={feedBattingStatsData[i].data2Text}
          headWidth={75}
          tailWidth={215}
          speechText={feedBattingStatsData[i].speechText}
        />
        {feedBattingStatsData.length > i + 1 && (
          <ArrowDataComparer
            className="stats-data"
            dataName={feedBattingStatsData[i + 1].dataName}
            data1Text={feedBattingStatsData[i + 1].data1Text}
            data2Text={feedBattingStatsData[i + 1].data2Text}
            headWidth={75}
            tailWidth={215}
            speechText={feedBattingStatsData[i + 1].speechText}
          />
        )}
      </div>
    );
  }
  return battingStatsBogies;
};

export const useBowlingStatsJSX = (
  player1Name: string,
  player2Name: string,
  player1AddData: ESPNPlayerAllMatchesInfo,
  player2AddData: ESPNPlayerAllMatchesInfo
): JSX.Element[] => {
  const feedBowlingStatsData: FeedData[] = [
    {
      dataName: "Matches",
      data1Text: String(player1AddData.bowlingCareer.matches),
      data2Text: String(player2AddData.bowlingCareer.matches),
      speechText: `let's see bowling stats of both players. Matches: ${player1Name}, ${player1AddData.bowlingCareer.matches} matches, ${player2Name}, ${player2AddData.bowlingCareer.matches} matches`,
    },
    {
      dataName: "Wickets",
      data1Text: String(player1AddData.bowlingCareer.wickets),
      data2Text: String(player2AddData.bowlingCareer.wickets),
      speechText: `Wickets: ${player1Name}, ${player1AddData.bowlingCareer.wickets} wickets, ${player2Name}, ${player2AddData.bowlingCareer.wickets} wickets`,
    },
    {
      dataName: "4 Wickets",
      data1Text: String(player1AddData.bowlingCareer.fourWickets),
      data2Text: String(player2AddData.bowlingCareer.fourWickets),
      speechText: `4 Wicket Haul: ${player1Name}, ${player1AddData.bowlingCareer.fourWickets}, ${player2Name}, ${player2AddData.bowlingCareer.fourWickets}`,
    },
    {
      dataName: "5 Wickets",
      data1Text: String(player1AddData.bowlingCareer.fiveWickets),
      data2Text: String(player2AddData.bowlingCareer.fiveWickets),
      speechText: `5 Wicket Haul: ${player1Name}, ${player1AddData.bowlingCareer.fiveWickets}, ${player2Name}, ${player2AddData.bowlingCareer.fiveWickets}`,
    },
  ];

  const bowlingStatsBogies = [
    <div className="bowling-stats-header">Batting Statistics</div>,
  ];

  for (let i = 0; i < feedBowlingStatsData.length; i = i + 2) {
    bowlingStatsBogies.push(
      <div className="bowling-stats">
        <ArrowDataComparer
          className="stats-data"
          dataName={feedBowlingStatsData[i].dataName}
          data1Text={feedBowlingStatsData[i].data1Text}
          data2Text={feedBowlingStatsData[i].data2Text}
          headWidth={75}
          tailWidth={215}
          speechText={feedBowlingStatsData[i].speechText}
        />
        {feedBowlingStatsData.length > i + 1 && (
          <ArrowDataComparer
            className="stats-data"
            dataName={feedBowlingStatsData[i + 1].dataName}
            data1Text={feedBowlingStatsData[i + 1].data1Text}
            data2Text={feedBowlingStatsData[i + 1].data2Text}
            headWidth={75}
            tailWidth={215}
            speechText={feedBowlingStatsData[i + 1].speechText}
          />
        )}
      </div>
    );
  }
  return bowlingStatsBogies;
};
