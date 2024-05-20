import React, { FC, MouseEventHandler, useEffect, useState } from "react";
import { Outlet } from "react-router-dom";
import MainHeader from "components/headers/mainHeader";
import { observer } from "mobx-react-lite";
import AuthStore from "../../../stores/authStore";
import CompanySidebar from "components/companySidebar";

interface CompanyLayoutProps {
	showHeader?: boolean;
	showMenu?: boolean;
}

const CompanyLayout: FC<CompanyLayoutProps> = observer(({ showHeader = true, showMenu = true }) => {
	const [isOpen, setIsOpen] = useState(true);

	const toggleSidebar = () => {
		setIsOpen(!isOpen);
	};
	// TODO Add check for subscription, add for company
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
				<div className="flex h-full overflow-hidden">
					{showMenu && <CompanySidebar isOpen={isOpen} toggleSidebar={toggleSidebar} />}
					<main className={`flex-grow ${showMenu ? "w-3/4" : "w-full"} h-full`}>
						<Outlet />
					</main>
				</div>
			</div>
		</React.Fragment>
	);
});

export default CompanyLayout;
