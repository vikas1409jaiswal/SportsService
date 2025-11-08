import React from "react";
import ReactCountryFlag from "react-country-flag";

type CountryCodeMap = {
  name: string;
  threeCode: string;
  twoCode: string;
};

interface CountryFlagProps {
  countryName?: string;
  countryCode?: string;
  height?: number;
  width?: number;
}

export const CountryFlag: React.FC<CountryFlagProps> = ({
  countryName,
  countryCode,
  height,
  width,
}) => {
  const countryCodes: CountryCodeMap[] = [
    {
      name: "India",
      threeCode: "IND",
      twoCode: "IN",
    },
    {
      name: "Pakistan",
      threeCode: "PAK",
      twoCode: "PK",
    },
    {
      name: "Sri Lanka",
      threeCode: "SL",
      twoCode: "LK",
    },
    {
      name: "Netherlands",
      threeCode: "NED",
      twoCode: "NL",
    },
    {
      name: "Scotland",
      threeCode: "SCO",
      twoCode: "GB-SCT",
    },
    {
      name: "Bangladesh",
      threeCode: "BAN",
      twoCode: "BD",
    },
    {
      name: "Afghanistan",
      threeCode: "AFG",
      twoCode: "AF",
    },
    {
      name: "Oman",
      threeCode: "OMA",
      twoCode: "OM",
    },
    {
      name: "Nepal",
      threeCode: "NEP",
      twoCode: "NP",
    },
    {
      name: "Zimbabwe",
      threeCode: "ZIM",
      twoCode: "ZW",
    },
    {
      name: "United Arab Emirates",
      threeCode: "UAE",
      twoCode: "AE",
    },
    {
      name: "Ireland",
      threeCode: "IRE",
      twoCode: "IE",
    },
    {
      name: "United States of America",
      threeCode: "USA",
      twoCode: "US",
    },
    {
      name: "New Zealand",
      threeCode: "NZ",
      twoCode: "NZ",
    },
    {
      name: "Namibia",
      threeCode: "NAM",
      twoCode: "NA",
    },
    {
      name: "South Africa",
      threeCode: "SA",
      twoCode: "ZA",
    },
    {
      name: "Papua New Guinea",
      threeCode: "PNG",
      twoCode: "PG",
    },
    {
      name: "Canada",
      threeCode: "CAN",
      twoCode: "CA",
    },
    {
      name: "Australia",
      threeCode: "AUS",
      twoCode: "AU",
    },
    {
      name: "England",
      threeCode: "ENG",
      twoCode: "GB-ENG",
    },
  ];

  return (
    <>
      {!countryName?.includes("West Indies") ? (
        <ReactCountryFlag
          countryCode={
            countryCodes.find(
              (x) =>
                (countryName && x.name === countryName) ||
                (countryCode && x.threeCode === countryCode)
            )?.twoCode || "US"
          }
          svg
          style={{
            width: width || "90%",
            height: height || "100%",
          }}
          title={countryName}
        />
      ) : (
        <img
          alt="WI"
          src="https://img.cricketworld.com/images/f-125442/cwi.jpg"
          width={width || "90%"}
          height={height || "80%"}
          style={{ marginTop: 10 }}
        />
      )}
    </>
  );
};
