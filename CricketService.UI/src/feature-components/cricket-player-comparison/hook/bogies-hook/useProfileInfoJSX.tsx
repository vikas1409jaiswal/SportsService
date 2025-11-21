import { ArrowDataComparer } from "../../common/ArrowDataComparer";
import { CricketPlayerResponse } from "../useFetchPlayerAllMatches";
import { useMemo } from "react";
import playersRolesInfo from "../../../common/players-roles-info.json";

const useProfileInfoJSX = (
  player1Data: CricketPlayerResponse,
  player2Data: CricketPlayerResponse
): JSX.Element[] => {
  const { playingRole: playingRole1, battingStyle: battingStyle1, bowlingStyle: bowlingStyle1 } = useMemo(() => {
    const match = playersRolesInfo.find(
      (p) => p.playerHref && player1Data?.playerHref && p.playerHref === player1Data?.playerHref
    );
    return {
      playingRole: match?.playingRole || "",
      battingStyle: match?.battingStyle || "",
      bowlingStyle: match?.bowlingStyle || "",
    };
  }, [player1Data?.playerHref]);

  const { playingRole: playingRole2, battingStyle: battingStyle2, bowlingStyle: bowlingStyle2 } = useMemo(() => {
    const match = playersRolesInfo.find(
      (p) => p.playerHref && player2Data?.playerHref && p.playerHref === player2Data?.playerHref
    );
    return {
      playingRole: match?.playingRole || "",
      battingStyle: match?.battingStyle || "",
      bowlingStyle: match?.bowlingStyle || "",
    };
  }, [player2Data?.playerHref]);

  const profileInfoBogies = [
    <ArrowDataComparer
      className="data age-container"
      dataName="Age"
      data1Text={player1Data?.age}
      data2Text={player2Data?.age}
      speechText={`${player1Data?.fullName}, Age, ${player1Data?.age.split("y ")[0]} years, ${player2Data?.fullName}, Age ${player2Data?.age.split("y ")[0]} years`}
    />,
    <ArrowDataComparer
      className="data batting-style-container"
      dataName="Batting Style"
      data1Text={battingStyle1}
      data2Text={battingStyle2}
      speechText={`Batting Style: ${player1Data?.fullName}, ${battingStyle1}, ${player2Data?.fullName}, ${battingStyle2}`}
    />,
    <ArrowDataComparer
      className="data bowling-style-container"
      dataName="Bowling Style"
      data1Text={bowlingStyle1}
      data2Text={bowlingStyle2}
      speechText={`Bowling Style: ${player1Data?.fullName}, ${bowlingStyle1}, ${player2Data?.fullName}, ${bowlingStyle2}`}
    />,
    <ArrowDataComparer
      className="data playing-role-container"
      dataName="Playing Role"
      data1Text={playingRole1}
      data2Text={playingRole2}
      speechText={`Playing Role: ${player1Data?.fullName}, ${playingRole1}, ${player2Data?.fullName}, ${playingRole2}`}
    />,
  ];
  return profileInfoBogies;
};

export default useProfileInfoJSX;
