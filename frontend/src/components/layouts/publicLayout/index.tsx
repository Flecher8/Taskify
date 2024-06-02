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
			<div className="flex flex-col h-full">
				{showHeader && <PublicHeader />}
				<div className="flex h-full w-full overflow-hidden">
					<main className="flex-grow w-full h-full">
						<Outlet />
					</main>
				</div>
			</div>
		</React.Fragment>
	);
};

export default PublicLayout;
