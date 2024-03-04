import React, { FC } from "react";
import { Outlet } from "react-router-dom";
import MainHeader from "components/headers/mainHeader";

interface AuthLayoutProps {
	showHeader?: boolean;
}

const AuthLayout: FC<AuthLayoutProps> = ({ showHeader = true }) => {
	return (
		<React.Fragment>
			{showHeader && <MainHeader />}
			<main>
				<Outlet />
			</main>
		</React.Fragment>
	);
};

export default AuthLayout;