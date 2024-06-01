import { Draggable, Droppable } from "@hello-pangea/dnd";
import TaskCard from "components/taskCard";
import { Section, SectionType } from "entities/section";
import { FC, useEffect, useRef, useState } from "react";
import "./section.scss";
import ClickToEdit from "components/clickToEditText";
import sectionsStore from "stores/sectionsStore";
import DropDownContext from "components/dropDownContext";
import MyDropDownContext from "components/myDropDownContext";
import SectionCardMenu from "./sectionCardMenu";
import { CustomTask } from "entities/customTask";

interface SectionCardProps {
	section: Section;
	index: number;
	createTask: (sectionId: string, newTaskName: string) => void;
	editTask: (customTask: CustomTask) => void;
	deleteTask: (id: string) => void;
	editSection: (section: Section) => void;
	deleteSection: (id: string, redirectId: string) => void;
	sections: Section[];
}

const SectionCard: FC<SectionCardProps> = ({
	section,
	index,
	createTask,
	editTask,
	deleteTask,
	editSection,
	deleteSection,
	sections
}) => {
	const [isCreatingTask, setIsCreatingTask] = useState(false);
	const [newTaskName, setNewTaskName] = useState("");

	const inputRef = useRef<HTMLInputElement>(null);
	const containerRef = useRef<HTMLDivElement>(null);

	useEffect(() => {
		if (isCreatingTask && inputRef.current) {
			inputRef.current.focus();
		}
	}, [isCreatingTask]);

	useEffect(() => {
		const handleClickOutside = (event: MouseEvent) => {
			if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
				setIsCreatingTask(false);
			}
		};

		document.addEventListener("mousedown", handleClickOutside);

		return () => {
			document.removeEventListener("mousedown", handleClickOutside);
		};
	}, []);

	const handleCreateTaskClick = () => {
		setIsCreatingTask(true);
	};

	const handleCancelClick = () => {
		setIsCreatingTask(false);
		setNewTaskName("");
	};

	const handleCreateClick = () => {
		createTask(section.id, newTaskName);
		// Reset state
		setIsCreatingTask(false);
		setNewTaskName("");
	};

	const handleSectionNameChange = async (newName: string) => {
		try {
			const newSection = section;
			newSection.name = newName;
			await sectionsStore.updateSection(section.id, newSection);
			section.name = newName;
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<Draggable draggableId={section.id} index={index}>
			{sectionProvided => (
				<div
					className="bg-white m-[8px] border rounded-lg w-[300px] flex flex-col shrink-0"
					{...sectionProvided.draggableProps}
					ref={sectionProvided.innerRef}>
					<div
						className="p-[12px] flex flex-row justify-between items-center"
						{...sectionProvided.dragHandleProps}>
						<div className="flex flex-row gap-1 items-center">
							<ClickToEdit
								initialValue={section.name}
								onValueChange={handleSectionNameChange}
								useHover={false}
							/>
							<div
								className={`w-[10px] h-[10px] rounded-full border ${
									section.sectionType === SectionType.ToDo
										? "bg-gray-300"
										: section.sectionType === SectionType.Doing
										? "bg-blue-300"
										: "bg-green-300"
								}`}></div>
						</div>
						<div
							className="flex items-center"
							data-rfd-drag-handle-context-id={
								sectionProvided.dragHandleProps?.["data-rfd-drag-handle-context-id"]
							}
							data-rfd-drag-handle-draggable-id="gibberish"
							style={{
								// When you set the data-rbd-drag-handle-context-id, RBD applies cursor: grab, so we need to revert that
								cursor: "auto"
							}}>
							<DropDownContext
								dropDownDirection={"dropdown-start"}
								openDropDownButtonContent={<i className="fa-light fa-ellipsis"></i>}
								openDropDownButtonStyle={
									"p-1 flex items-center hover:bg-gray-200 hover:cursor-pointer transition duration-300"
								}
								dropDownContentStyle={"bg-white rounded-md"}>
								<SectionCardMenu
									section={section}
									editSection={editSection}
									deleteSection={deleteSection}
									sections={sections}
								/>
							</DropDownContext>
						</div>
					</div>
					<Droppable droppableId={section.id} type="task">
						{providedDroppable => {
							return (
								<div
									className="section p-[8px] min-h-[5px] text-sm max-h-96 overflow-y-auto"
									{...providedDroppable.droppableProps}
									ref={providedDroppable.innerRef}>
									{section.customTasks.map((customTask, index) => (
										<TaskCard
											key={customTask.id}
											customTask={customTask}
											index={index}
											editTask={editTask}
											deleteTask={deleteTask}
											section={section}
										/>
									))}
									{providedDroppable.placeholder}
								</div>
							);
						}}
					</Droppable>
					{isCreatingTask ? (
						<div className="m-[8px] m-[10px] flex flex-col duration-200" ref={containerRef}>
							<div>
								<input
									type="text"
									value={newTaskName}
									className="border text-sm rounded-md w-full min-h-[50px] p-1"
									onChange={e => setNewTaskName(e.target.value)}
									placeholder="Enter a name for this task"
									ref={inputRef}
								/>
							</div>
							<div className="mt-2">
								<button
									className="bg-[#0c66e4] h-[30px] w-[100px] text-sm border rounded-md mr-5 text-white"
									onClick={handleCreateClick}>
									Create
								</button>
								<button onClick={handleCancelClick}>
									<i className="fa-light fa-xmark"></i>
								</button>
							</div>
						</div>
					) : (
						<button
							className="m-[8px] h-[30px] text-sm  bg-stone-100 border rounded-lg hover:bg-stone-200 duration-200 flex flex-row items-center"
							onClick={handleCreateTaskClick}>
							<i className="fa-light fa-plus ml-2 mr-1"></i>
							<h3>Create new task</h3>
						</button>
					)}
				</div>
			)}
		</Draggable>
	);
};

export default SectionCard;
