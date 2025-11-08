import React from "react";
import { Chart } from "react-google-charts";

export const defaultOptions = {
  title: "Line Chart",
  curveType: "function",
  legend: { position: "bottom" },
};

export interface LineChartOptions {
  title: string;
  curveType: string;
  legend: { position: string };
}

export interface LineChartProps {
  data: any;
  options: LineChartOptions;
}

export const LineChart: React.FunctionComponent<LineChartProps> = ({
  data,
  options,
}) => {
  return (
    <Chart
      chartType="LineChart"
      width="100%"
      height="400px"
      data={data}
      options={options}
    />
  );
};
