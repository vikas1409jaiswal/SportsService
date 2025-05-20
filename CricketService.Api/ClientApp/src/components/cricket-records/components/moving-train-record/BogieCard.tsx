import React, { ReactNode } from "react";

interface BogieCardProps {
  leftColumn: ReactNode;
  width?: number;
  rightColumn?: ReactNode;
  leftRightColumnRatio?: [number, number];
}

export const BogieCard: React.FC<BogieCardProps> = ({
  leftColumn,
  rightColumn,
  width,
  leftRightColumnRatio,
}) => {
  return (
    <div
      style={{
        width: width || 600,
        display: "grid",
        gridTemplateColumns: rightColumn
          ? leftRightColumnRatio
            ? `${leftRightColumnRatio[0]}fr ${leftRightColumnRatio[1]}fr`
            : "1fr 1fr"
          : "1fr",
      }}
    >
      {leftColumn}
      {rightColumn}
    </div>
  );
};
