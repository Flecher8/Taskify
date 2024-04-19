import React, { FC, MouseEventHandler } from "react";
import { Link } from "react-router-dom";
import "./mainHeader.scss";
import DropDownContext from "components/dropDownContext";
import NotificationsDashboard from "components/notificationsDashboard";

const MainHeader: FC = () => {
	return (
		<header className="bg-primary p-4">
			<div className="container mx-auto flex justify-between items-center">
				<div className="flex items-center">
					<h3 className="text-xl font-semibold text-white">
						<Link to="/projects" className="">
							Taskify
						</Link>
					</h3>
				</div>
				<nav>
					<ul className="flex space-x-4">
						<li className="hover:bg-violet-900 transition duration-300 rounded-full flex justify-center w-6">
							<DropDownContext
								dropDownDirection="dropdown-end"
								openDropDownButtonContent={<i className="fa-light fa-bell"></i>}
								openDropDownButtonStyle="text-white"
								dropDownContentStyle="w-80 bg-white mt-5">
								<NotificationsDashboard />
							</DropDownContext>
						</li>
						<li>
							<Link to="/profile" className="hover:text-gray-300 text-white">
								<i className="fa-light fa-user"></i>
							</Link>
						</li>
					</ul>
				</nav>
			</div>
		</header>
	);
};

export default MainHeader;
