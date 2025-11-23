import React, { useState } from "react";
import { IndividualPages } from "./components/individual-pages-record/IndividualPages";
import { ReorderModal } from "./components/ReorderModal";
import { useCustomESPNTable } from "./hook/useCustomESPNTable";
import { useMenuToggle } from "../../hooks/useMenuToggle";
import { HamburgerMenu, MenuItem } from "../../components/common/HamburgerMenu";
import { profiles } from "./constants";
import { ProfileProvider } from "./ProfileContext";

import "./CricketRecords.scss";

export const CricketRecords: React.FC = () => {
  const [selectedProfile, setSelectedProfile] = useState(profiles[0].value);
  const [isReorderModalOpen, setIsReorderModalOpen] = useState(false);
  const [columnOrder, setColumnOrder] = useState<string[]>([]);
  const [removedItems, setRemovedItems] = useState<string[]>([]);
  const { isMenuOpened } = useMenuToggle();
  const originalRows = useCustomESPNTable(selectedProfile, 10);

  // Reorder rows data according to column order and filter out removed items
  const reorderRowData = (row: any) => {
    if (!row?.data) return row;
    
    let filteredData = row.data;
    
    // Filter out removed items
    if (removedItems.length > 0) {
      filteredData = filteredData.filter((item: { key: string }) => !removedItems.includes(item.key));
    }
    
    // Reorder according to column order
    if (columnOrder.length > 0) {
      const reorderedData = columnOrder
        .map(columnKey => filteredData.find((item: { key: string }) => item.key === columnKey))
        .filter(Boolean);
      return { ...row, data: reorderedData };
    }
    
    return { ...row, data: filteredData };
  };

  const rows = originalRows?.map(reorderRowData) || [];

  const handleSaveColumnOrder = (newOrder: string[]) => {
    console.log("Received new column order:", newOrder);
    setColumnOrder(newOrder);
  };

  const handleRemoveItems = (removedItemsList: string[]) => {
    console.log("Received removed items:", removedItemsList);
    setRemovedItems(removedItemsList);
  };

  const menuItemsProfiles: MenuItem[] = profiles.map(profile => ({
    label: profile.label,
    onClick: () => setSelectedProfile(profile.value),
    key: profile.value
  }));

  const menuItemsReorder: MenuItem[] = [
    {
      label: "Reorder Stats",
      onClick: () => setIsReorderModalOpen(true),
      key: "ReorderStats"
    }
  ];

  return (
    <ProfileProvider value={{ selectedProfile, setSelectedProfile }}>
      <div className="cricket-records-container">
        <div className="cricket-records-menu-grid">
          <HamburgerMenu
            menuItems={menuItemsReorder}
            isVisible={isMenuOpened}
            ariaLabel="Reorder Stats"
            className="reorder-menu-cricket-records"
            buttonContent="R"
          />
          <HamburgerMenu
            menuItems={menuItemsProfiles}
            isVisible={isMenuOpened}
            ariaLabel="Select cricket profile"
            className="profile-menu-cricket-records"
            buttonContent="P"
          />
        </div>
        <IndividualPages rows={rows?.slice(0, 10)} />
        <ReorderModal 
          isOpen={isReorderModalOpen}
          onClose={() => setIsReorderModalOpen(false)}
          rows={originalRows || []}
          onSave={handleSaveColumnOrder}
          onRemove={handleRemoveItems}
          currentOrder={columnOrder}
          removedItems={removedItems}
        />
      </div>
    </ProfileProvider>
  );
  //return <MovingTrainRecord rows={rows?.slice(0, 10)} />;
};
