import { config } from "../configs";
import etohjson from "./../../src/data/StaticData/englishToHindi.json";

export const toCapitalCase = (inputString: string) => {
  let words = inputString?.split(" ");

  let capitalizedWords = words?.map((word) => {
    if (word.length > 0) {
      return word[0]?.toUpperCase() + word?.slice(1)?.toLowerCase();
    } else {
      return "";
    }
  });

  return capitalizedWords?.join(" ");
};

const playerArr: string[] = [];

export const getNameFromHref = (
  href: string,
  lang: string = config.language,
  splitLength?: number
) => {
  const nameStr = href?.split("/")[2];
  const nameArr = nameStr?.split("-");
  nameArr?.pop();
  const engName =
    nameArr?.length > 0
      ? nameArr
          .map((x, i, a) =>
            i !== a?.length - 1 && a.join(" ")?.length > (splitLength || 14)
              ? `${x[0]}.`
              : x
          )
          ?.join(" ")
      : "";

  const hindiName = (etohjson as any).players[nameArr?.join(" ")];

  console.log(etohjson);

  hindiName === undefined && playerArr.push(nameArr?.join(" "));
  console.log(playerArr);

  return (lang === "hindi" ? hindiName : engName?.toUpperCase()) as string;
};
