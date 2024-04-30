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
		<div
			className={`flex h-full relative transition-all duration-300 bg-indigo-500 border-t border-white ${
				isOpen ? "w-[250px]" : "w-[15px]"
			}`}>
			<div
				className="flex absolute bg-indigo-500 cursor-pointer rounded-full justify-center items-center border border-white border-solid -right-[12px] w-5 h-5 top-5"
				onClick={toggleSidebar}>
				{isOpen ? (
					<i className="fa-light fa-angle-left text-white"></i>
				) : (
					<i className="fa-light fa-angle-right text-white"></i>
				)}
			</div>
			<div
				className={`${
					!isOpen && "hidden"
				} w-full origin-left duration-200 text-white text-lg flex flex-col justify-between`}>
				<div className="m-5">
					<div>
						<Link to={`/project/${projectId}`} className="link">
							<div className="flex flex-row">
								<div className="mr-2 text-sm flex items-center">
									<i className="fa-solid fa-square-kanban"></i>
								</div>
								<div>
									<h3>Board</h3>
								</div>
							</div>
						</Link>
					</div>
					<div>
						<Link to={`/project/${projectId}/members`} className="link">
							<div className="flex flex-row">
								<div className="mr-2 text-sm flex items-center">
									<i className="fa-solid fa-user"></i>
								</div>

								<div>
									<h3>Members</h3>
								</div>
							</div>
						</Link>
					</div>
					<div>
						<Link to={`/project/${projectId}/roles`} className="link">
							<div className="flex flex-row">
								<div className="mr-2 text-sm flex items-center">
									<i className="fa-solid fa-tower-control"></i>
								</div>
								<div>
									<h3>Roles</h3>
								</div>
							</div>
						</Link>
					</div>
					<div>
						<Link to={`/project/${projectId}/statistics`} className="link">
							<div className="flex flex-row">
								<div className="mr-2 text-sm flex items-center">
									<i className="fa-solid fa-chart-simple"></i>
								</div>
								<div>
									<h3>Statistics</h3>
								</div>
							</div>
						</Link>
					</div>
					<div>
						<Link to={`/project/${projectId}/settings`} className="link">
							<div className="flex flex-row">
								<div className="mr-2 text-sm flex items-center">
									<i className="fa-solid fa-gear"></i>
								</div>
								<div>
									<h3>Settings</h3>
								</div>
							</div>
						</Link>
					</div>
				</div>
				<div className="">
					<Link to={`/subscription`}>
						<div className="flex flex-row justify-center items-center rounded-full m-3 px-2 bg-gradient-to-br from-purple-500 from-10% to-pink-500 to-70%">
							<div className="mr-2 text-sm flex items-center">
								<i className="fa-solid fa-stars"></i>
							</div>
							<div className="">
								<h3>Subscription</h3>
							</div>
						</div>
					</Link>
				</div>
			</div>
		</div>
	);
};

export default SideMenu;
