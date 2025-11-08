import { motion, useAnimation } from "framer-motion";
import React, { useContext } from "react";
import $ from "jquery";

import "./CricketTeam.scss";
import { CricketProfileContext } from "../../CricketPlayersProfile";

type CricketTeam = {
  // isCurrentSquadPlayers: boolean;
  // countryInfo: Country;
  // allCountriesIds: number[];
};

export const CricketTeam: React.FunctionComponent<CricketTeam> = () => {
  // const [isVisibleMenuOptions, setVisibleMenuOptions] = useState(false);

  const { selectedCountryDetails } = useContext(CricketProfileContext);

  const { teamName, flagUri } = selectedCountryDetails || {
    teamName: "...Loading",
    flagUri: "",
    teamUuid: "",
  };

  const teamHeaderControl = useAnimation();

  // const { showAllPlayers, setSelectedPlayerUsingDropdown } =
  //   useContext(SoccerContext);

  // useEffect(() => {
  //   teamHeaderControl.start({
  //     scale: [0.1, 1],
  //     transition: {
  //       type: "spring",
  //       stiffness: 100,
  //       damping: 10,
  //     },
  //   });
  // }, [currentSelectedIdIndex]);

  // if (!isVisibleMenuOptions) {
  //   setSelectedPlayerUsingDropdown(false);
  // }

  // // Press Shift + M for opening menu.
  // $(document).on({
  //   keydown: (event) => {
  //     // if (event.originalEvent?.key === "M") {
  //     //   setVisibleMenuOptions(!isVisibleMenuOptions);
  //     // }
  //   },
  // });

  return (
    <div className="cricket-team-player-header">
      <motion.img animate={teamHeaderControl} src={flagUri} />
      <motion.h1 animate={teamHeaderControl}>
        {`${teamName} National Cricket Team`}
      </motion.h1>
      {/* {isVisibleMenuOptions && <MenuOptions />} */}
    </div>
  );
};
