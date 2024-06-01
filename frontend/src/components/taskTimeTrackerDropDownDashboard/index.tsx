import CollapsContext from "components/collapsContext";
import DateTimePicker from "components/dateTimePicker";
import Time from "components/time";
import TimePicker from "components/timePicker";
import { TaskTimeTracker, TaskTimeTrackerType } from "entities/taskTimeTracker";
import { Dispatch, FC, SetStateAction, useEffect, useState } from "react";
import { IDay } from "react-calendar-datetime-picker/dist/types/type";
import { dateToIDay, iDayToDate } from "utilities/date_IDay_Converter";

interface TaskTimeTrackerDropDownDashboardProps {
	customTaskId: string;
	numberOfSecondsSpentOnTask: number;
	setNumberOfSecondsSpentOnTask: Dispatch<SetStateAction<number>>;
	stopwatchTime: number;
	stopwatchIsRunning: boolean;
	switchStopwatch: () => void;
	createRangeTime: (startTime: string, endTime: string, date: Date) => void;
	usersTimers: TaskTimeTracker[];
	deleteUserTimer: (id: string) => void;
}

const TaskTimeTrackerDropDownDashboard: FC<TaskTimeTrackerDropDownDashboardProps> = ({
	customTaskId,
	numberOfSecondsSpentOnTask,
	setNumberOfSecondsSpentOnTask,
	stopwatchTime,
	stopwatchIsRunning,
	switchStopwatch,
	createRangeTime,
	usersTimers,
	deleteUserTimer
}) => {
	const [timerType, setTimerType] = useState<TaskTimeTrackerType>(TaskTimeTrackerType.Stopwatch);

	const [startTime, setStartTime] = useState<string>("12:00");
	const [endTime, setEndTime] = useState<string>("12:00");
	const [date, setDate] = useState<Date>(new Date());

	// Variable to indicate if startTime is less than endTime
	const isStartTimeBeforeEndTime = () => {
		// Splitting start and end time into hours and minutes
		const [startHour, startMinute] = startTime.split(":");
		const [endHour, endMinute] = endTime.split(":");

		// Converting hours and minutes to numbers
		const startHourNum = parseInt(startHour);
		const startMinuteNum = parseInt(startMinute);
		const endHourNum = parseInt(endHour);
		const endMinuteNum = parseInt(endMinute);

		// Comparing the times
		if (startHourNum < endHourNum) {
			return true; // startTime is before endTime
		} else if (startHourNum === endHourNum) {
			// If hours are equal, compare minutes
			return startMinuteNum < endMinuteNum;
		} else {
			return false; // startTime is after endTime
		}
	};

	const isStartTimeBeforeEndTimeValue = isStartTimeBeforeEndTime();

	useEffect(() => {
		createStartTime();
	}, []);

	useEffect(() => {}, [customTaskId, numberOfSecondsSpentOnTask, stopwatchTime, usersTimers, stopwatchIsRunning]);

	const createStartTime = () => {
		// Get current time
		const currentTime = new Date();
		// Format current time to "HH:MM" format
		const formattedHours = String(currentTime.getHours()).padStart(2, "0");
		const formattedMinutes = String(currentTime.getMinutes()).padStart(2, "0");
		const formattedTime = `${formattedHours}:${formattedMinutes}`;
		// Set start and end time to current time
		setStartTime(formattedTime);
		setEndTime(formattedTime);
	};

	const handleChangeTimeType = async () => {
		if (timerType === TaskTimeTrackerType.Stopwatch) {
			setTimerType(TaskTimeTrackerType.Range);
		} else {
			setTimerType(TaskTimeTrackerType.Stopwatch);
		}
	};

	const handleStartTimeChange = (newStartTime: string) => {
		setStartTime(newStartTime);
		// Ensure start time is before end time
		if (newStartTime >= endTime) {
			setEndTime(newStartTime);
		}
	};

	const handleEndTimeChange = (newEndTime: string) => {
		setEndTime(newEndTime);
		// Ensure end time is after start time
		if (newEndTime <= startTime) {
			setStartTime(newEndTime);
		}
	};

	const onDateChange = async (newDate: IDay | null) => {
		const newDateTypeDate = iDayToDate(newDate);
		if (newDateTypeDate === null) return;

		setDate(newDateTypeDate);
	};

	const handleCreateRangeTime = async () => {
		try {
			createRangeTime(startTime, endTime, date);
		} catch (error) {
			console.error(error);
		}
	};

	const groupTimersByUser = usersTimers.reduce((acc, timer) => {
		const userName = `${timer.user.firstName} ${timer.user.lastName}`;
		if (!acc[userName]) {
			acc[userName] = [];
		}
		acc[userName].push(timer);
		return acc;
	}, {} as { [key: string]: TaskTimeTracker[] });

	const sortedGroupTimersByUser = Object.entries(groupTimersByUser)
		.map(([userName, userTimers]) => ({
			userName,
			userTimers: userTimers.sort((a, b) => {
				if (a.endDateTime === null && b.endDateTime === null) return 0;
				if (a.endDateTime === null) return 1;
				if (b.endDateTime === null) return -1;
				return new Date(a.endDateTime).getTime() - new Date(b.endDateTime).getTime();
			})
		}))
		.reduce((acc, { userName, userTimers }) => {
			acc[userName] = userTimers;
			return acc;
		}, {} as { [key: string]: TaskTimeTracker[] });

	const timeDiffInSeconds = (date1: Date, date2: Date) => {
		const diffInMilliseconds = Math.abs(new Date(date1).getTime() - new Date(date2).getTime());
		const diffInSeconds = Math.floor(diffInMilliseconds / 1000);
		return diffInSeconds;
	};

	return (
		<div className="flex flex-col">
			<div className="p-1 flex flex-row justify-between">
				<div>Time spent:</div>
				<div>
					<Time time={numberOfSecondsSpentOnTask} />
				</div>
			</div>
			<div className="flex flex-col flex-nowrap overflow-auto max-h-[200px] scrollable custom-scroll-xs">
				<div className="flex flex-col">
					{Object.entries(sortedGroupTimersByUser)
						.filter(([, userTimers]) => userTimers.some(timer => timer.endDateTime !== null))
						.map(([userName, userTimers]) => {
							const totalTimeForUser = userTimers.reduce((total, timer) => {
								if (timer.endDateTime !== null) {
									return total + timeDiffInSeconds(timer.endDateTime, timer.startDateTime);
								}
								return total;
							}, 0);

							return (
								<CollapsContext
									key={userName}
									title={
										<div className="flex flex-row justify-between items-center w-full p-2 h-full">
											<div>{userName}</div>
											<div>
												<Time time={totalTimeForUser} />
											</div>
										</div>
									}
									divStyle="border mb-1"
									titleStyle="p-0 items-center">
									{userTimers.map(timer => (
										<div key={timer.id} className="border-b p-1">
											{timer.endDateTime !== null ? (
												<div className="grid grid-cols-3 text-xs gap-x-5 auto-rows-max">
													<div className="mr-1 flex justify-center items-center">
														<Time time={timeDiffInSeconds(timer.endDateTime, timer.startDateTime)} />
													</div>
													<div className="mr-1 flex justify-center items-center">
														{new Date(timer.startDateTime).getFullYear()}/
														{(new Date(timer.startDateTime).getMonth() + 1).toString().padStart(2, "0")}/
														{new Date(timer.startDateTime).getDate().toString().padStart(2, "0")}
													</div>
													<div
														className="text-red-500 hover:cursor-pointer flex justify-end items-center"
														onClick={() => deleteUserTimer(timer.id)}>
														<i className="fa-light fa-trash-can"></i>
													</div>
												</div>
											) : null}
										</div>
									))}
								</CollapsContext>
							);
						})}
				</div>
			</div>
			<div>
				<div className="flex flex-col bg-indigo-500 text-white ">
					<div className="pt-3 grid grid-cols-2 gap-3 divide-x divide-white text-xs">
						<div
							className={`flex justify-center items-center hover:cursor-pointer ${
								timerType !== TaskTimeTrackerType.Stopwatch ? "opacity-50" : ""
							}`}
							onClick={handleChangeTimeType}>
							<div className="flex flex-row justify-center items-center">
								<div className="">
									<i className="fa-light fa-circle-play text-white pr-1"></i>
								</div>
								<div>Stopwatch</div>
							</div>
						</div>
						<div
							className={`flex justify-center items-center hover:cursor-pointer ${
								timerType !== TaskTimeTrackerType.Range ? "opacity-50" : ""
							}`}>
							<div className="flex flex-row justify-center items-center" onClick={handleChangeTimeType}>
								<div>
									<i className="fa-light fa-arrows-left-right text-white pr-1"></i>
								</div>
								<div>Range</div>
							</div>
						</div>
					</div>
					<div className="px-3 py-3">
						{timerType === TaskTimeTrackerType.Stopwatch ? (
							<div className="flex flex-row justify-center items-center">
								<div
									className="flexjustify-center items-center hover:cursor-pointer mr-1"
									onClick={switchStopwatch}>
									{stopwatchIsRunning ? (
										<i className="fa-light fa-pause bg-indigo-500 border-0 rounded-full"></i>
									) : (
										<i className="fa-light fa-play bg-indigo-500 border-0 rounded-full"></i>
									)}
								</div>
								<div>
									<Time time={stopwatchTime} />
								</div>
							</div>
						) : (
							<div>
								<div className="flex flex-row justify-center items-center">
									<div className="flex items-center bg-indigo-400 rounded-md px-5">
										<TimePicker time={startTime} onChange={handleStartTimeChange} />
									</div>
									<div className="flex items-center mx-2">-</div>
									<div className="flex items-center bg-indigo-400 rounded-md px-5">
										<TimePicker time={endTime} onChange={handleEndTimeChange} />
									</div>
								</div>
								<div className="flex flex-col justify-between m-1">
									<div className="flex flex-row">
										<div className="mr-1">When</div>
										<div className="flex w-[100px]">
											<DateTimePicker
												initValue={dateToIDay(date)}
												withTime={false}
												onChange={onDateChange}
												inputClass={"text-xs bg-indigo-400"}
											/>
										</div>
									</div>
									<div className="flex justify-end items-center pt-1">
										<div
											className={`${
												isStartTimeBeforeEndTimeValue
													? "hover:cursor-pointer"
													: "hover:cursor-no-drop text-gray-200"
											}`}
											onClick={isStartTimeBeforeEndTimeValue ? handleCreateRangeTime : () => {}}>
											Save
										</div>
									</div>
								</div>
							</div>
						)}
					</div>
				</div>
			</div>
		</div>
	);
};

export default TaskTimeTrackerDropDownDashboard;
