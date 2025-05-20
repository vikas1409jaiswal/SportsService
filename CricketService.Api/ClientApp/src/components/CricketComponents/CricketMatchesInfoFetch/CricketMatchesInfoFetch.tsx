import React, { useEffect, useState, useContext } from "react";
import { CricketContext, CricketFormats } from "../../CricketHomePage";
import { useCricketMatchesBySeason } from "./useCricketMatches";
import { MatchDetailsBySeason } from "./MatchDetailsBySeason/MatchDetailsBySeason";

import "./CricketMatchesInfoFetch.css";
import { CricketMatchesBySeason } from "../../../models/espn-cricinfo-models/CricketMatchModels";
import { CricketFormat } from "../../../models/enums/CricketFormat";

export interface CricketMatchesInfoFetchProps {
  years: number[];
}

export const CricketMatchesInfoFetch: React.FunctionComponent<
  CricketMatchesInfoFetchProps
> = ({ years }) => {
  const formatOptions = [
    CricketFormats.T20ICricket,
    CricketFormats.ODICricket,
    CricketFormats.TestCricket,
  ];

  const seasonOptions = {
    "T20 International": getRangeYearsArray(2005, 2025),
    "One-day International": getRangeYearsArray(1971, 2025),
    "Test Cricket": getRangeYearsArray(1887, 2025),
  };

  const [currentSelectedFormat, setCurrentSelectedFormat] = useState(
    CricketFormats.ODICricket
  );
  //const [currentSelectedYear, setCurrentSelectedYear] = useState(seasonOptions[currentSelectedFormat][0]);

  const cricketMatchesBySeason: CricketMatchesBySeason[] =
    useCricketMatchesBySeason(CricketFormat.ODI, years);

  console.log(cricketMatchesBySeason);

  //<DropDown
  //             dropDownText={'Format'}
  //             dropownOptions={formatOptions}
  //             selectedValue={currentSelectedFormat}
  //             onSelect={(o) => setCurrentSelectedFormat(o as CricketFormats)} />
  //         <DropDown
  //             dropDownText={'Season'}
  //             dropownOptions={seasonOptions[currentSelectedFormat]}
  //             selectedValue={currentSelectedYear}
  //             onSelect={(o) => setCurrentSelectedYear(o)} />

  return (
    <>
      <div className="cricket-home-page-header">
        {/*<CricketTeam />*/}
        {/*countryInfo={}*/}
        {/*currentSelectedIdIndex={currentSelectedIdIndex}*/}
        {/*setCurrentSelectedIdIndex={setCurrentSelectedIdIndex}*/}

        {cricketMatchesBySeason && (
          <MatchDetailsBySeason matchDataBySeasons={cricketMatchesBySeason} />
        )}
      </div>
    </>
  );
};

export const getRangeYearsArray = (startYear: number, endYear: number) => {
  return Array.from({ length: endYear - startYear + 1 }, (_, i) =>
    (startYear + i).toString()
  );
};
