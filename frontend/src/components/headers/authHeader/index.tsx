import React, { FC, MouseEventHandler } from "react";
import { Link } from "react-router-dom";
import "./authHeader.scss";

// interface HeaderProps {
//   isSidebarOpen?: boolean;
//   showMenu?: boolean;
// 	onToggleSidebar: MouseEventHandler<HTMLButtonElement>;
// }

const AuthHeader: FC = () => {
	return (
		<header className="bg-primary text-white header p-4">
			<div className="container mx-auto flex justify-between items-center">
				<h1 className="text-xl font-semibold">Taskify</h1>
				<nav>
					<ul className="flex space-x-4">
						<li>
							<Link to="/" className="hover:text-gray-300">
								Home
							</Link>
						</li>
						<li>
							<Link to="/about" className="hover:text-gray-300">
								About
							</Link>
						</li>
						<li>
							<Link to="/contact" className="hover:text-gray-300">
								Contact
							</Link>
						</li>
					</ul>
				</nav>
			</div>
		</header>
	);
};

export default AuthHeader;
