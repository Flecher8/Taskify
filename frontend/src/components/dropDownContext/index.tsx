import React from "react";
import { FC, ReactNode } from "react";

interface DropDownContextProps {
	dropDownDirection?: string;
	openDropDownButtonContent: ReactNode;
	openDropDownButtonStyle: string;
	dropDownContentStyle: string;
	children: ReactNode;
}

const DropDownContext: FC<DropDownContextProps> = ({
	dropDownDirection: dropdownDirection = "dropdown-end",
	openDropDownButtonContent,
	openDropDownButtonStyle,
	dropDownContentStyle,
	children
}) => {
	return (
		<div className={`dropdown ${dropdownDirection}`}>
			<div tabIndex={0} role="button" className={openDropDownButtonStyle}>
				{openDropDownButtonContent}
			</div>
			<div tabIndex={0} className={`dropdown-content z-[1] p-2 shadow rounded-box ${dropDownContentStyle}`}>
				{React.cloneElement(children as React.ReactElement<any>)}
			</div>
		</div>
	);
};

export default DropDownContext;
