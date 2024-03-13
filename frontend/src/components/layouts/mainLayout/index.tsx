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
	const [isSidebarOpen, setIsSidebarOpen] = useState<boolean>(true);

	const toggleSidebar: MouseEventHandler<HTMLButtonElement> = () => {
		setIsSidebarOpen(!isSidebarOpen);
	};

	useEffect(() => {
		console.log(AuthStore.isAuth);
		if (!AuthStore.isAuth) {
			window.location.href = "/login";
		}
	}, []);

	return (
		<React.Fragment>
			{showHeader && <MainHeader />}
			{showMenu && <SideMenu isOpen={isSidebarOpen} />}
			<main>
				{showMenu && (
					<button onClick={toggleSidebar}>
						{isSidebarOpen ? <i className="fa-light fa-sidebar"></i> : <i className="fa-light fa-sidebar"></i>}
					</button>
				)}
				<Outlet />
			</main>
		</React.Fragment>
	);
});

export default MainLayout;
