import { FC } from "react";

interface TimeProps {
	time: number;
}

const Time: FC<TimeProps> = ({ time }) => {
	const formatTime = (totalSeconds: number): string => {
		const hours = Math.floor(totalSeconds / 3600);
		const minutes = Math.floor((totalSeconds % 3600) / 60);
		const seconds = totalSeconds % 60;

		const formattedHours = String(hours).padStart(1, "0");
		const formattedMinutes = String(minutes).padStart(2, "0");
		const formattedSeconds = String(seconds).padStart(2, "0");

		return `${formattedHours}:${formattedMinutes}:${formattedSeconds}`;
	};
	return <div>{formatTime(time)}</div>;
};

export default Time;
