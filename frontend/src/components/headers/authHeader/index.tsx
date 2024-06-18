import React, { FC, MouseEventHandler } from "react";
import { Link } from "react-router-dom";
import "./authHeader.scss";

interface AuthHeaderProps {
	buttonText: string;
    buttonLink: string;
    textBeforeButton: string;
}

const AuthHeader: FC<AuthHeaderProps>  = ({buttonText, buttonLink, textBeforeButton}) => {
	return (
        <header className="authHeader header p-4">
            <div className="container mx-auto flex justify-between items-center">
                <div className="flex items-center">
                    <h1 className="text-xl font-semibold">
                        <Link to="/" className="">
                            Taskify
                        </Link>
                    </h1>
                </div>
                <div className="signupbutton flex items-center">
                    <p className="mr-4 textNearButton text-sm"><label>{textBeforeButton}</label></p>
                    <Link to={buttonLink} className="linkButton btn btn-primary text-sm">
                        {buttonText}
                    </Link>
                </div>
            </div>
        </header>
    );
};

export default AuthHeader;
