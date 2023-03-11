import React, { useState } from "react";

import "./ExpandableSection.scss";

interface ExpandableSectionProps {
  title: string;
  children: any;
  isExpanded: boolean;
}

export const ExpandableSection: React.FunctionComponent<
  ExpandableSectionProps
> = ({ title, children, isExpanded }) => {
  const [isOpen, setIsOpen] = useState(isExpanded);

  const toggleOpen = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div className="expandable-section">
      <div className="expandable-section-header" onClick={toggleOpen}>
        <h3>{title}</h3>
      </div>
      {isOpen && <div className="expandable-section-content">{children}</div>}
    </div>
  );
};
