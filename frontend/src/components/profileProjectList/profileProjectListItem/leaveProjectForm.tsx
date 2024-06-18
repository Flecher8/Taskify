import { Project } from "entities/project";
import { FC } from "react";

interface DeleteProjectFormProps {
	project: Project;
	leaveProject: (id: string) => void;
	close: () => void;
}

const LeaveProjectForm: FC<DeleteProjectFormProps> = ({ project, leaveProject, close }) => {
	const handleDelete = () => {
		leaveProject(project.id);
		close();
	};

	return (
		<div>
			<h2 className="text-lg font-semibold">Leave Project</h2>
			<p>Are you sure you want to leave the project "{project.name}"?</p>
			<div className="mt-4 flex justify-end">
				<button className="btn btn-secondary mr-2" onClick={close}>
					Cancel
				</button>
				<button className="btn btn-danger" onClick={handleDelete}>
					Leave
				</button>
			</div>
		</div>
	);
};

export default LeaveProjectForm;
