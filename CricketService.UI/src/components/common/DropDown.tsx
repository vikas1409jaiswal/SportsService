import React, { useState } from "react";
import {
  Dropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
} from "reactstrap";

export interface DropDownProps {
  dropDownText: string;
  dropownOptions: string[];
  selectedValue: string;
  onSelect: (selectedOption: string) => void;
}

export const DropDown: React.FunctionComponent<DropDownProps> = ({
  dropDownText,
  dropownOptions,
  selectedValue,
  onSelect,
}) => {
  const [isOpen, setIsOpen] = useState(false);

  const toggle = () => {
    setIsOpen(!isOpen);
  };

  const handleOptionSelect = (selectedOption: string) => {
    onSelect(selectedOption);
    toggle();
  };

  return (
    <Dropdown isOpen={isOpen} toggle={toggle}>
      <DropdownToggle
        caret
      >{`${dropDownText}: ${selectedValue}`}</DropdownToggle>
      <DropdownMenu>
        {dropownOptions.map((ddo) => (
          <DropdownItem key={ddo} onClick={() => handleOptionSelect(ddo)}>
            {ddo}
          </DropdownItem>
        ))}
      </DropdownMenu>
    </Dropdown>
  );
};
