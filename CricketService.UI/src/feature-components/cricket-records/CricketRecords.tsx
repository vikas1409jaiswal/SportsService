import React, { useState } from "react";
import { IndividualPages } from "./components/individual-pages-record/IndividualPages";
import { useCustomESPNTable } from "./hook/useCustomESPNTable";
import { useMenuToggle } from "../../hooks/useMenuToggle";
import { HamburgerMenu, MenuItem } from "../../components/common/HamburgerMenu";

import "./CricketRecords.scss";


export const CricketRecords: React.FC = () => {
  // Profiles constant
  const profiles = [
    { label: "Most Wickets In Career", value: "MostWicketsInCareer" },
    { label: "Most Runs In Career", value: "MostRunsInCareer" }
  ];

  const [selectedProfile, setSelectedProfile] = useState(profiles[0].value);
  const { isMenuOpened } = useMenuToggle();
  const rows = useCustomESPNTable(selectedProfile, 10);

  const menuItems: MenuItem[] = profiles.map(profile => ({
    label: profile.label,
    onClick: () => setSelectedProfile(profile.value),
    key: profile.value
  }));

  console.log("rows", rows.map(r => r.data.find(d => d.key === "Player Href")?.value));

  return (
    <div className="cricket-records-container">
        <HamburgerMenu
          menuItems={menuItems}
          isVisible={isMenuOpened}
          ariaLabel="Select cricket profile"
          className="three-dot-menu"
        />
      <IndividualPages rows={rows?.slice(0, 10)} />
    </div>
  );
  //return <MovingTrainRecord rows={rows?.slice(0, 10)} />;
};
