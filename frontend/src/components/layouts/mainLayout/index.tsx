import React, { FC } from "react";
import { Outlet } from "react-router-dom";
import MainHeader from "components/headers/mainHeader";

interface MainLayoutProps {
	showHeader?: boolean;
}

const MainLayout: FC<MainLayoutProps> = ({ showHeader = true }) => {
	return (
		<React.Fragment>
			{showHeader && <MainHeader />}
			<main>
				<Outlet />
			</main>
		</React.Fragment>
	);
};

export default MainLayout;