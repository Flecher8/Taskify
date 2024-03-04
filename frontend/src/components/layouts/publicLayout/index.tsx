import PublicHeader from "components/headers/publicHeader";
import React, { FC } from "react";
import { Outlet } from "react-router-dom";


interface PublicLayoutProps {
	showHeader?: boolean;
}

const PublicLayout: FC<PublicLayoutProps> = ({ showHeader = true }) => {
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