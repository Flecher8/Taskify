import {
	DragDropContext,
	DragStart,
	Droppable,
	ResponderProvided,
	DropResult,
	DraggableLocation
} from "@hello-pangea/dnd";
import SectionCard from "components/sectionCard";
import { CustomTask } from "entities/customTask";
import { Project } from "entities/project";
import { Section, SectionType } from "entities/section";
import { FC, useEffect, useRef, useState } from "react";
import sectionsStore from "stores/sectionsStore";
import "./board.scss";
import customTaskStore from "stores/customTasksStore";

interface BoardProps {
	project: Project;
}

const Board: FC<BoardProps> = ({ project }) => {
	const [sections, setSections] = useState<Section[]>([]);

	const [isCreatingSection, setIsCreatingSection] = useState(false);
	const [newSectionName, setNewSectionName] = useState<string>("");
	const inputRef = useRef<HTMLInputElement>(null);
	const containerRef = useRef<HTMLDivElement>(null);

	const loadData = async () => {
		const newSections = await sectionsStore.getSectionsByProjectId(project.id);

		setSections(newSections);

		try {
		} catch (error) {
			console.error(error);
		}
	};

	const onDragEnd = (result: DropResult) => {
		const { destination, source, draggableId, type } = result;

		if (!destination || (destination.droppableId === source.droppableId && destination.index === source.index)) {
			return;
		}

		if (type === "column") {
			handleColumnReorder(source.index, destination.index);
		} else {
			handleTaskMovement(source, destination, draggableId);
		}
	};

	const handleColumnReorder = (sourceIndex: number, destinationIndex: number) => {
		const newSections = Array.from(sections);
		const [movedSection] = newSections.splice(sourceIndex, 1);
		newSections.splice(destinationIndex, 0, movedSection);

		updateSequenceNumbers(newSections);
		setSections(newSections);

		// Send requset to backend
		try {
			sectionsStore.moveSection({ id: movedSection.id, moveTo: movedSection.sequenceNumber });
		} catch (error: any) {
			console.error(error);
		}
	};

	const handleTaskMovement = (source: DraggableLocation, destination: DraggableLocation, draggableId: string) => {
		const start = sections.find(section => section.id === source.droppableId);
		const finish = sections.find(section => section.id === destination.droppableId);

		if (!start || !finish) {
			return;
		}

		if (start === finish) {
			moveTaskWithinSameSection(start, source.index, destination.index);
		} else {
			moveTaskToDifferentSection(start, finish, source.index, destination.index, draggableId);
		}
	};

	const moveTaskWithinSameSection = (section: Section, startIndex: number, endIndex: number) => {
		const newTasks = Array.from(section.customTasks);
		const [movedTask] = newTasks.splice(startIndex, 1);
		newTasks.splice(endIndex, 0, movedTask);

		updateSequenceNumbers(newTasks);
		const newSection = { ...section, customTasks: newTasks };

		updateSectionsInSectionArray(newSection);

		// Send requset to backend
		try {
			customTaskStore.moveCustomTask({ id: movedTask.id, targetSequenceNumber: movedTask.sequenceNumber });
		} catch (error: any) {
			console.error(error);
		}
	};

	const moveTaskToDifferentSection = (
		start: Section,
		finish: Section,
		startIndex: number,
		endIndex: number,
		draggableId: string
	) => {
		const startTasks = Array.from(start.customTasks);
		const [movedTask] = startTasks.splice(startIndex, 1);

		updateSequenceNumbers(startTasks);

		const finishTasks = Array.from(finish.customTasks);
		finishTasks.splice(endIndex, 0, movedTask);

		updateSequenceNumbers(finishTasks);

		const newStart = { ...start, customTasks: startTasks };
		const newFinish = { ...finish, customTasks: finishTasks };

		updateSectionsInSectionArray(newStart, newFinish);

		// Send requset to backend
		try {
			customTaskStore.redirectCustomTask({
				id: movedTask.id,
				targetSectionId: finish.id,
				targetSequenceNumber: movedTask.sequenceNumber
			});
		} catch (error: any) {
			console.error(error);
		}
	};

	const updateSequenceNumbers = (items: { sequenceNumber: number }[]) => {
		items.forEach((item, index) => {
			item.sequenceNumber = index;
		});
	};

	const updateSectionsInSectionArray = (...updatedSections: Section[]) => {
		const newSections = sections.map(section => {
			const updatedSection = updatedSections.find(updated => updated.id === section.id);
			return updatedSection ? updatedSection : section;
		});
		setSections(newSections);
	};

	const createTask = async (sectionId: string, newTaskName: string) => {
		try {
			// Send request to create the task on the server
			const createdTask = await customTaskStore.createCustomTask({ sectionId: sectionId, name: newTaskName });

			// If the server successfully creates the task, update the UI with the received data
			setSections(prevSections => {
				const newSections = [...prevSections];
				const sectionIndex = newSections.findIndex(s => s.id === sectionId);
				if (sectionIndex !== -1) {
					newSections[sectionIndex].customTasks.push(createdTask);
				}

				return newSections;
			});
		} catch (error) {
			console.error(error);
		}
	};

	const createSection = async (name: string) => {
		try {
			// Send request to create the section on the server
			const createdSection = await sectionsStore.createSection({ projectId: project.id, name: name });

			setSections(prevSections => [...prevSections, createdSection]);
		} catch (error) {
			console.error(error);
		}
	};

	useEffect(() => {
		if (isCreatingSection && inputRef.current) {
			inputRef.current.focus();
		}
	}, [isCreatingSection]);

	useEffect(() => {
		const handleClickOutside = (event: MouseEvent) => {
			if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
				setIsCreatingSection(false);
			}
		};

		document.addEventListener("mousedown", handleClickOutside);

		return () => {
			document.removeEventListener("mousedown", handleClickOutside);
		};
	}, []);

	const handleCreateSectionClick = () => {
		setIsCreatingSection(true);
	};

	const handleCancelClick = () => {
		setIsCreatingSection(false);
		setNewSectionName("");
	};

	const handleCreateClick = () => {
		createSection(newSectionName);
		// Reset state
		setIsCreatingSection(false);
		setNewSectionName("");
	};

	const editSection = async (section: Section) => {
		try {
			await sectionsStore.updateSection(section.id, section);
			// Update the sections state with the updated section
			const updatedSections = sections.map(s => (s.id === section.id ? section : s));
			setSections(updatedSections);
		} catch (error) {
			console.error("Error editing section:", error);
		}
	};

	const deleteSection = async (id: string, redirectId: string) => {
		try {
			await sectionsStore.deleteRedirect(id, redirectId);

			loadData();
		} catch (error) {
			console.error("Error deleting section:", error);
		}
	};

	const editTask = async (updatedTask: CustomTask) => {
		try {
			// Iterate over each section
			const updatedSections = sections.map(section => {
				// Find the task in the section by ID
				const updatedTasks = section.customTasks.map(task => {
					if (task.id === updatedTask.id) {
						// Update the task with the new values
						return updatedTask;
					}
					return task;
				});
				// Update the section with the updated tasks
				return { ...section, customTasks: updatedTasks };
			});
			// Update the state with the updated sections
			setSections(updatedSections);

			// Send request to update the task on the server
			await customTaskStore.updateCustomTask(updatedTask.id, updatedTask);
		} catch (error) {
			console.error("Error editing task:", error);
		}
	};

	const deleteTask = async (id: string) => {
		try {
			// Send request to delete the task from the server
			await customTaskStore.deleteCustomTask(id);

			// Remove the deleted task from all sections
			const updatedSections = sections.map(section => {
				// Check if the task with the given ID exists in this section
				const taskIndex = section.customTasks.findIndex(task => task.id === id);
				if (taskIndex !== -1) {
					// If found, create a new array without the task
					const updatedTasks = [
						...section.customTasks.slice(0, taskIndex),
						...section.customTasks.slice(taskIndex + 1)
					];
					// Return the section with the updated tasks
					return { ...section, customTasks: updatedTasks };
				}
				// If not found, return the section as is
				return section;
			});

			// Update the state with the updated sections
			setSections(updatedSections);
		} catch (error) {
			console.error("Error deleting task:", error);
		}
	};

	useEffect(() => {
		loadData();
	}, [project]);

	return (
		<div className="board flex flex-row flex-nowrap overflow-x-auto w-full h-full">
			<DragDropContext onDragEnd={onDragEnd}>
				<Droppable droppableId="all-sections" direction="horizontal" type="column">
					{provided => (
						<div
							className="sections flex flex-row flex-nowrap items-start"
							{...provided.droppableProps}
							ref={provided.innerRef}>
							{sections.map((section, index) => {
								return (
									<SectionCard
										key={section.id}
										section={section}
										index={index}
										createTask={createTask}
										editTask={editTask}
										deleteTask={deleteTask}
										editSection={editSection}
										deleteSection={deleteSection}
										sections={sections}
									/>
								);
							})}
							{provided.placeholder}
						</div>
					)}
				</Droppable>
			</DragDropContext>
			<div className="flex w-[300px] shrink-0">
				{isCreatingSection ? (
					<div className="m-[8px] m-[10px] flex flex-col duration-200 w-full shrink-0" ref={containerRef}>
						<div>
							<input
								type="text"
								value={newSectionName}
								className="border text-sm rounded-md w-full min-h-[50px] p-1"
								onChange={e => setNewSectionName(e.target.value)}
								placeholder="Enter section name	"
								ref={inputRef}
							/>
						</div>
						<div className="mt-2 flex flex-row flex-nowrap">
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
						className="m-[8px] h-[30px] flex shrink-0 w-full text-sm bg-stone-100 border rounded-lg hover:bg-stone-200 duration-200"
						onClick={handleCreateSectionClick}>
						<div className="flex flex-row items-center flex-nowrap shrink-0">
							<i className="fa-light fa-plus ml-2 mr-1"></i>
							<h3>Create new section</h3>
						</div>
					</button>
				)}
			</div>
		</div>
	);
};

export default Board;
