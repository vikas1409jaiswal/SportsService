import React from "react";
import { RotatingCylinder } from "../../../common/RotatingCylinder";
import logos from "./../../../../data/StaticData/teamLogos.json";

import "./CountryContent.scss";
import { config } from "../../../../configs";

interface CountryContentProps {
  countryName: string;
  hideName?: boolean;
  className?: string;
  height?: number;
  width?: number;
  translateZ?: number;
  rotationSpeed?: number;
}

export const CountryContent: React.FC<CountryContentProps> = ({
  countryName,
  hideName,
  className,
  height,
  width,
  translateZ,
  rotationSpeed,
}) => {
  const image = `http://localhost:3013/images-team-logos/${countryName?.replaceAll(
    " ",
    "-"
  )}.png`;

  return (
    <div className={`country-container ${className || ""}`}>
      <div className="country-logo" style={{ background: "none" }}>
        <RotatingCylinder
          images={[image, image, image, image, image]}
          width={width || 120}
          height={height || 120}
          rotationSpeed={config.isAnimation ? rotationSpeed || 10 : 0}
          translateZ={translateZ || 80}
        />
      </div>
      {!hideName && (
        <div className="country-name text-3d">{countryName?.toUpperCase()}</div>
      )}
    </div>
  );
};
