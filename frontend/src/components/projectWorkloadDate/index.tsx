import { FC } from "react";
import { getFormattedDate } from "utilities/dateFormatted";

interface ProjectWorkloadDateProps {
	date: Date;
	showMonth?: boolean;
	selected?: boolean;
}

const ProjectWorkloadDate: FC<ProjectWorkloadDateProps> = ({ date, showMonth, selected }) => {
	const { month, dayOfWeek, dayOfMonth } = getFormattedDate(date);
	const textColorClass = selected ? "text-indigo-500 font-bold" : "text-gray-500";

	return (
		<div className="flex flex-col h-full w-full p-1 text-sm">
			{showMonth ? (
				<div className={`flex h-5 ${textColorClass}`}>{month}</div>
			) : (
				<div className="h-5"></div> // Empty div for spacing
			)}
			<div className="flex flex-row justify-between">
				<div className={`hidden lg:block ${textColorClass}`}>{dayOfWeek}</div>
				<div className={`text-center  ${textColorClass}`}>{dayOfMonth}</div>
			</div>
		</div>
	);
};

export default ProjectWorkloadDate;
