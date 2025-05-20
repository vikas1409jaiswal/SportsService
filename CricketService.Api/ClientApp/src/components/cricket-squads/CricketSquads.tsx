import React, { useState } from "react";
import { useCustomSquadInfo } from "./useCricketSquad";
import { CountryContent } from "../cricket-players/cricket-body/PlayerInfo/CountryContent";
import { MovingTrain } from "../common/MovingTrain";
import { SquadPlayerCard } from "./components/SquadPlayerCard";
import $ from "jquery";
import { motion } from "framer-motion";

import "./CricketSquads.scss";
import { PromotionFooter } from "./components/PromotionFooter";

interface CricketSquadsProps {
  secondaryTeam?: string;
}

const urls = [
  "https://www.espncricinfo.com/series/icc-champions-trophy-2024-25-1459031/bangladesh-squad-1468695/series-squads",
];

export const CricketSquads: React.FC<CricketSquadsProps> = ({
  secondaryTeam,
}) => {
  const [selectedUrlIndex, setSelectedUrlIndex] = useState(0);
  const squadInfo = useCustomSquadInfo(urls[selectedUrlIndex]);
  const [enableAnimation, toggleAnimation] = useState(false);

  console.log(squadInfo);

  $(document).on({
    keydown: (event) => {
      if (event.originalEvent?.key === "a") {
        toggleAnimation(!enableAnimation);
      }
      if (event.originalEvent?.key === "ArrowRight" && selectedUrlIndex < 10) {
        setSelectedUrlIndex(selectedUrlIndex + 1);
        event.preventDefault();
      }
      if (event.originalEvent?.key === "ArrowLeft" && selectedUrlIndex > 0) {
        setSelectedUrlIndex(selectedUrlIndex - 1);
        event.preventDefault();
      }
    },
  });

  const squadCards = squadInfo.players
    .slice(0)
    .filter((x) => !x.isWithdrawn)
    .map((p, i) => (
      <SquadPlayerCard
        player={p}
        index={i}
        teamName={squadInfo?.teamName}
        selectedUrlIndex={selectedUrlIndex}
      />
    ));

  return (
    <>
      {!enableAnimation && (
        <div className="squad-players-grid-container">
          {squadInfo.players.map((x) => (
            <div>
              <img
                src={`http://localhost:3013/images/${squadInfo?.teamName.replaceAll(
                  " ",
                  "-"
                )}/${x.href?.split("/")[2]}.png`}
                height={200}
                width={150}
                alt={x.name}
              />
              <h6>{x.name}</h6>
              <h6>{x.href?.split("/")[2]}</h6>
            </div>
          ))}
        </div>
      )}
      {enableAnimation && (
        <div
          className="squad-players"
          style={{
            height: 880,
            background: 'url("https://wallpapercave.com/wp/wp3049846.jpg")',
            backgroundSize: "100% 100%",
          }}
        >
          <header className="team-squad-header">
            {`ICC Champions Trophy 2025 - ${squadInfo?.teamName
              ?.toUpperCase()
              ?.replaceAll("-", " ")}`}
          </header>
          {selectedUrlIndex < 10 && (
            <div className="squad-players-list">
              <MovingTrain
                bogies={squadCards}
                trackLength={(12230 * 15) / 15 + 200}
                duration={8.8 * 15}
                delay={10}
              />
            </div>
          )}
          <PromotionFooter />
          <motion.div
            initial={{ y: 50, opacity: 0 }}
            animate={{ y: 0, opacity: 1 }}
            transition={{ duration: 1, delay: 5 }}
          >
            <CountryContent
              countryName={squadInfo?.teamName}
              hideName
              rotationSpeed={5}
            />
          </motion.div>
          {secondaryTeam && (
            <CountryContent
              countryName={secondaryTeam}
              hideName
              className="secondary-country"
            />
          )}
        </div>
      )}
    </>
  );
};
