import React from "react";
import { IndividualPages } from "./components/individual-pages-record/IndividualPages";
import { useCustomESPNTable } from "./hook/useCustomESPNTable";

interface CricketRecordsProps {}

export const CricketRecords: React.FC<CricketRecordsProps> = ({}) => {
  const rows = useCustomESPNTable();
  return <IndividualPages rows={rows?.slice(0, 10)} />;
  //return <MovingTrainRecord rows={rows?.slice(0, 10)} />;
};
