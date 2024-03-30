import { Draggable } from "@hello-pangea/dnd";
import { CustomTask } from "entities/customTask";
import { FC } from "react";

interface TaskCardProps {
	customTask: CustomTask;
	index: number;
}

const TaskCard: FC<TaskCardProps> = ({ customTask, index }) => {
	return (
		<Draggable draggableId={customTask.id} index={index}>
			{provided => (
				<div
					className="border rounded-lg p-[8px] mb-[8px] bg-white"
					{...provided.draggableProps}
					{...provided.dragHandleProps}
					ref={provided.innerRef}>
					<div>
						<h3 className="break-words">{customTask.name}</h3>
					</div>
				</div>
			)}
		</Draggable>
	);
};

export default TaskCard;
