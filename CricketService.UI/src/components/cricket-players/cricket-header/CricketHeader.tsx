import React from "react";
import { CricketTeam } from "./cricket-team/CricketTeam";
import { GenericModifiers } from "./../Types/SoccerTypes";

import "./CricketHeader.scss";

export interface CricketHeaderProps {}

export const CricketHeader: React.FunctionComponent<
  CricketHeaderProps
> = ({}) => {
  // const { isCurrentSquadPlayers, countryId, setCountryId } =
  //   useContext(SoccerContext);

  // const countryInfo = useCountryById(countryId, isCurrentSquadPlayers);

  // const allCountriesIds = useCountries(isCurrentSquadPlayers).map(
  //   (cd) => cd.id
  // );

  // useEffect(() => {
  //   setCountryId(allCountriesIds[currentSelectedIdIndex] || 1);
  // }, [currentSelectedIdIndex]);

  return (
    <>
      <div className="soccer-home-page-header">
        <CricketTeam
        // isCurrentSquadPlayers={isCurrentSquadPlayers}
        // countryInfo={countryInfo}
        // allCountriesIds={allCountriesIds}
        />
      </div>
    </>
  );
};
