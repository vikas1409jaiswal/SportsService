import React, { useState } from "react";
import { ESPNTableRow } from "../../hook/useCustomESPNTable";
import { IndividualPage } from "./IndividualPage";

export interface IndividualPagesProps {
  rows: ESPNTableRow[];
}

export const IndividualPages: React.FunctionComponent<IndividualPagesProps> = ({
  rows,
}) => {
  const [selectedRowIndex, setSelectedRowIndex] = useState(0);

  console.log(rows);

  return (
    <IndividualPage
      row={rows[selectedRowIndex]}
      selectedRowIndex={selectedRowIndex}
      setSelectedRowIndex={setSelectedRowIndex}
    />
  );
};
