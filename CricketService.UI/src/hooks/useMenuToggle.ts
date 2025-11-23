import { useState, useEffect } from "react";
import $ from "jquery";

export const useMenuToggle = () => {
  const [isMenuOpened, setIsMenuOpened] = useState(true);

  useEffect(() => {
    const handler = (event: any) => {
      if (
        event.originalEvent?.key === "m" || event.originalEvent?.key === "M"
      ) {
        setIsMenuOpened((prev: boolean) => !prev);
      }
    };
    $(document).on("keydown", handler);
    return () => {
      $(document).off("keydown", handler);
    };
  }, []);

  return {
    isMenuOpened,
    setIsMenuOpened
  };
};