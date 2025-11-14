import React from "react";
import { IndividualPages } from "./components/individual-pages-record/IndividualPages";
import { useCustomESPNTable } from "./hook/useCustomESPNTable";

import "./CricketRecords.scss";

interface CricketRecordsProps {}

export const CricketRecords: React.FC<CricketRecordsProps> = ({}) => {
  const rows = useCustomESPNTable();
  return <div className="cricket-records-container">
    <IndividualPages rows={rows?.slice(0, 10)} />
  </div>;
  //return <MovingTrainRecord rows={rows?.slice(0, 10)} />;
};
