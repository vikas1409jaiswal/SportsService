import React from "react";

// 3D animated intro for player1 vs player2
const usePlayersIntroJSX = (player1Name: string, player2Name: string, startAnimation: boolean): JSX.Element => {
  return (
    <div className="players-intro-3d">
      <span className={"player-name left" + (startAnimation ? " dash-animate" : "")}>{player1Name}</span>
      <span className={"vs-text" + (startAnimation ? " dash-animate-vs" : "")}>vs</span>
      <span className={"player-name right" + (startAnimation ? " dash-animate" : "")}>{player2Name}</span>
      <style>{`
        .players-intro-3d {
          display: flex;
          justify-content: center;
          align-items: center;
          font-size: 2.6rem;
          font-weight: bold;
          perspective: 600px;
          margin: 32px 0 24px 0;
        }
        .players-intro-3d .player-name {
          display: inline-block;
          padding: 0 28px;
          color: #fff;
          background: linear-gradient(90deg, #1976d2 60%, #00c6ff 100%);
          border-radius: 18px;
          box-shadow: 0 6px 32px rgba(25, 118, 210, 0.25);
          transform-style: preserve-3d;
          margin: 0 10px;
          letter-spacing: 2px;
          text-shadow: 0 2px 8px #1976d2, 0 0px 1px #fff;
        }
        .players-intro-3d .player-name.left.dash-animate {
          animation: dash3d 1.2s cubic-bezier(.68,-0.55,.27,1.55) both;
          animation-delay: 0.1s;
        }
        .players-intro-3d .player-name.right.dash-animate {
          animation: dash3d 1.2s cubic-bezier(.68,-0.55,.27,1.55) both;
          animation-delay: 0.3s;
        }
        .players-intro-3d .vs-text.dash-animate-vs {
          animation: dashVs 1.1s cubic-bezier(.68,-0.55,.27,1.55) both 0.2s;
        }
        .players-intro-3d .vs-text {
          color: #ff1744;
          font-size: 2.2rem;
          font-weight: 900;
          margin: 0 18px;
          letter-spacing: 3px;
          text-shadow: 0 2px 12px #ff1744, 0 0px 1px #fff;
        }
        @keyframes dash3d {
          0% {
            opacity: 0;
            transform: rotateY(90deg) scale(0.7);
            filter: blur(8px);
          }
          60% {
            opacity: 1;
            transform: rotateY(-10deg) scale(1.1);
            filter: blur(0px);
          }
          100% {
            opacity: 1;
            transform: rotateY(0deg) scale(1);
            filter: blur(0px);
          }
        }
        @keyframes dashVs {
          0% {
            opacity: 0;
            transform: scale(0.5) rotateX(90deg);
            filter: blur(8px);
          }
          60% {
            opacity: 1;
            transform: scale(1.2) rotateX(-10deg);
            filter: blur(0px);
          }
          100% {
            opacity: 1;
            transform: scale(1) rotateX(0deg);
            filter: blur(0px);
          }
        }
      `}</style>
    </div>
  );
};

export default usePlayersIntroJSX;
