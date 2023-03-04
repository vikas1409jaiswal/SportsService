import React, { createRef, useContext, useState } from 'react';
import { CricketContext } from '../../CricketHomePage';
/*import { Player } from '../../SoccerHooks/usePlayers';*/

import './CricketFooter.css';

export interface CricketFooterProps {
}

export const CricketFooter: React.FunctionComponent<CricketFooterProps> = (props) => {

  const cricketContext = useContext(CricketContext);

  //const setButtonBackgroundColor = (playerPosition: PlayerPosition) => {
  //    let bColor = '';
  //    if (playerPosition === soccerContext.selectedPlayerPosition) {
  //        bColor = 'red';
  //    }
  //    return bColor;
  //}

  return (
    <>
      <div className='cricket-home-page-footer'>
        <button>
          {'Batter'}
        </button>
        <button>
          {'Bowler'}
        </button>
        <button>
          {'All-Rounder'}
        </button>
        <button>
          {'WicketKeeper'}
        </button>
        {/*style={{ backgroundColor: setButtonBackgroundColor(PlayerPosition.forward) }}*/}
        {/*disabled={soccerContext.selectedPlayerPosition === PlayerPosition.forward}*/}
        {/*onClick={() => { soccerContext.setSelectedPlayerPosition(PlayerPosition.forward) }}*/}
      </div>
    </>
  );
};
