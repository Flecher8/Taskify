import { FC, useEffect, useState } from "react";
import { Section } from "entities/section";
import { CustomTask } from "entities/customTask";
import ClickToEditText from "components/clickToEditText";
import DateTimePicker from "components/dateTimePicker";
import DropDownContext from "components/dropDownContext";
import { User } from "entities/user";
import projectMembersStore from "stores/projectMembersStore";
import projectsStore from "stores/projectsStore";
import SelectUsersWithFilter from "components/selectUsersWithFilter";
import { dateToIDay, iDayToDate } from "utilities/date_IDay_Converter";
import { IDay } from "react-calendar-datetime-picker/dist/types/type";
import { localToUTC, utcToLocal } from "utilities/timeConverter";

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
		customTask.startDateTimeUtc ? utcToLocal(customTask.startDateTimeUtc) : null
	);
	const [endDate, setEndDate] = useState<Date | null>(
		customTask.endDateTimeUtc ? utcToLocal(customTask.endDateTimeUtc) : null
	);

	const [assignableUsers, setAssignableUsers] = useState<User[]>([]);

	useEffect(() => {
		setStartDate(customTask.startDateTimeUtc || null);
		setEndDate(customTask.endDateTimeUtc || null);
	}, [customTask.startDateTimeUtc, customTask.endDateTimeUtc]);

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
			setStartDate(newDate);
			const updatedTask: CustomTask = { ...customTask, startDateTimeUtc: localToUTC(newDate) };
			editTask(updatedTask);
		}

		setStartDate(null);
	};

	const handleEndDateChange = (date: IDay | null) => {
		if (date !== null) {
			const newDate = iDayToDate(date);
			if (newDate === null) {
				return;
			}
			setEndDate(newDate);
			const updatedTask: CustomTask = { ...customTask, endDateTimeUtc: localToUTC(newDate) };
			editTask(updatedTask);
		}

		setEndDate(null);
	};

	const handleDescriptionChange = (newDescription: string) => {};

	const handleDeleteTask = () => {
		deleteTask(customTask.id);
	};

	return (
		<div className="flex flex-row justify-center gap-3 min-h-full">
			<div className="flex flex-col w-64">
				<div>
					<div>
						<i className="fa-light fa-list-check"></i>
					</div>
					<div>
						<ClickToEditText initialValue={customTask.name} onValueChange={handleTaskNameChange} />
					</div>
				</div>
				<div className="flex flex-row gap-x-5 items-center">
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
						<SelectUsersWithFilter users={assignableUsers} onSelect={handleUserChange} current={selectedUser} />
					</DropDownContext>
				</div>
				<div className="flex flex-row items-center">
					<label className="w-full">Start date</label>
					<DateTimePicker initValue={dateToIDay(startDate)} onChange={handleStartDateChange} />
				</div>
				<div className="flex flex-row items-center ">
					<label className="w-full">Due date</label>
					<DateTimePicker initValue={dateToIDay(endDate)} onChange={handleEndDateChange} />
				</div>
				<div className="flex flex-row items-center">
					<label>Story points</label>
					<ClickToEditText
						initialTextStyle={
							"bg-gray-100 hover:bg-gray-300 transition duration-300 w-5 flex justify-center items-center"
						}
						inputStyle={"w-[50px] bg-gray-100 w-5 flex justify-center items-center"}
						initialValue={customTask.storyPoints || 1}
						onValueChange={handleDescriptionChange}
						checkEmptyText={false}
						type={"number"}
						minValue={1}
						maxValue={100}
					/>
				</div>
				<div>
					<label>Description</label>
					<ClickToEditText
						initialTextStyle={"bg-gray-100 hover:bg-gray-300 h-[50px] hover:bg-gray-300 transition duration-300"}
						inputStyle={"h-[200px] w-full "}
						initialValue={customTask.description || ""}
						onValueChange={handleDescriptionChange}
						checkEmptyText={false}
						maxLength={10000}
						isTextArea={true}
					/>
				</div>
			</div>
			<div className="flex flex-col w-36">
				<div>
					<h2>Actions</h2>
				</div>
				<div>
					<DropDownContext
						openDropDownButtonContent={<button>Delete</button>}
						openDropDownButtonStyle="p-1 flex items-center hover:bg-gray-300 hover:cursor-pointer transition duration-300"
						dropDownContentStyle="bg-white">
						<div>
							<p>Are you sure?</p>
							<button onClick={handleDeleteTask}>Confirm</button>
						</div>
					</DropDownContext>
				</div>
			</div>
		</div>
	);
};

export default TaskInfo;
