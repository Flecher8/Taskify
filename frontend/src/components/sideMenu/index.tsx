import { Link } from "react-router-dom";
import React, { MouseEventHandler, useEffect, useState } from "react";
import "./sideMenu.scss";
import { SIDEBAR_WIDTH } from "../../constants";

interface SideMenuProps {
	isOpen: boolean;
	toggleSidebar: () => void;
}

// TODO WHY icon does not loads after F5??? --- <i className="fa-light fa-bars"></i>

const SideMenu: React.FC<SideMenuProps> = ({ isOpen, toggleSidebar }) => {
	return (
		<div className={`flex h-full relative bg-slate-300`}>
			<div
				className="flex absolute cursor-pointer rounded-full justify-center items-center -right-2 w-5 h-5 top-5 bg-blue-200"
				onClick={toggleSidebar}>
				{isOpen ? <i className="fa-light fa-angle-left"></i> : <i className="fa-light fa-angle-right"></i>}
			</div>
			<div className={`${!isOpen && "hidden"} origin-left duration-200`}>
				<div>123</div>
			</div>
		</div>
	);
};

export default SideMenu;
