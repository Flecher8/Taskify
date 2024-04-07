import ProjectRoleListItem from "components/projectRoleListItem";
import { ProjectRole } from "entities/projectRole";
import { FC } from "react";

interface ProjectRoleListProps {
	projectRoles: ProjectRole[];
	filterName: string;
	editRole: (role: ProjectRole) => void;
	deleteRole: (id: string) => void;
}

const ProjectRoleList: FC<ProjectRoleListProps> = ({ projectRoles, filterName, editRole, deleteRole }) => {
	return (
		<div className="flex flex-col flex-between h-full">
			<div className="flex flex-col border-b max-h-96 overflow-auto">
				{projectRoles
					.filter(projectRole => projectRole.name.toLowerCase().includes(filterName.toLowerCase()))
					.map(projectRole => (
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
