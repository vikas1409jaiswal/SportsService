import React, { useEffect, useRef } from "react";
import { SquadPlayer } from "../useCricketSquad";
import { SpeechLanguage, speakText } from "../../common/SpeakText";
import RotatingCircle from "../../cricket-records/common/RotatingCircle";
import { useInView } from "react-intersection-observer";
import * as htmlToImage from "html-to-image";

interface SquadPlayerCardProps {
  player: SquadPlayer;
  index: number;
  teamName: string;
  selectedUrlIndex: number;
}

export const SquadPlayerCard: React.FC<SquadPlayerCardProps> = ({
  player,
  index,
  teamName,
}) => {
  const [ref, inView] = useInView({
    triggerOnce: true,
    threshold: 1,
  });

  useEffect(() => {
    index === 0 &&
      speakText(
        `${teamName} national cricket team squad for ICC Champions Trophy 2025`,
        SpeechLanguage.HindiIndian,
        false
      );
  }, []);

  useEffect(() => {
    inView &&
      speakText(
        `${player.name?.replace("(c)", "captain")}, ${
          player.role
        }, Age: ${player.age?.split(" ")[0]?.replace("y", "year")}, ${
          player.batting
        }, ${player.bowling}`,
        SpeechLanguage.HindiIndian,
        false
      );
  }, [inView, index]);

  // const cardRef = useRef<HTMLDivElement>(null);

  // const saveAsImage = (format: "png" | "jpeg") => {
  //   if (cardRef.current) {
  //     const options = {
  //       quality: 1, // Quality of the output image
  //     };

  //     if (format === "png") {
  //       htmlToImage
  //         .toPng(cardRef.current, options)
  //         .then((dataUrl) => {
  //           downloadImage(dataUrl, `${player.href}.png`);
  //         })
  //         .catch((error) => {
  //           console.error("Failed to capture the image:", error);
  //         });
  //     } else if (format === "jpeg") {
  //       htmlToImage
  //         .toJpeg(cardRef.current, { quality: 0.95 })
  //         .then((dataUrl) => {
  //           downloadImage(dataUrl, `${player.href.split("/")[2]}.jpg`);
  //         })
  //         .catch((error) => {
  //           console.error("Failed to capture the image:", error);
  //         });
  //     }
  //   }
  // };

  // const downloadImage = (dataUrl: string, fileName: string) => {
  //   const link = document.createElement("a");
  //   link.href = dataUrl;
  //   link.download = fileName;
  //   document.body.appendChild(link);
  //   link.click();
  //   document.body.removeChild(link);
  // };

  return (
    <div
      className="squad-player-card"
      ref={ref}
      //onClick={() => saveAsImage("jpeg")}
    >
      <img
        src={`http://localhost:3013/images/${teamName?.replaceAll(" ", "-")}/${
          player.href?.split("/")[2]
        }.png`}
        alt={player.name}
        height={650}
        width={500}
        style={{ maxWidth: 500 }}
      />
      <div className="player-card-detail">
        <div className="player-card-header">
          <p>{player.name}</p>
          {player.role && <h5>{player.role?.toUpperCase()}</h5>}
        </div>
        {player.age && (
          <div className="detail-row">
            <p className="info-label">Age</p>
            <p>{player.age}</p>
          </div>
        )}
        {player.batting && (
          <div className="detail-row">
            <p className="info-label">Batting</p>
            <p>{player.batting}</p>
          </div>
        )}
        {player.bowling && (
          <div className="detail-row">
            <p className="info-label">Bowling</p>
            <p>{player.bowling}</p>
          </div>
        )}
      </div>
      <RotatingCircle number={index + 1} />
    </div>
  );
};
