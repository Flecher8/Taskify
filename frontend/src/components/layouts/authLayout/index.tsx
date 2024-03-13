import AuthHeader from "components/headers/authHeader";
import React, { FC, useEffect } from "react";
import { Outlet } from "react-router-dom";
import authStore from "stores/authStore";

interface AuthLayoutProps {
	showLoginHeader?: boolean;
}

const AuthLayout: FC<AuthLayoutProps> = ({ showLoginHeader = true }) => {
	useEffect(() => {
		if (authStore.isAuth) {
			window.location.href = "projects";
		}
	}, []);
	return (
		<div className="authLayout">
			{showLoginHeader && (
				<AuthHeader buttonText={"Sign up"} buttonLink={"/signup"} textBeforeButton="Don't have an account?" />
			)}
			{!showLoginHeader && (
				<AuthHeader buttonText={"Login"} buttonLink={"/login"} textBeforeButton="Already have an account?" />
			)}
			<main className="">
				<Outlet />
			</main>
		</div>
	);
};

export default AuthLayout;
