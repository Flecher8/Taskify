import React, { FC, MouseEventHandler, useState } from "react";
import { Outlet } from "react-router-dom";
import Header from "components/header";
import SideMenu from "components/sideMenu";

interface LayoutProps {
	showHeader?: boolean;
	showMenu?: boolean;
}

const Layout: FC<LayoutProps> = ({ showHeader = true, showMenu = true }) => {
	const [isSidebarOpen, setIsSidebarOpen] = useState<boolean>(false);

	const toggleSidebar: MouseEventHandler<HTMLButtonElement> = () => {
		setIsSidebarOpen(!isSidebarOpen);
	};

	return (
		<React.Fragment>
			{showHeader && <Header isSidebarOpen={isSidebarOpen} showMenu={showMenu} onToggleSidebar={toggleSidebar} />}
			{showMenu && <SideMenu isOpen={isSidebarOpen} />}
			<main style={showMenu && isSidebarOpen ? { marginLeft: "20%" } : {}}>
				<Outlet />
			</main>
		</React.Fragment>
	);
};

export default Layout;
