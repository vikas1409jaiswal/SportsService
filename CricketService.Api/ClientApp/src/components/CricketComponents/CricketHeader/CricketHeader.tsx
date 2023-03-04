import React, { useEffect, useState, useContext } from 'react';
import { CricketContext, CricketFormats } from '../../CricketHomePage';
import { CricketMatchesBySeason, useCricketMatchesBySeason } from '../../CricketHooks/useCricketMatches';
import { MatchDetailsBySeason } from './MatchDetailsBySeason/MatchDetailsBySeason';
import { DropDown } from '../../common/DropDown';

import './CricketHeader.css';


export interface CricketHeaderProps {
    year: number;
}

export const CricketHeader: React.FunctionComponent<CricketHeaderProps> = ({ year }) => {

    const formatOptions = [CricketFormats.T20ICricket, CricketFormats.ODICricket, CricketFormats.TestCricket];

    const seasonOptions = {
        'T20 International': getRangeYearsArray(2005, 2023),
        'One-day International': getRangeYearsArray(1971, 2023),
        'Test Cricket': getRangeYearsArray(1887, 2023)
    }

    const [currentSelectedFormat, setCurrentSelectedFormat] = useState(CricketFormats.T20ICricket);
    //const [currentSelectedYear, setCurrentSelectedYear] = useState(seasonOptions[currentSelectedFormat][0]);

    const cricketMatchesBySeason: CricketMatchesBySeason = useCricketMatchesBySeason(year);

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
      <div className='cricket-home-page-header'>
      {/*<CricketTeam />*/}
      {/*countryInfo={}*/}
      {/*currentSelectedIdIndex={currentSelectedIdIndex}*/}
              {/*setCurrentSelectedIdIndex={setCurrentSelectedIdIndex}*/}
             
         {cricketMatchesBySeason && <MatchDetailsBySeason matchDataBySeason={cricketMatchesBySeason} />}
      </div>
    </>
  );
};

export const getRangeYearsArray = (startYear: number, endYear: number) => {
    return Array.from({ length: endYear - startYear + 1 }, (_, i) => (startYear + i).toString());
}
