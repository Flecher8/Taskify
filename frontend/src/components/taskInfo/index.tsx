import { FC, useEffect, useState } from "react";
import { Section } from "entities/section";
import { CustomTask } from "entities/customTask";
import ClickToEdit from "components/clickToEditText";
import DateTimePicker from "components/dateTimePicker";
import DropDownContext from "components/dropDownContext";
import { User } from "entities/user";
import projectMembersStore from "stores/projectMembersStore";
import projectsStore from "stores/projectsStore";
import SelectUsersWithFilter from "components/selectUsersWithFilter";
import { dateToIDay, iDayToDate } from "utilities/date_IDay_Converter";
import { IDay, IRange } from "react-calendar-datetime-picker/dist/types/type";
import { localToUTC, utcToLocal } from "utilities/timeConverter";
import RangeDateTimePicker from "components/rangeDateTimePicker";
import TaskTimeTrackerComponent from "components/taskTimeTrackerComponent";

interface TaskInfoProps {
	customTask: CustomTask;
	section: Section;
	close: () => void;
	editTask: (customTask: CustomTask) => void;
	deleteTask: (id: string) => void;
}

const TaskInfo: FC<TaskInfoProps> = ({ customTask, section, close, editTask, deleteTask }) => {
	const [selectedUser, setSelectedUser] = useState<User | null>(customTask.responsibleUser);
	const [startDate, setStartDate] = useState<Date | null>(
		customTask.startDateTimeUtc !== null ? utcToLocal(customTask.startDateTimeUtc) : null
	);
	const [endDate, setEndDate] = useState<Date | null>(
		customTask.endDateTimeUtc !== null ? utcToLocal(customTask.endDateTimeUtc) : null
	);

	const [assignableUsers, setAssignableUsers] = useState<User[]>([]);

	useEffect(() => {
		loadProjectMembers();
	}, []);

	const loadProjectMembers = async () => {
		try {
			const project = await projectsStore.getProjectById(section.project?.id);
			const projectCreator = project.user;

			const projectMembers = await projectMembersStore.getProjectMembersByProjectId(section.project?.id);
			const projectMembersUsers = projectMembers.map(pm => pm.user);

			const newAssignableUser: User[] = [];
			newAssignableUser.push(projectCreator, ...projectMembersUsers);
			setAssignableUsers(newAssignableUser);
		} catch (error) {
			console.error("Error loading project members: ", error);
		}
	};

	const handleTaskNameChange = (newName: string) => {
		const updatedTask: CustomTask = { ...customTask, name: newName };
		editTask(updatedTask);
	};

	const handleUserChange = (user: User | null) => {
		setSelectedUser(user);
		const updatedTask: CustomTask = { ...customTask, responsibleUser: user };
		editTask(updatedTask);
	};

	const handleStartDateChange = (date: IDay | null) => {
		if (date !== null) {
			const newDate = iDayToDate(date);
			if (newDate === null) {
				return;
			}

			if (newDate !== null && startDate !== null && newDate.getTime() === startDate.getTime()) {
				return;
			}

			// const utcDate = localToUTC(newDate);
			const utcDate = newDate;

			// Check if the new start date is after the current end date
			if (endDate !== null && newDate.getTime() > endDate.getTime()) {
				setEndDate(newDate);
				const updatedTask: CustomTask = { ...customTask, endDateTimeUtc: utcDate, startDateTimeUtc: utcDate };
				editTask(updatedTask);
				return;
			}

			setStartDate(newDate);
			const updatedTask: CustomTask = { ...customTask, startDateTimeUtc: utcDate };
			editTask(updatedTask);
		} else {
			setStartDate(null);
		}
	};

	const handleEndDateChange = (date: IDay | null) => {
		if (date !== null) {
			const newDate = iDayToDate(date);
			if (newDate === null || newDate === endDate) {
				return;
			}

			if (newDate !== null && endDate !== null && newDate.getTime() === endDate.getTime()) {
				return;
			}

			// const utcDate = localToUTC(newDate);
			const utcDate = newDate;

			// Check if the new end date is before the current start date
			if (startDate !== null && newDate.getTime() < startDate.getTime()) {
				setStartDate(newDate);
				const updatedTask: CustomTask = { ...customTask, endDateTimeUtc: utcDate, startDateTimeUtc: utcDate };
				editTask(updatedTask);
				return;
			}

			setEndDate(newDate);
			const updatedTask: CustomTask = { ...customTask, endDateTimeUtc: utcDate };
			editTask(updatedTask);
		} else {
			setEndDate(null);
		}
	};

	const handleDescriptionChange = (newDescription: string) => {
		const updatedTask: CustomTask = { ...customTask, description: newDescription };
		editTask(updatedTask);
	};

	const handleStoryPointsChange = (newStoryPoints: number | null) => {
		const updatedTask: CustomTask = { ...customTask, storyPoints: newStoryPoints };
		editTask(updatedTask);
	};

	const handleDeleteTask = () => {
		deleteTask(customTask.id);
		close();
	};

	return (
		<div className="flex flex-row justify-center gap-3 min-h-full">
			<div className="flex flex-col w-64">
				<div className="flex flex-row items-center mb-5">
					<div className="flex mr-3">
						<i className="fa-light fa-list-check"></i>
					</div>
					<div className="w-full">
						<ClickToEdit initialValue={customTask.name} onValueChange={handleTaskNameChange} />
					</div>
				</div>
				<div className="flex flex-row items-center mb-5 w-full">
					<div className="mr-3">
						<i className="fa-light fa-user"></i>
					</div>
					<div className="flex flex-row items-center gap-x-5 justify-between">
						<label>Assigned</label>
						<DropDownContext
							dropDownDirection="dropdown-start"
							openDropDownButtonContent={
								selectedUser ? (
									<div className="">
										{selectedUser.firstName} {selectedUser.lastName}{" "}
										<i className="fa-sharp fa-light fa-chevron-down"></i>
									</div>
								) : (
									<div className="">
										Unassigned <i className="fa-sharp fa-light fa-chevron-down"></i>
									</div>
								)
							}
							openDropDownButtonStyle={"rounded-full hover:bg-gray-300 w-50 h-50 p-1  transition duration-300"}
							dropDownContentStyle={"bg-white w-[300px]"}>
							<SelectUsersWithFilter
								users={assignableUsers}
								onSelect={handleUserChange}
								current={selectedUser}
							/>
						</DropDownContext>
					</div>
				</div>
				<div className="flex flex-row mb-5">
					<div className="mr-2">
						<i className="fa-light fa-timer"></i>
					</div>
					<div>
						<div className="flex flex-row justify-between mb-1">
							<label className="w-full mr-2">Time tracker</label>
							<TaskTimeTrackerComponent customTaskId={customTask.id} />
						</div>
					</div>
				</div>
				<div className="flex flex-row mb-5">
					<div className="mr-3">
						<i className="fa-light fa-calendar"></i>
					</div>
					<div className="">
						<div className="flex flex-row items-center mb-1">
							<label className="w-full">Start date</label>
							<DateTimePicker
								initValue={dateToIDay(startDate)}
								onChange={handleStartDateChange}
								calenderModalClass={"max-w-[300px]"}
							/>
						</div>
						<div className="flex flex-row items-center">
							<label className="w-full">Due date</label>
							<DateTimePicker initValue={dateToIDay(endDate)} onChange={handleEndDateChange} />
						</div>
					</div>
				</div>
				<div className="flex flex-row items-center mb-5 w-full">
					<div className="mr-3">
						<i className="fa-light fa-hundred-points"></i>
					</div>
					<div className="flex flex-row items-center justify-between w-full">
						<label className="flex w-36">Story points</label>
						<ClickToEdit
							initialTextStyle={"bg-gray-100 hover:bg-gray-300 transition duration-300 flex justify-center px-5"}
							inputStyle={"bg-gray-100 w-full flex justify-center items-center"}
							initialValue={customTask.storyPoints || null}
							onValueChange={handleStoryPointsChange}
							checkEmptyText={false}
							type={"number"}
							minValue={1}
							maxValue={100}
							placeholder={"-"}
						/>
					</div>
				</div>
				<div className="flex flex-row w-full">
					<div className="mr-3">
						<i className="fa-light fa-align-justify"></i>
					</div>
					<div className="w-full">
						<label>Description</label>
						<ClickToEdit
							initialTextStyle={
								"bg-gray-100 hover:bg-gray-300 h-[50px] hover:bg-gray-300 transition duration-300"
							}
							inputStyle={"h-[200px] w-full "}
							initialValue={customTask.description || ""}
							onValueChange={handleDescriptionChange}
							checkEmptyText={false}
							maxLength={10000}
							isTextArea={true}
							placeholder={"Add description..."}
						/>
					</div>
				</div>
			</div>
			<div className="flex flex-col w-36">
				<div>
					<h2>Actions</h2>
				</div>
				<div>
					<DropDownContext
						dropDownDirection="dropdown-start"
						openDropDownButtonContent={
							<button className="flex border border-red-500 hover:bg-red-500 hover:text-white transition duration-300 hover:cursor-pointer px-2 py-1 w-full justify-center items-center font-bold text-red-500">
								Delete
							</button>
						}
						openDropDownButtonStyle="flex items-center transition duration-300"
						dropDownContentStyle="bg-white">
						<div className="m-3 h-12 w-24 flex flex-col items-center">
							<h2 className="mb-3">Are you sure?</h2>
							<button
								onClick={handleDeleteTask}
								className="mb-5 flex border bg-red-500 hover:bg-red-600 rounded-full px-2 py-1 w-full justify-center items-center font-bold text-white">
								Delete
							</button>
						</div>
					</DropDownContext>
				</div>
			</div>
		</div>
	);
};

export default TaskInfo;
