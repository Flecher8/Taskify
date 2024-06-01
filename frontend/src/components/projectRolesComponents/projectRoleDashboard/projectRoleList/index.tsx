import ProjectRoleListItem from "components/projectRolesComponents/projectRoleDashboard/projectRoleList/projectRoleListItem";
import { ProjectRole } from "entities/projectRole";
import { FC } from "react";

interface ProjectRoleListProps {
	roles: ProjectRole[];
	filterName: string;
	editRole: (role: ProjectRole) => void;
	deleteRole: (id: string) => void;
}

const ProjectRoleList: FC<ProjectRoleListProps> = ({ roles, filterName, editRole, deleteRole }) => {
	return (
		<div className="flex flex-col flex-between h-full">
			<div className="flex flex-col border-b max-h-96 overflow-auto custom-scroll-sm">
				{roles
					.filter(role => role.name.toLowerCase().includes(filterName.toLowerCase()))
					.map(role => (
						<ProjectRoleListItem key={role.id} role={role} editRole={editRole} deleteRole={deleteRole} />
					))}
			</div>
		</div>
	);
};

export default ProjectRoleList;
