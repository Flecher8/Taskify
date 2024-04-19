import { Draggable } from "@hello-pangea/dnd";
import Modal from "components/modal";
import TaskInfo from "components/taskInfo";
import { CustomTask } from "entities/customTask";
import { FC, useState } from "react";

interface TaskCardProps {
	customTask: CustomTask;
	index: number;
}

const TaskCard: FC<TaskCardProps> = ({ customTask, index }) => {
	const [showItemMoreButton, setShowItemMoreButton] = useState(false);
	const modalId = customTask.id + "editTask";
	return (
		<Draggable draggableId={customTask.id} index={index}>
			{provided => (
				<div
					className="border rounded-lg p-[8px] mb-[8px] bg-white hover:border hover:border-purple-300"
					{...provided.draggableProps}
					{...provided.dragHandleProps}
					ref={provided.innerRef}
					onMouseEnter={() => setShowItemMoreButton(true)}
					onMouseLeave={() => setShowItemMoreButton(false)}>
					<div className="flex flex-row justify-between items-center">
						<div>
							<h3 className="break-words">{customTask.name}</h3>
						</div>
						<div
							className={`${!showItemMoreButton ? "hidden" : ""}`}
							data-rfd-drag-handle-context-id={provided.dragHandleProps?.["data-rfd-drag-handle-context-id"]}
							data-rfd-drag-handle-draggable-id="gibberish"
							style={{
								// When you set the data-rbd-drag-handle-context-id, RBD applies cursor: grab, so we need to revert that
								cursor: "auto"
							}}>
							<Modal
								id={modalId}
								openButtonText={<i className="fa-light fa-ellipsis"></i>}
								openButtonStyle={
									"hover:cursor-pointer hover:bg-gray-300 rounded-full px-1 transition duration-300"
								}>
								<TaskInfo customTask={customTask} />
							</Modal>
						</div>
					</div>
				</div>
			)}
		</Draggable>
	);
};

export default TaskCard;
