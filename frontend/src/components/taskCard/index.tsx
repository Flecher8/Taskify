import { Draggable } from "@hello-pangea/dnd";
import Modal from "components/modal";
import TaskInfo from "components/taskInfo";
import { CustomTask } from "entities/customTask";
import { Section } from "entities/section";
import { FC, useState } from "react";

interface TaskCardProps {
	customTask: CustomTask;
	index: number;
	editTask: (customTask: CustomTask) => void;
	deleteTask: (id: string) => void;
	section: Section;
}

const TaskCard: FC<TaskCardProps> = ({ customTask, index, editTask, deleteTask, section }) => {
	const modalId = customTask.id + "editTask";

	const closeModal = () => {
		const modal = document.getElementById(modalId) as HTMLDialogElement;
		modal.close();
	};

	return (
		<Draggable draggableId={customTask.id} index={index}>
			{provided => (
				<div
					className="border rounded-lg p-[8px] mb-[8px] bg-white hover:border hover:border-purple-300"
					{...provided.draggableProps}
					{...provided.dragHandleProps}
					ref={provided.innerRef}>
					<div className="flex flex-row justify-between items-center">
						<div>
							<h3 className="max-w-[220px] text-ellipsis overflow-hidden">{customTask.name}</h3>
						</div>
						<div
							data-rfd-drag-handle-context-id={provided.dragHandleProps?.["data-rfd-drag-handle-context-id"]}
							data-rfd-drag-handle-draggable-id="gibberish"
							style={{
								// When you set the data-rbd-drag-handle-context-id, RBD applies cursor: grab, so we need to revert that
								cursor: "auto"
							}}>
							<Modal
								id={modalId}
								modalBoxStyle={"h-full"}
								openButtonText={
									<div className="relative top-0 left-0">
										<i className="fa-light fa-ellipsis"></i>
									</div>
								}
								openButtonStyle={
									"hover:cursor-pointer hover:bg-gray-300 rounded-full px-1 transition duration-300"
								}>
								<TaskInfo
									customTask={customTask}
									section={section}
									close={closeModal}
									editTask={editTask}
									deleteTask={deleteTask}
								/>
							</Modal>
						</div>
					</div>
				</div>
			)}
		</Draggable>
	);
};

export default TaskCard;
