import React, { useState, useRef, useEffect } from "react";
import "./HamburgerMenu.scss";

export interface MenuItem {
  label: string;
  onClick: () => void;
  key?: string;
}

interface HamburgerMenuProps {
  menuItems: MenuItem[];
  isVisible?: boolean;
  className?: string;
  ariaLabel?: string;
}

export const HamburgerMenu: React.FC<HamburgerMenuProps> = ({
  menuItems,
  isVisible = false,
  className = "",
  ariaLabel = "Open menu"
}) => {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);
  const menuButtonRef = useRef<HTMLButtonElement>(null);
  const dropdownMenuRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      const target = event.target as Node;
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(target) &&
        (!dropdownMenuRef.current || !dropdownMenuRef.current.contains(target))
      ) {
        setIsDropdownOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const handleMenuClick = () => {
    setIsDropdownOpen((prev) => !prev);
  };

  const handleMenuItemClick = (menuItem: MenuItem) => {
    setIsDropdownOpen(false);
    menuItem.onClick();
  };

  if (!isVisible) {
    return null;
  }

  return (
    <>
      <div className={`hamburger-menu ${className}`.trim()} ref={dropdownRef}>
        <button 
          ref={menuButtonRef} 
          className="hamburger-menu-button" 
          onClick={handleMenuClick} 
          aria-label={ariaLabel}
        >
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <circle cx="12" cy="5" r="2" fill="#333" />
            <circle cx="12" cy="12" r="2" fill="#333" />
            <circle cx="12" cy="19" r="2" fill="#333" />
          </svg>
        </button>
      </div>
      {isDropdownOpen && isVisible && (
        <div
          ref={dropdownMenuRef}
          className="hamburger-dropdown-menu"
        >
          {menuItems.map((item, index) => (
            <button
              key={item.key || index}
              className="hamburger-dropdown-item"
              onClick={() => handleMenuItemClick(item)}
            >
              {item.label}
            </button>
          ))}
        </div>
      )}
    </>
  );
};