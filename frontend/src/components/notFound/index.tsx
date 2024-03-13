import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "./notFound.scss";
import authStore from "stores/authStore";

const NotFound = () => {
	const [goto, setGoto] = useState("/");

	useEffect(() => {
		if (authStore.isAuth) {
			setGoto("/projects");
		}
	}, []);

	return (
		<div className="flex items-center justify-center h-screen bg-gray-100">
			<div className="text-center">
				<h1 className="text-6xl font-bold text-gray-800 animate-bounce">404</h1>
				<p className="text-gray-600">Oops! Page not found.</p>
				<Link className="btn btn-primary mt-4" to={`${goto}`}>
					Home
				</Link>
			</div>
		</div>
	);
};

export default NotFound;
