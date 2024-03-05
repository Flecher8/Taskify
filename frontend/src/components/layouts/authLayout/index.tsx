import AuthHeader from "components/headers/authHeader";
import React, { FC } from "react";
import { Outlet } from "react-router-dom";


interface AuthLayoutProps {
	showLoginHeader?: boolean;
}

const AuthLayout: FC<AuthLayoutProps> = ({ showLoginHeader = true }) => {
	return (
		<div className="authLayout">
			{showLoginHeader && (
				<AuthHeader
				buttonText={"Sign up"}
				buttonLink={"/signup"}
				textBeforeButton="Don't have an account?"
				/>
			)}
			{!showLoginHeader && (
				<AuthHeader
				buttonText={"Login"}
				buttonLink={"/login"}
				textBeforeButton="Already have an account?"
				/>
			)}
			<main className="">
				<Outlet />
			</main>
    	</div>
	);
};

export default AuthLayout;