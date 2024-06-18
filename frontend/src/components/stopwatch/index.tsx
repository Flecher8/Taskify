import Time from "components/time";
import { Dispatch, FC, SetStateAction, useEffect } from "react";

interface StopwatchProps {
	time: number;
	setTime: Dispatch<SetStateAction<number>>;
	isRunning: boolean;
}

const Stopwatch: FC<StopwatchProps> = ({ time, setTime, isRunning }) => {
	useEffect(() => {
		let interval: NodeJS.Timeout;

		if (isRunning) {
			interval = setInterval(() => {
				setTime(prevTime => prevTime + 1);
			}, 1000);
		}

		return () => clearInterval(interval);
	}, [isRunning, setTime]);

	return (
		<div>
			<Time time={time} />
		</div>
	);
};

export default Stopwatch;
