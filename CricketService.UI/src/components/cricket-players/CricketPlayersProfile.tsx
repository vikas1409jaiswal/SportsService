import { useEffect, useState } from "react";
import {
  PlayerShortInfo,
  TeamInfo,
  useAllPlayersUuids,
} from "../hooks/useAllPlayersUuids";
import "./CricketPlayersProfile.scss";
import { CricketHeader } from "./cricket-header/CricketHeader";
import React from "react";
import { GenericModifiers } from "./Types/SoccerTypes";
import $ from "jquery";
import {
  CricketTeamDetails,
  useTeamByUuid,
} from "../hooks/useCricketTeamByUuid";
import { PlayerPosition } from "../CricketHomePage";
import { CricketBody } from "./cricket-body/CricketBody";
import { FaThumbsUp, FaShareAlt, FaYoutube } from "react-icons/fa";

interface CricketProfileContextValue {
  countries: TeamInfo[];
  selectedCountryDetails: CricketTeamDetails;
  currentSelectedCountryIndex: number;
  selectedPlayerType: PlayerPosition;
  showAllPlayers: boolean;
  playersForShow: PlayerShortInfo[];
  showHeader: boolean;
  setCurrentSelectedCountryIndex: GenericModifiers<number>;
  setSelectedPlayerType: GenericModifiers<PlayerPosition>;
  setShowAllPlayers: GenericModifiers<boolean>;
}

const initialCricketProfileContextValue: CricketProfileContextValue = {
  countries: [],
  selectedCountryDetails: {
    teamUuid: "",
    teamName: "",
    flagUri: "",
  },
  currentSelectedCountryIndex: 0,
  selectedPlayerType: PlayerPosition.none,
  showAllPlayers: false,
  playersForShow: [],
  showHeader: false,
  setCurrentSelectedCountryIndex: () => {},
  setSelectedPlayerType: () => {},
  setShowAllPlayers: () => {},
};

interface CricketPlayersProfileProps {
  // showTeamsGroup: boolean;
  // showColorPicker: boolean;
  // backgroundColor: string;
  // selectedStyle: string;
  // selectedTeamGroupsKey: string[];
  // setBackgroundColor: StringModifiers;
  // setHeaderBgColor: StringModifiers;
  // setHeaderTitleBgColor: StringModifiers;
  // setHeaderFontColor: StringModifiers;
  // setDetailsBodyBgColor: StringModifiers;
  // setDetailsBodyFontColor: StringModifiers;
  // setDetailsHeaderBgColor: StringModifiers;
  // setDetailsHeaderFontColor: StringModifiers;
  // setFooterButtonDefaultColor: StringModifiers;
  // setFooterButtonSelectedColor: StringModifiers;
  // setShowColorPicker: BooleanToggler;
  // setShowTeamsGroup: BooleanToggler;
  // setSelectedStyle: StringModifiers;
  // setSelectedTeamGroupsKey: StringArrayModifiers;
}

export const CricketProfileContext =
  React.createContext<CricketProfileContextValue>(
    initialCricketProfileContextValue
  );

export const CricketPlayersProfile: React.FunctionComponent<
  CricketPlayersProfileProps
> = () => {
  const { playerInfos, isLoading } = useAllPlayersUuids();

  const [currentSelectedCountryIndex, setCurrentSelectedCountryIndex] =
    useState(0);
  const [selectedPlayerType, setSelectedPlayerType] = useState(
    PlayerPosition.none
  );

  //Boolean States
  const [showAllPlayers, setShowAllPlayers] = useState(false);
  const [showHeader, setShowHeader] = useState(false);

  const countries = isLoading
    ? []
    : (playerInfos
        ?.map((x) => x.teams)
        .flat()
        .filter(
          (x, i, arr) => arr.map((y) => y.uuid).indexOf(x.uuid) === i
        ) as TeamInfo[]);

  const { teamDetails } = useTeamByUuid(
    countries[currentSelectedCountryIndex]?.uuid
  );

  $(document).on({
    keydown: (event) => {
      if (
        event.originalEvent?.key === "ArrowUp" &&
        currentSelectedCountryIndex < countries?.length - 1
      ) {
        event.preventDefault();
        setCurrentSelectedCountryIndex(currentSelectedCountryIndex + 1);
      }
      if (
        event.originalEvent?.key === "ArrowDown" &&
        currentSelectedCountryIndex > 0
      ) {
        event.preventDefault();
        setCurrentSelectedCountryIndex(currentSelectedCountryIndex - 1);
      }
    },
  });

  const playerInOrder = [
    "Shakib Al Hasan",
    "Sohail Tanvir",
    "Mohammad Nabi",
    "Hayden Walsh",
    "Raymon Reifer",
    "Isuru Udana",
    "Fidel Edwards",
    "Dwayne Bravo",
    "David Wiese",
    "David Wiese",
  ].reverse();

  const getSelectedPlayers = () => {
    const selectedPlayers: PlayerShortInfo[] = [];
    playerInOrder?.forEach((x) => {
      if (playerInfos?.map((y) => y.playerName).includes(x)) {
        selectedPlayers.push(
          playerInfos.find((p) => p.playerName === x) as PlayerShortInfo
        );
      }
    });

    return selectedPlayers;
  };

  const playersForShow = showHeader
    ? (playerInfos
        ?.filter((x) =>
          x.teams.map((y) => y.uuid).includes(teamDetails?.teamUuid as string)
        )
        .filter((x) => [""].includes(x.playerName)) as PlayerShortInfo[])
    : getSelectedPlayers().reverse();

  return (
    <>
      <div
        className="cricket-players-home-page"
        style={{
          height: 860,
          background: 'url("https://wallpapercave.com/wp/wp3049846.jpg")',
          backgroundSize: "100% 100%",
        }}
      >
        <CricketProfileContext.Provider
          value={{
            countries,
            selectedCountryDetails: teamDetails as CricketTeamDetails,
            currentSelectedCountryIndex,
            selectedPlayerType,
            showAllPlayers,
            playersForShow,
            showHeader,
            setCurrentSelectedCountryIndex,
            setSelectedPlayerType,
            setShowAllPlayers,
          }}
        >
          <CricketBody />
          <div className="moving-text">
            Like <FaThumbsUp color="blue" />, Share <FaShareAlt color="green" />
            , and Subscribe{" "}
            <img
              alt="subs-icon"
              src="https://cdn.pixabay.com/photo/2020/07/15/21/04/subscribe-5408999_1280.png"
              height={50}
              width={100}
            />
            <span> </span>to Data plus Animation to stay connected.
          </div>
        </CricketProfileContext.Provider>
      </div>
    </>
  );
};
