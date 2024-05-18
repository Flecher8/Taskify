import { FC } from "react";

interface TimeSpentIndicatorProps {
	secondsSpent: number;
	normalWorkingHoursPerDay: number;
	isActive?: boolean;
}

const TimeSpentIndicator: FC<TimeSpentIndicatorProps> = ({
	secondsSpent,
	normalWorkingHoursPerDay,
	isActive = true
}) => {
	const totalSeconds = secondsSpent - normalWorkingHoursPerDay * 3600;
	const hoursOverCapacity = Math.floor(totalSeconds / 3600);
	const minutesOverCapacity = Math.floor((totalSeconds % 3600) / 60);

	const percentage = (secondsSpent / (normalWorkingHoursPerDay * 3600)) * 100;
	let bgColor = "bg-emerald-200";
	let barColor = "bg-emerald-400";

	if (!isActive && secondsSpent === 0) {
		bgColor = "bg-gray-200";
	} else if (percentage > 100) {
		bgColor = "bg-red-200";
		barColor = "bg-red-400";
	} else if (percentage >= 80) {
		bgColor = "bg-amber-200";
		barColor = "bg-amber-400";
	}

	return (
		<div className={`relative w-full h-full rounded-md ${bgColor} flex`}>
			<div className="w-full h-full flex flex-col rounded-md justify-end items-center text-sm text-white">
				<div
					style={{ height: `${Math.min(percentage, 100)}%` }}
					className={`w-full flex flex-end rounded-md ${barColor}`}>
					{totalSeconds > 0 && isActive && (
						<div className="flex items-center justify-center flex-col w-full h-full text-xs text-white hidden lg:flex">
							<div>
								<span className="font-bold">{hoursOverCapacity} </span>h{" "}
								<span className="font-bold">{minutesOverCapacity}</span> m
							</div>
							<div>over capacity</div>
						</div>
					)}
				</div>
			</div>
		</div>
	);
};

export default TimeSpentIndicator;
