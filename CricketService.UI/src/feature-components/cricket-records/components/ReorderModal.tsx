import React, { useMemo } from "react";
import { DragReorderModal, DragReorderItem } from "../../../components/common";

const NON_DRAGGABLE_STATS = ["Player Href", "Team Name", "Span"];

interface ReorderModalProps {
  isOpen: boolean;
  onClose: () => void;
  rows: any[];
  onSave?: (reorderedStats: string[]) => void;
  onRemove?: (removedStats: string[]) => void;
  currentOrder?: string[];
  removedItems?: string[];
}

export const ReorderModal: React.FC<ReorderModalProps> = ({ 
  isOpen, 
  onClose, 
  rows, 
  onSave, 
  onRemove,
  currentOrder,
  removedItems = []
}) => {
  const items: DragReorderItem[] = useMemo(() => {
    if (rows && rows.length > 0 && Array.isArray(rows[0].data)) {
      return rows[0].data.map((item: { key: string }) => ({
        key: item.key,
        label: item.key,
        isNonDraggable: NON_DRAGGABLE_STATS.includes(item.key)
      }));
    }
    return [];
  }, [rows]);

  return (
    <DragReorderModal
      isOpen={isOpen}
      onClose={onClose}
      title="Reorder Statistics"
      items={items}
      currentOrder={currentOrder}
      onSave={onSave}
      onRemove={onRemove}
      removedItems={removedItems}
      enabledBackgroundColor="#e6ffe6"
      disabledBackgroundColor="#f0f0f0"
      draggedBackgroundColor="#e3f2fd"
      showRemoveButton={true}
    />
  );
};