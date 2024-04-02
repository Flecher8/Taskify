import { Draggable, Droppable } from "@hello-pangea/dnd";
import TaskCard from "components/taskCard";
import { Section } from "entities/section";
import { FC, useEffect, useRef, useState } from "react";
import "./section.scss";

interface SectionCardProps {
	section: Section;
	index: number;
	createTask: (sectionId: string, newTaskName: string) => void;
}

const SectionCard: FC<SectionCardProps> = ({ section, index, createTask }) => {
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

	const createSection = (name: string) => {
		createTask(section.id, name);
	};

	return (
		<Draggable draggableId={section.id} index={index}>
			{sectionProvided => (
				<div
					className="bg-white m-[8px] border rounded-lg w-[300px] flex flex-col shrink-0"
					{...sectionProvided.draggableProps}
					ref={sectionProvided.innerRef}>
					<div className="p-[12px]" {...sectionProvided.dragHandleProps}>
						<h3 className="break-words text-base">{section.name}</h3>
					</div>
					<Droppable droppableId={section.id} type="task">
						{providedDroppable => {
							return (
								<div
									className="p-[8px] min-h-[5px] text-sm"
									{...providedDroppable.droppableProps}
									ref={providedDroppable.innerRef}>
									{section.customTasks.map((customTask, index) => (
										<TaskCard key={customTask.id} customTask={customTask} index={index} />
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
