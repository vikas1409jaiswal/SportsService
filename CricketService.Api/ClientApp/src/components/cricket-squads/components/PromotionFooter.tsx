import React from "react";
import { FaShareAlt, FaThumbsUp } from "react-icons/fa";

import "./PromotionFooter.scss";

interface PromotionFooterProps {}

export const PromotionFooter: React.FC<PromotionFooterProps> = ({}) => {
  return (
    <div className="moving-text-footer">
      Like <FaThumbsUp color="blue" />, Share <FaShareAlt color="green" />, and
      Subscribe{" "}
      <img
        alt="subs-icon"
        src="https://cdn.pixabay.com/photo/2020/07/15/21/04/subscribe-5408999_1280.png"
        height={50}
        width={100}
      />
      <span> </span>to Data plus Animation to stay connected.
    </div>
  );
};
