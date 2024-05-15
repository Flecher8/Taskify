import { FC, ReactNode } from "react";

interface StatisticsContainerProps {
	children: ReactNode;
}

const StatisticsContainer: FC<StatisticsContainerProps> = ({ children }) => {
	return (
		<div className="flex container justify-center items-center border border-gray-200 rounded-md p-2 m-1">
			{children}
		</div>
	);
};

export default StatisticsContainer;
