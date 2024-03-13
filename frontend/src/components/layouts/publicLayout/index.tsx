import PublicHeader from "components/headers/publicHeader";
import React, { FC, useEffect } from "react";
import { Outlet } from "react-router-dom";
import authStore from "stores/authStore";

interface PublicLayoutProps {
	showHeader?: boolean;
}

const PublicLayout: FC<PublicLayoutProps> = ({ showHeader = true }) => {
	useEffect(() => {
		if (authStore.isAuth) {
			window.location.href = "projects";
		}
	}, []);
	return (
		<React.Fragment>
			{showHeader && <PublicHeader />}
			<main>
				<Outlet />
			</main>
		</React.Fragment>
	);
};

export default PublicLayout;
