import React, { FC, MouseEventHandler, useState } from "react";
import { Outlet } from "react-router-dom";
import MainHeader from "components/headers/mainHeader";
import ProjectSidebar from "components/projectSidebar";
import { SIDEBAR_WIDTH } from "./constants";

interface LayoutProps {
	showHeader?: boolean;
	showMenu?: boolean;
}

const Layout: FC<LayoutProps> = ({ showHeader = true, showMenu = true }) => {
	const [isSidebarOpen, setIsSidebarOpen] = useState<boolean>(true);

	const toggleSidebar: MouseEventHandler<HTMLButtonElement> = () => {
		setIsSidebarOpen(!isSidebarOpen);
	};

	return (
		<React.Fragment>
			{showHeader && <MainHeader />}
			{/* {showMenu && <SideMenu isOpen={isSidebarOpen} />} */}
			<main style={showMenu && isSidebarOpen ? { marginLeft: SIDEBAR_WIDTH } : {}}>
				{showMenu && (
					<button onClick={toggleSidebar}>
						{isSidebarOpen ? <i className="fa-light fa-sidebar"></i> : <i className="fa-light fa-sidebar"></i>}
					</button>
				)}
				<div className="content-wrapper">
					<Outlet />
				</div>
			</main>
		</React.Fragment>
	);
};

export default Layout;
