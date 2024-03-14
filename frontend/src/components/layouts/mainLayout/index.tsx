import React, { FC, MouseEventHandler, useEffect, useState } from "react";
import { Outlet } from "react-router-dom";
import MainHeader from "components/headers/mainHeader";
import SideMenu from "components/sideMenu";
import { observer } from "mobx-react-lite";
import AuthStore from "../../../stores/authStore";

interface MainLayoutProps {
	showHeader?: boolean;
	showMenu?: boolean;
}

const MainLayout: FC<MainLayoutProps> = observer(({ showHeader = true, showMenu = true }) => {
	const [isOpen, setIsOpen] = useState(true);

	const toggleSidebar = () => {
		setIsOpen(!isOpen);
	};

	useEffect(() => {
		console.log(AuthStore.isAuth);
		if (!AuthStore.isAuth) {
			window.location.href = "/login";
		}
	}, []);

	return (
		<React.Fragment>
			<div className="flex flex-col h-full">
				{showHeader && <MainHeader />}
				<div className="flex flex-row h-full">
					{showMenu && (
						<div className={`${isOpen ? "w-[250px]" : "w-[15px]"} duration-200`}>
							<SideMenu isOpen={isOpen} toggleSidebar={toggleSidebar} />
						</div>
					)}
					<main className="w-full">
						<Outlet />
					</main>
				</div>
			</div>
		</React.Fragment>
	);
});

export default MainLayout;
