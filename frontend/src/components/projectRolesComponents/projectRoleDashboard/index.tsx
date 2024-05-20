import Modal from "components/modal";
import ProjectRoleList from "components/projectRolesComponents/projectRoleDashboard/projectRoleList";
import { ProjectRole, ProjectRoleType } from "entities/projectRole";
import { FC, useEffect, useState } from "react";
import projectRolesStore from "stores/projectRolesStore";
import CreateProjectRoleForm from "./createProjectRoleForm";

interface ProjectRoleDashboardProps {
	projectId?: string;
}

const idModal = "createRole";

const ProjectRoleDashboard: FC<ProjectRoleDashboardProps> = ({ projectId }) => {
	const [roles, setRoles] = useState<ProjectRole[]>([]);
	const [filterByName, setFilterByName] = useState("");

	useEffect(() => {
		// Load project roles when component mounts
		loadProjectRoles();
	}, [projectId]);

	const loadProjectRoles = async () => {
		try {
			const roles = await projectRolesStore.getProjectRolesByProjectId(projectId);
			const sortedProjectRoles = roles.slice().sort((a: ProjectRole, b: ProjectRole) => {
				return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime();
			});

			setRoles(sortedProjectRoles);
		} catch (error) {
			console.error("Error loading project roles:", error);
		}
	};

	const closeModal = () => {
		const modal = document.getElementById(idModal) as HTMLDialogElement;
		modal.close();
	};

	const createRole = async (name: string, roleType: ProjectRoleType) => {
		try {
			if (projectId === undefined) {
				throw new Error("Can not find projectId");
			}
			const newRole = await projectRolesStore.createProjectRole({
				projectId: projectId,
				name: name,
				projectRoleType: roleType
			});

			loadProjectRoles();
		} catch (error) {
			console.error(error);
		}
	};

	const editRole = async (role: ProjectRole) => {
		try {
			if (projectId === undefined) {
				throw new Error("Can not find projectId");
			}
			await projectRolesStore.updateProjectRole(role.id, role);

			loadProjectRoles();
		} catch (error) {
			console.error(error);
		}
	};

	const deleteRole = async (id: string) => {
		try {
			if (projectId === undefined) {
				throw new Error("Can not find projectId");
			}
			await projectRolesStore.deleteProjectRole(id);

			loadProjectRoles();
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<div className="flex flex-col w-full justify-centerspace-y-4 h-full">
			<div className="flex justify-between">
				<input
					type="text"
					className="p-2 border rounded"
					placeholder="Filter by name"
					value={filterByName}
					onChange={e => setFilterByName(e.target.value)}
				/>
				<Modal id={idModal} openButtonText="Create" openButtonStyle="px-4 py-2 bg-blue-500 text-white rounded">
					<CreateProjectRoleForm create={createRole} close={closeModal} />
				</Modal>
			</div>
			<div className="mt-5 h-full">
				{roles.length === 0 ? (
					<p className="flex text-xl italic justify-center overflow-auto">There are no roles in this project.</p>
				) : (
					<ProjectRoleList roles={roles} filterName={filterByName} editRole={editRole} deleteRole={deleteRole} />
				)}
			</div>
		</div>
	);
};

export default ProjectRoleDashboard;
