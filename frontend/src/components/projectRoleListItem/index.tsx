import { ProjectRole } from "entities/projectRole";
import { FC } from "react";

interface ProjectRoleListItemProps {
	projectRole: ProjectRole;
	editRole: (role: ProjectRole) => void;
	deleteRole: (id: string) => void;
}

const ProjectRoleListItem: FC<ProjectRoleListItemProps> = ({ projectRole, editRole, deleteRole }) => {
	return (
		<div className="flex flex-row justify-between items-center">
			<div>{projectRole.name}</div>
			<div>{projectRole.projectRoleType.toString()}</div>
		</div>
	);
};

export default ProjectRoleListItem;
