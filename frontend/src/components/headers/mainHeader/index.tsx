import React, { FC, MouseEventHandler, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./mainHeader.scss";
import DropDownContext from "components/dropDownContext";
import NotificationsDashboard from "components/notificationsDashboard";
import UserSettingsMenu from "components/userSettingsMenu";
import { User } from "entities/user";
import userStore from "stores/userStore";
import authStore from "stores/authStore";

const MainHeader: FC = () => {
	const [user, setUser] = useState<User | undefined>(undefined);

	useEffect(() => {
		const loadUserData = async () => {
			const userId = userStore.userId;
			if (userId) {
				const userData = await userStore.getUserById(userId);
				setUser(userData);
			}
		};
		loadUserData();
	}, []);
	return (
		<header className="bg-indigo-500 p-4">
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
							<DropDownContext
								dropDownDirection="dropdown-end"
								openDropDownButtonContent={<i className="fa-light fa-user"></i>}
								openDropDownButtonStyle="text-white"
								dropDownContentStyle="w-48 bg-white mt-5">
								<UserSettingsMenu user={user} />
							</DropDownContext>
						</li>
					</ul>
				</nav>
			</div>
		</header>
	);
};

export default MainHeader;
