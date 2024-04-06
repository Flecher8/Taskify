import ProjectRoleListItem from "components/projectRoleListItem";
import { ProjectRole } from "entities/projectRole";
import { FC } from "react";

interface ProjectRoleListProps {
	projectRoles: ProjectRole[];
	editRole: (role: ProjectRole) => void;
	deleteRole: (id: string) => void;
}

const ProjectRoleList: FC<ProjectRoleListProps> = ({ projectRoles, editRole, deleteRole }) => {
	return (
		<div className="">
			<div>
				{projectRoles.map(projectRole => (
					<ProjectRoleListItem
						key={projectRole.id}
						projectRole={projectRole}
						editRole={editRole}
						deleteRole={deleteRole}
					/>
				))}
			</div>
		</div>
	);
};

export default ProjectRoleList;
