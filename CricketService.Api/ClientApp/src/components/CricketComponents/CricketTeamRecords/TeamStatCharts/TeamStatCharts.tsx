import React from "react";
import Chart from "react-google-charts";
import { defaultOptions, LineChart } from "../../../common/charts/LineChart";
import { CricketTeamData } from "../Models/Interface";

export interface TeamStatChartsProps {
  isLoading: boolean;
  teamData: CricketTeamData[];
}

export const TeamStatCharts: React.FunctionComponent<TeamStatChartsProps> = ({
  teamData,
  isLoading,
}) => {
  return (
    <>
      <LineChart
        data={[
          ["Team", "Matches", "Won", "Lost"],
          ...teamData
            .filter((x) => x.teamRecordDetails.odiResults.matches > 0)
            .map((x) => [
              x.teamName,
              x.teamRecordDetails.odiResults.matches,
              x.teamRecordDetails.odiResults.won,
              x.teamRecordDetails.odiResults.lost,
            ]),
        ]}
        options={{ ...defaultOptions, title: "Total Matches" }}
      />
    </>
  );
};
