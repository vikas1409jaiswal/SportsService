import React from "react";
import { ESPNTableRow } from "../../hook/useCustomESPNTable";
import { MovingTrain } from "../../../common/MovingTrain";
import { BogieCard } from "./BogieCard";
import { PlayerBirthDayCard } from "../../common/PlayerBirthDayCard";

interface MovingTrainRecordProps {
  rows: ESPNTableRow[];
}

export const MovingTrainRecord: React.FC<MovingTrainRecordProps> = ({
  rows,
}) => {
  const bogieCards = rows
    .slice(0)
    .map((p, i) => (
      <BogieCard
        leftColumn={
          <PlayerBirthDayCard
            playerHref={
              p.data.find((x) => x.key === "Player Href")?.value || ""
            }
            dateOfBirth={
              p.data.find((x) => x.key === "Date Of Birth")?.value || ""
            }
            index={i}
          />
        }
        width={480}
      />
    ));

  return (
    <MovingTrain
      bogies={bogieCards}
      trackLength={13000}
      duration={165}
      delay={5}
    />
  );
};
