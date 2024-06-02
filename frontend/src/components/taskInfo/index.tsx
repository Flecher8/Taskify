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
		<div className="flex flex-row justify-center min-h-full">
			<div className="flex flex-col w-3/4">
				<div className="grid grid-cols-9 items-center mb-5">
					<div className="flex col-span-1">
						<i className="fa-light fa-list-check"></i>
					</div>
					<div className="col-span-8">
						<ClickToEdit initialValue={customTask.name} onValueChange={handleTaskNameChange} />
					</div>
				</div>
				<div className="items-center mb-5 w-full grid grid-cols-9">
					<div className="col-span-1">
						<i className="fa-light fa-user"></i>
					</div>
					<div className="col-span-3">
						<label>Assigned</label>
					</div>
					<div className="col-span-5 flex items-center justify-center w-full">
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
							openDropDownButtonStyle={"rounded-full hover:bg-gray-300 w-50 h-50 p-1 transition duration-300"}
							dropDownContentStyle={"bg-white w-[300px] p-1"}>
							<SelectUsersWithFilter
								users={assignableUsers}
								onSelect={handleUserChange}
								current={selectedUser}
							/>
						</DropDownContext>
					</div>
				</div>
				<div className="mb-5 grid grid-cols-9">
					<div className="col-span-1">
						<i className="fa-light fa-timer"></i>
					</div>
					<div className="col-span-3">
						<label className="w-full">Time tracker</label>
					</div>
					<div className="col-span-5 flex items-center justify-center">
						<TaskTimeTrackerComponent customTaskId={customTask.id} />
					</div>
				</div>
				<div className="mb-5 grid grid-cols-9">
					<div className="col-span-1">
						<i className="fa-light fa-calendar"></i>
					</div>
					<div className="col-span-8">
						<div className="items-center grid grid-cols-8 mb-1">
							<div className="col-span-3">
								<label className="w-full">Start date</label>
							</div>
							<div className="col-span-5">
								<DateTimePicker
									initValue={dateToIDay(startDate)}
									onChange={handleStartDateChange}
									calenderModalClass={"max-w-[300px]"}
								/>
							</div>
						</div>
						<div className="items-center grid grid-cols-8">
							<div className="col-span-3">
								<label className="w-full">Due date</label>
							</div>
							<div className="col-span-5">
								<DateTimePicker initValue={dateToIDay(endDate)} onChange={handleEndDateChange} />
							</div>
						</div>
					</div>
				</div>
				<div className="mb-5 grid grid-cols-9">
					<div className="col-span-1">
						<i className="fa-light fa-hundred-points"></i>
					</div>
					<div className="col-span-3">
						<label>Story points</label>
					</div>
					<div className="col-span-5 flex justify-center items-center">
						<ClickToEdit
							initialTextStyle={"bg-gray-100 hover:bg-gray-200 transition duration-200 flex justify-center px-5"}
							inputStyle={"bg-gray-100 flex justify-center items-center"}
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
				<div className="grid grid-cols-9 mb-1">
					<div className="col-span-1">
						<i className="fa-light fa-align-justify"></i>
					</div>
					<div className="col-span-8">
						<label>Description</label>
					</div>
				</div>
				<div className="grid grid-cols-9">
					<div className="col-span-1"> </div>
					<div className="col-span-8">
						<ClickToEdit
							initialTextStyle={
								"bg-gray-100 hover:bg-gray-300 h-[50px] hover:bg-gray-200 transition duration-300"
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
			<div className="flex flex-col w-1/4">
				<div>
					<h2>Actions</h2>
				</div>
				<div className="w-full">
					<DropDownContext
						dropDownDirection="dropdown-end"
						openDropDownButtonContent={
							<button className="w-full flex border border-red-500 hover:bg-red-500 hover:text-white transition duration-300 hover:cursor-pointer px-2 py-1 w-full justify-center items-center font-bold text-red-500">
								Delete
							</button>
						}
						openDropDownButtonStyle="flex items-center transition duration-300"
						dropDownContentStyle="bg-white shadow-lg drop-shadow-lg rounded-md z-[10000] p-1">
						<div className="m-3 h-12 w-24 flex flex-col items-center">
							<h2 className="mb-3">Are you sure?</h2>
							<button
								onClick={handleDeleteTask}
								className="mb-5 flex border bg-red-500 hover:bg-white hover:border-red-600 hover:text-red-600 transition duration-200 rounded-full px-2 py-1 w-full justify-center items-center font-bold text-white">
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
