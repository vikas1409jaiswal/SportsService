import React from "react";
import { IndividualPages } from "./components/individual-pages-record/IndividualPages";
import { useCustomESPNTable } from "./hook/useCustomESPNTable";

interface CricketRecordsProps {}

export const CricketRecords: React.FC<CricketRecordsProps> = ({}) => {
  const rows = useCustomESPNTable();
  console.log(rows);
  return <IndividualPages rows={rows} />;
  //return <MovingTrainRecord rows={rows?.slice(0, 10)} />;
};
