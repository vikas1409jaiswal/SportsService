import React, { useEffect, useState } from "react";
import { MovingTrainRecord } from "./components/moving-train-record/MovingTrainRecord";
import playerBirthDays from "./../../data/StaticData/birthdays.json";
import { ESPNTableRow } from "./hook/useCustomESPNTable";
import $ from "jquery";

import "./CricketersBirthDays.scss";
import { getNameFromHref } from "../../utils/ReusableFuctions";
import { speakText } from "../common/SpeakText";

interface CricketersBirthDaysProps {}

export const CricketersBirthDays: React.FC<CricketersBirthDaysProps> = ({}) => {
  const rowData: ESPNTableRow[] = [];
  const [showTrain, setShowTrain] = useState(true);

  // useEffect(() => {
  //   speakText(
  //     "In this video we will see cricketers born in this week from 7th november to 13th November"
  //   );
  // }, [showTrain]);

  $(document).on({
    keydown: (event) => {
      if (event.originalEvent?.key === "a") {
        setShowTrain(!showTrain);
      }
    },
  });

  playerBirthDays
    .filter(
      (x, i) =>
        [21, 22, 23, 24, 25, 26, 27].includes(x.dateOfBirth.date) &&
        x.dateOfBirth.month === 11 &&
        x.dateOfBirth.year > 1970
    )
    .filter((x, i) => ![6, 9, 11, 16, 18, 19, 21, 28, 31, 36, 37]?.includes(i))
    .forEach((x) =>
      rowData.push({
        data: [
          { key: "Player Href", value: `${x.href}` },
          {
            key: "Date Of Birth",
            value: `${x.dateOfBirth.date} November ${x.dateOfBirth.year}`,
          },
        ],
      })
    );

  return showTrain ? (
    <div
      className="cricketers-birthdays-container"
      style={{
        height: 880,
        background: 'url("https://wallpapercave.com/wp/wp3049846.jpg")',
        backgroundSize: "100% 100%",
      }}
    >
      <MovingTrainRecord rows={rowData} />
    </div>
  ) : (
    <div className="cricketers-birthdays-container">
      {rowData.map((x, i) => (
        <a
          href={x.data
            .find((x) => x.key === "Player Href")
            ?.value?.split("/")[2]
            ?.replace("http://localhost:44440/", "")}
        >
          {getNameFromHref(
            x.data.find((x) => x.key === "Player Href")?.value || "",
            "eng",
            20
          )}
        </a>
      ))}
    </div>
  );
};
