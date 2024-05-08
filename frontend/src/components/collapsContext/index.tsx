import React from "react";
import { FC, ReactNode } from "react";

interface CollapsContextProps {
	divStyle?: string;
	titleStyle?: string;
	contextStyle?: string;
	title: ReactNode;
	children: ReactNode;
}

const CollapsContext: FC<CollapsContextProps> = ({
	divStyle = "",
	titleStyle = "",
	contextStyle = "",
	title,
	children
}) => {
	return (
		<div className={`collapse ${divStyle}`}>
			<input type="checkbox" />
			<div className={`collapse-title ${titleStyle}`}>{title}</div>
			<div className={`collapse-content ${contextStyle}`}>{children}</div>
		</div>
	);
};

export default CollapsContext;
