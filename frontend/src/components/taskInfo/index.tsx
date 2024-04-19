import { CustomTask } from "entities/customTask";
import { FC } from "react";

interface TaskInfoProps {
	customTask: CustomTask;
}

const TaskInfo: FC<TaskInfoProps> = ({ customTask }) => {
	return <div>Task info</div>;
};

export default TaskInfo;
