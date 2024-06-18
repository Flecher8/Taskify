import React, { FC, ReactNode, useRef, useState } from "react";

interface MyDropDownContextProps {
	openDropDownButtonContent: ReactNode;
	openDropDownButtonStyle: string;
	dropDownContentStyle: string;
	children: ReactNode;
}

const MyDropDownContext: FC<MyDropDownContextProps> = ({
	openDropDownButtonContent,
	openDropDownButtonStyle,
	dropDownContentStyle,
	children
}) => {
	const [isOpen, setIsOpen] = useState(false);
	const dropdownRef = useRef<HTMLDivElement>(null);

	const toggleDropdown = () => {
		setIsOpen(!isOpen);
	};

	const handleClickOutside = (event: MouseEvent) => {
		if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
			setIsOpen(false);
		}
	};

	React.useEffect(() => {
		document.addEventListener("mousedown", handleClickOutside);
		return () => {
			document.removeEventListener("mousedown", handleClickOutside);
		};
	}, []);

	return (
		<div className="relative" ref={dropdownRef}>
			<div tabIndex={0} role="button" className={openDropDownButtonStyle} onClick={toggleDropdown}>
				{openDropDownButtonContent}
			</div>
			{isOpen && (
				<div className={`absolute z-[1] p-2 shadow rounded-box ${dropDownContentStyle}`}>
					{React.cloneElement(children as React.ReactElement<any>)}
				</div>
			)}
		</div>
	);
};

export default MyDropDownContext;
