import { FC, ReactNode } from "react";

interface StatisticsContainerProps {
	name: string;
	children: ReactNode;
}

const StatisticsContainer: FC<StatisticsContainerProps> = ({ name, children }) => {
	return (
		<div className="flex flex-col container justify-center items-center border border-gray-200 rounded-md w-full h-full shadow-lg drop-shadow-lg">
			<div className="w-full h-10 border-b border-gray-100 px-3 py-3 font-medium">{name}</div>
			<div className="w-full h-full flex justify-center items-center">{children}</div>
		</div>
	);
};

export default StatisticsContainer;
