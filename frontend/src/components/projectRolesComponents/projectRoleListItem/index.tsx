import Modal from "components/modal";
import { ProjectRole, getRoleTypeName } from "entities/projectRole";
import { FC } from "react";
import EditProjectRoleForm from "./editProjectRoleForm";
import DeleteProjectRoleForm from "./deleteProjectRoleForm";

interface ProjectRoleListItemProps {
	projectRole: ProjectRole;
	editRole: (role: ProjectRole) => void;
	deleteRole: (id: string) => void;
}

const idEditRoleModal = "editProjectRole";
const idDeleteRoleModal = "deleteProjectRole";

const ProjectRoleListItem: FC<ProjectRoleListItemProps> = ({ projectRole, editRole, deleteRole }) => {
	const closeEditModal = () => {
		const modal = document.getElementById(projectRole.id + idEditRoleModal) as HTMLDialogElement;
		modal.close();
	};

	const closeDeleteModal = () => {
		const modal = document.getElementById(projectRole.id + idDeleteRoleModal) as HTMLDialogElement;
		modal.close();
	};

	return (
		<div className="flex flex-row justify-around border-t p-3">
			<div className="flex flex-row justify-between items-center w-[90%] mr-5">
				<div className="">{projectRole.name}</div>
				<div className="">{getRoleTypeName(projectRole.projectRoleType)}</div>
			</div>
			<div className="flex flex-row justify-around">
				<div className="flex mr-5 items-center justify-center">
					<Modal
						id={projectRole.id + idEditRoleModal}
						openButtonText={
							<i className="fa-light fa-pen-to-square rounded rounded-full hover:bg-gray-200 p-1 w-10"></i>
						}
						openButtonStyle={""}>
						<EditProjectRoleForm role={projectRole} edit={editRole} close={closeEditModal} />
					</Modal>
				</div>
				<div className="flex">
					<Modal
						id={projectRole.id + idDeleteRoleModal}
						openButtonText={<i className="fa-light fa-trash rounded rounded-full hover:bg-gray-200 p-1 w-10"></i>}
						openButtonStyle={""}>
						<DeleteProjectRoleForm role={projectRole} deleteRole={deleteRole} close={closeDeleteModal} />
					</Modal>
				</div>
			</div>
		</div>
	);
};

export default ProjectRoleListItem;
