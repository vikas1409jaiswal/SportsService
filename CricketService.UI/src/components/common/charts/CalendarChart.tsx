import React from "react";
import { Chart } from "react-google-charts";

export interface CalendarChartOptions {
  title: string;
}

export interface CalendarChartProps {
  data: any;
  options: CalendarChartOptions;
}

export const CalendarChart: React.FunctionComponent<CalendarChartProps> = ({
  data,
  options,
}) => {
  return (
    <Chart
      chartType="Calendar"
      width="100%"
      height="4000px"
      data={data}
      options={options}
    />
  );
};
