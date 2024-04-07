import { Link, useParams } from "react-router-dom";
import React, { MouseEventHandler, useEffect, useState } from "react";
import "./sideMenu.scss";
import { SIDEBAR_WIDTH } from "../../constants";

interface SideMenuProps {
	isOpen: boolean;
	toggleSidebar: () => void;
}

// TODO WHY icon does not loads after F5??? --- <i className="fa-light fa-bars"></i>

const SideMenu: React.FC<SideMenuProps> = ({ isOpen, toggleSidebar }) => {
	const { projectId } = useParams<{ projectId: string }>();

	return (
		<div className={`flex h-full relative transition-all duration-300 ${isOpen ? "w-[250px]" : "w-[15px]"}`}>
			<div
				className="flex absolute bg-primary cursor-pointer rounded-full justify-center items-center -right-[12px] w-5 h-5 top-5"
				onClick={toggleSidebar}>
				{isOpen ? (
					<i className="fa-light fa-angle-left text-white"></i>
				) : (
					<i className="fa-light fa-angle-right text-white"></i>
				)}
			</div>
			<div className={`${!isOpen && "hidden"} w-full origin-left duration-200`}>
				<div>
					<Link to={`/project/${projectId}`}>Board</Link>
				</div>
				<div>
					<Link to={`/project/${projectId}/roles`}>Roles</Link>
				</div>
			</div>
		</div>
	);
};

export default SideMenu;
