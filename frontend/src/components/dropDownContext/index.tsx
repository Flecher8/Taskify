import React, { useEffect, useRef, useState } from "react";
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
	const [isOpen, setIsOpen] = useState(false);
	const dropdownRef = useRef<HTMLDivElement>(null);

	const handleToggleDropdown = () => {
		setIsOpen(!isOpen);
	};

	const handleClickOutside = (event: MouseEvent) => {
		if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
			setIsOpen(false);
		}
	};

	useEffect(() => {
		if (isOpen) {
			document.addEventListener("mousedown", handleClickOutside);
		} else {
			document.removeEventListener("mousedown", handleClickOutside);
		}

		// Cleanup event listener on component unmount
		return () => {
			document.removeEventListener("mousedown", handleClickOutside);
		};
	}, [isOpen]);

	return (
		<div ref={dropdownRef} className={`dropdown ${dropdownDirection}`}>
			<div tabIndex={0} role="button" className={openDropDownButtonStyle} onClick={handleToggleDropdown}>
				{openDropDownButtonContent}
			</div>
			{isOpen && (
				<div tabIndex={0} className={`dropdown-content z-[1] shadow ${dropDownContentStyle}`}>
					{React.cloneElement(children as React.ReactElement<any>)}
				</div>
			)}
		</div>
	);
};

export default DropDownContext;
