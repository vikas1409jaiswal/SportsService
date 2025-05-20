import React from "react";

import "./MenuOptions.scss";

const seasons = [
  "AtoZ",
  "Current",
  "2018-19",
  "2019-20",
  "2020-21",
  "2021-22",
  "2022-23",
];

export interface MenuOptionsProps {}

export const MenuOptions: React.FunctionComponent<MenuOptionsProps> = (
  props
) => {
  // const compareCountries = (a: Country, b: Country) => {
  //   if (a.name < b.name) {
  //     return -1;
  //   }
  //   if (a.name > b.name) {
  //     return 1;
  //   }
  //   return 0;
  // };

  // const comparePlayers = (a: SoccerPlayer, b: SoccerPlayer) => {
  //   if (a.playerName != null && b.playerName != null) {
  //     if (a.playerName < b.playerName) {
  //       return -1;
  //     }
  //     if (a.playerName > b.playerName) {
  //       return 1;
  //     }
  //   }
  //   return 0;
  // };

  // const {
  //   isCurrentSquadPlayers,
  //   countryId,
  //   setCountryId,
  //   selectedPlayerId,
  //   setSelectedPlayerId,
  //   setSelectedPlayerPosition,
  //   setSelectedPlayerUsingDropdown,
  //   selectedSeasonForPlayers,
  //   setSeasonForPlayers,
  // } = useContext(SoccerContext);

  // const countries = useCountries(isCurrentSquadPlayers);

  // const selectedCountryName = countries.filter((c) => c.id === countryId)[0]
  //   ?.name;

  // const allPlayers = useWikipediaByCountry(
  //   selectedCountryName,
  //   isCurrentSquadPlayers
  // );

  // const allPlayersList = [
  //   ...allPlayers.currentSquad,
  //   ...allPlayers.recentCallUps,
  // ];

  // const selectedPlayerName = allPlayersList.filter(
  //   (p) => p.playerId === selectedPlayerId
  // )[0]?.playerName;

  // const handleCountryChange = (e: any) => {
  //   setCountryId(countries.filter((c) => c.name === e.target.value)[0]?.id);
  // };

  // const handleSeasonChange = (e: any) => {
  //   setSeasonForPlayers(e.target.value);
  // };

  // const handlePlayerChange = (e: any) => {
  //   const getPlayerPosition = (position: any) => {
  //     if (position === PlayerPosition.forward) {
  //       return PlayerPosition.forward;
  //     }
  //     if (position === PlayerPosition.midFielder) {
  //       return PlayerPosition.midFielder;
  //     }
  //     if (position === PlayerPosition.defender) {
  //       return PlayerPosition.defender;
  //     }
  //     if (position === PlayerPosition.goalKeeper) {
  //       return PlayerPosition.goalKeeper;
  //     }
  //     return PlayerPosition.none;
  //   };

  //   setSelectedPlayerId(
  //     allPlayersList.filter((p) => p.playerName === e.target.value)[0]?.playerId
  //   );
  //   setSelectedPlayerPosition(
  //     getPlayerPosition(
  //       allPlayersList.filter((p) => p.playerName === e.target.value)[0]
  //         ?.position
  //     )
  //   );
  //   setSelectedPlayerUsingDropdown(true);
  // };

  // const menuOptionsControl = useAnimation();

  // useEffect(() => {
  //   menuOptionsControl.start({
  //     scale: [0.1, 1],
  //     transition: {
  //       type: "spring",
  //       stiffness: 100,
  //       damping: 10,
  //     },
  //   });
  // }, []);

  return (
    <>
      {/* <motion.div className="menu-options" animate={menuOptionsControl}>
        <div className="select-countries">
          <label>{"Select Country / Club"}</label>
          <select
            name="countries"
            value={selectedCountryName}
            onChange={handleCountryChange}
          >
            {countries.sort(compareCountries).map((c) => (
              <option key={c.id} value={c.name}>
                {c.name}
              </option>
            ))}
          </select>
        </div>
        <div className="select-season">
          <label>{"Select Season"}</label>
          <select
            name="seasons"
            value={selectedSeasonForPlayers}
            onChange={handleSeasonChange}
          >
            {seasons.map((s) => (
              <option key={s} value={s}>
                {s}
              </option>
            ))}
          </select>
        </div>
        <div className="select-soccer-players">
          <label>{"Select Players"}</label>
          <select
            name="players"
            value={selectedPlayerName != null ? selectedPlayerName : ""}
            onChange={handlePlayerChange}
          >
            {allPlayersList.sort(comparePlayers).map((p) => (
              <option
                key={p.playerId}
                value={p.playerName != null ? p.playerName : ""}
              >
                {p.playerName}
              </option>
            ))}
          </select>
        </div>
      </motion.div> */}
    </>
  );
};
