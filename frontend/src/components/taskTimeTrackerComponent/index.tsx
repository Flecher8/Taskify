import Stopwatch from "components/stopwatch";
import { FC, useEffect, useState } from "react";
import authStore from "stores/authStore";
import taskTimeTrackersStore from "stores/taskTimeTrackersStore";
import { TaskTimeTracker, TaskTimeTrackerType } from "entities/taskTimeTracker";
import DropDownContext from "components/dropDownContext";
import { localToUTC } from "utilities/timeConverter";
import Time from "components/time";
import TaskTimeTrackerDropDownDashboard from "components/taskTimeTrackerDropDownDashboard";

interface TaskTimeTrackerComponentProps {
	customTaskId: string;
}

const TaskTimeTrackerComponent: FC<TaskTimeTrackerComponentProps> = ({ customTaskId }) => {
	const userId = authStore.userId;
	const [stopwatchTime, setStopwatchTime] = useState<number>(0);
	const [stopwatchIsRunning, setStopwatchIsRunning] = useState<boolean>(false);
	const [activeTimer, setActiveTimer] = useState<TaskTimeTracker | null>(null);
	const [numberOfSecondsSpentOnTask, setNumberOfSecondsSpentOnTask] = useState<number>(0);
	const [usersTimers, setUsersTimers] = useState<TaskTimeTracker[]>([]);

	useEffect(() => {
		if (userId === null) {
			authStore.logout();
			return;
		}
		loadData();
	}, []);

	const loadNumberOfSecondsSpentOnTask = async () => {
		try {
			const numberOfSeconds = await taskTimeTrackersStore.getNumberOfSecondsSpentOnTask(customTaskId);

			if (numberOfSeconds === undefined) {
				return;
			}
			setNumberOfSecondsSpentOnTask(numberOfSeconds);
		} catch (error) {
			console.error(error);
		}
	};

	const loadActiveTimer = async () => {
		try {
			if (userId === null) {
				authStore.logout();
				return;
			}
			const activeTimer = await taskTimeTrackersStore.getIsTimerActive(userId, customTaskId);

			if (activeTimer === undefined) {
				return;
			}

			setActiveTimer(activeTimer);

			if (activeTimer !== null) {
				const startDateTime = new Date(activeTimer.startDateTime);
				const currentTime = localToUTC(new Date());
				if (currentTime === null) {
					return;
				}
				const timeDifferenceInSeconds = Math.floor((currentTime.getTime() - startDateTime.getTime()) / 1000);
				setStopwatchTime(timeDifferenceInSeconds);
				setStopwatchIsRunning(true);
			} else {
				setStopwatchIsRunning(false);
			}
		} catch (error) {
			console.error(error);
		}
	};

	const loadUsersTimers = async () => {
		try {
			if (userId === null) {
				authStore.logout();
				return;
			}
			const newUsersTimers = await taskTimeTrackersStore.getTaskTimeTrackersByTask(customTaskId);

			if (newUsersTimers === undefined) return;

			setUsersTimers(newUsersTimers);
		} catch (error) {
			console.error(error);
		}
	};

	const switchStopwatch = async () => {
		if (stopwatchIsRunning) {
			await stopStopwatch();
		} else {
			await startStopwatch();
		}
	};

	const startStopwatch = async () => {
		try {
			if (userId === null) {
				authStore.logout();
				return;
			}
			taskTimeTrackersStore.startTimer(userId, customTaskId);
			setStopwatchIsRunning(true);
		} catch (error) {
			console.error(error);
		}
	};

	const stopStopwatch = async () => {
		try {
			if (userId === null) {
				authStore.logout();
				return;
			}

			setStopwatchIsRunning(false);
			setNumberOfSecondsSpentOnTask(prev => prev + stopwatchTime);
			setStopwatchTime(0);
			await taskTimeTrackersStore.stopTimer(userId, customTaskId);
			await loadData();
		} catch (error) {
			console.error(error);
		}
	};

	const loadData = async () => {
		try {
			await Promise.all([loadNumberOfSecondsSpentOnTask(), loadActiveTimer(), loadUsersTimers()]);
		} catch (error) {
			console.error(error);
		}
	};

	const createRangeTime = async (startTime: string, endTime: string, date: Date) => {
		// Concatenate date and start/end times
		const startDate = new Date(date);
		startDate.setHours(parseInt(startTime.split(":")[0]));
		startDate.setMinutes(parseInt(startTime.split(":")[1]));

		const endDate = new Date(date);
		endDate.setHours(parseInt(endTime.split(":")[0]));
		endDate.setMinutes(parseInt(endTime.split(":")[1]));

		// Convert to UTC
		const utcStartDate = new Date(startDate.toISOString());
		const utcEndDate = new Date(endDate.toISOString());

		try {
			if (userId === null) {
				authStore.logout();
				return;
			}

			await taskTimeTrackersStore.createTaskTimeTracker({
				customTaskId: customTaskId,
				userId: userId,
				startDateTime: utcStartDate,
				endDateTime: utcEndDate,
				description: "",
				trackerType: TaskTimeTrackerType.Range
			});

			loadData();
		} catch (error) {
			console.error(error);
		}
	};

	const deleteUserTimer = async (id: string) => {
		try {
			await taskTimeTrackersStore.deleteTaskTimeTracker(id);
			setUsersTimers(prevTimers => prevTimers.filter(timer => timer.id !== id));
			loadData();
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<div className="">
			<div className="flex flex-row justify-center items-center hover:bg-gray-300 transition duration-300 pr-5 pl-1">
				<div className="flex justify-center items-center mr-1 hover:cursor-pointer" onClick={switchStopwatch}>
					{stopwatchIsRunning ? (
						<i className="fa-light fa-circle-pause bg-red-400 text-white border-0 rounded-full w-full h-full"></i>
					) : (
						<i className="fa-light fa-circle-play bg-green-400 text-white border-0 rounded-full"></i>
					)}
				</div>
				<div>
					<DropDownContext
						dropDownDirection="dropdown-start"
						openDropDownButtonContent={
							<div>
								{stopwatchIsRunning ? (
									<Stopwatch time={stopwatchTime} setTime={setStopwatchTime} isRunning={stopwatchIsRunning} />
								) : (
									<Time time={numberOfSecondsSpentOnTask} />
								)}
							</div>
						}
						openDropDownButtonStyle=""
						dropDownContentStyle="w-[250px] bg-white rounded-md">
						<TaskTimeTrackerDropDownDashboard
							customTaskId={customTaskId}
							numberOfSecondsSpentOnTask={numberOfSecondsSpentOnTask}
							setNumberOfSecondsSpentOnTask={setNumberOfSecondsSpentOnTask}
							stopwatchTime={stopwatchTime}
							stopwatchIsRunning={stopwatchIsRunning}
							switchStopwatch={switchStopwatch}
							createRangeTime={createRangeTime}
							usersTimers={usersTimers}
							deleteUserTimer={deleteUserTimer}
						/>
					</DropDownContext>
				</div>
			</div>
		</div>
	);
};

export default TaskTimeTrackerComponent;
