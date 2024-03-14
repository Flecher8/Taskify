import React, { FC, MouseEventHandler } from "react";
import { Link } from "react-router-dom";
import "./mainHeader.scss";

// interface HeaderProps {
//   isSidebarOpen?: boolean;
//   showMenu?: boolean;
// 	onToggleSidebar: MouseEventHandler<HTMLButtonElement>;
// }

const MainHeader: FC = () => {
	return (
		<header className="bg-primary text-white p-4">
			<div className="container mx-auto flex justify-between items-center">
				<div className="flex items-center">
					<h3 className="text-xl font-semibold">
						<Link to="/projects" className="">
							Taskify
						</Link>
					</h3>
				</div>
				<nav>
					<ul className="flex space-x-4">
						<li>
							<Link to="/profile" className="hover:text-gray-300">
								Upgrade
							</Link>
						</li>
						<li>
							<Link to="/profile" className="hover:text-gray-300">
								Profile
							</Link>
						</li>
					</ul>
				</nav>
			</div>
		</header>
	);
};

export default MainHeader;
