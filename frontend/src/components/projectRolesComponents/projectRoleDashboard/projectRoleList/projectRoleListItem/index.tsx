import Modal from "components/modal";
import { ProjectRole, getRoleTypeName } from "entities/projectRole";
import { FC } from "react";
import EditProjectRoleForm from "./editProjectRoleForm";
import DeleteProjectRoleForm from "./deleteProjectRoleForm";

interface ProjectRoleListItemProps {
	role: ProjectRole;
	editRole: (role: ProjectRole) => void;
	deleteRole: (id: string) => void;
}

const idEditRoleModal = "editProjectRole";
const idDeleteRoleModal = "deleteProjectRole";

const ProjectRoleListItem: FC<ProjectRoleListItemProps> = ({ role, editRole, deleteRole }) => {
	const closeEditModal = () => {
		const modal = document.getElementById(role.id + idEditRoleModal) as HTMLDialogElement;
		modal.close();
	};

	const closeDeleteModal = () => {
		const modal = document.getElementById(role.id + idDeleteRoleModal) as HTMLDialogElement;
		modal.close();
	};

	return (
		<div className="flex flex-row justify-around border-t p-3">
			<div className="flex flex-row justify-between items-center w-[90%] mr-5">
				<div className="">{role.name}</div>
				<div className="">{getRoleTypeName(role.projectRoleType)}</div>
			</div>
			<div className="flex flex-row justify-around">
				<div className="flex mr-5 items-center justify-center">
					<Modal
						id={role.id + idEditRoleModal}
						openButtonText={
							<i className="fa-light fa-pen-to-square rounded rounded-full hover:bg-gray-200 p-1 w-10"></i>
						}
						openButtonStyle={""}>
						<EditProjectRoleForm role={role} edit={editRole} close={closeEditModal} />
					</Modal>
				</div>
				<div className="flex">
					<Modal
						id={role.id + idDeleteRoleModal}
						openButtonText={<i className="fa-light fa-trash rounded rounded-full hover:bg-gray-200 p-1 w-10"></i>}
						openButtonStyle={""}>
						<DeleteProjectRoleForm role={role} deleteRole={deleteRole} close={closeDeleteModal} />
					</Modal>
				</div>
			</div>
		</div>
	);
};

export default ProjectRoleListItem;
