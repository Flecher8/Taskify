import React, { FC, MouseEventHandler } from "react";
import { Link } from "react-router-dom";
import "./publicHeader.scss";

// interface HeaderProps {
//   isSidebarOpen?: boolean;
//   showMenu?: boolean;
// 	onToggleSidebar: MouseEventHandler<HTMLButtonElement>;
// }

const PublicHeader: FC = () => {
	return (
		<header className="text-black header p-4">
			<div className="container mx-auto flex justify-between items-center">
				<div className="flex items-center">
					<h1 className="text-xl font-semibold">
						<Link to="/" className="">
							Taskify
						</Link>
					</h1>
				</div>
				<nav>
					<ul className="flex space-x-12">
						<li>
							<Link to="/product" className="hover:text-gray-300">
								Product
							</Link>
						</li>
						<li>
							<Link to="/pricing" className="hover:text-gray-300">
								Pricing
							</Link>
						</li>
						<li>
							<Link to="/contacts" className="hover:text-gray-300">
								Contacts
							</Link>
						</li>
					</ul>
				</nav>
				<nav>
					<ul className="flex space-x-4">
						<li>
							<Link to="/login" className="hover:text-gray-300">
								Log In
							</Link>
						</li>
					</ul>
				</nav>
			</div>
		</header>
	);
};

export default PublicHeader;
