import React, { useContext, useEffect, useState } from 'react';
import { CricketContext } from '../../../CricketHomePage';
/*import { Countries } from '../../../SoccerHooks/useCountries';*/
import { teamData } from './../../../../data/TeamData';

import './CricketTeam.scss';

export interface CricketTeamProps {
}

export const CricketTeam: React.FunctionComponent<CricketTeamProps> = (props) => {

  //const teamHeaderControl = useAnimation();

  const cricketContext = useContext(CricketContext);

  //useEffect(() => {
  //    teamHeaderControl.start({
  //        scale: [0.1, 1],
  //        transition: {
  //            type: 'spring',
  //            stiffness: 100,
  //            damping: 10
  //        }
  //    })
  //}, [props.currentSelectedIdIndex])

  let teamInfo = teamData.find(td => td.cricketnationalteam === cricketContext.countryName);

  return (
    <>
      <div className='cricket-team-header'>
        <img src={teamInfo?.nationalflagurl} />
        <h1>{cricketContext.countryName} National Cricket Team</h1>
        <img src={teamInfo?.cricketlogourl} />
      </div>
      <div>
       </div>
    </>
  );
};