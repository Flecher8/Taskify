import Modal from "components/modal";
import { ProjectMember } from "entities/projectMember";
import { FC } from "react";
import EditProjectMemberForm from "./editProjectMemberForm";
import DeleteProjectMemberForm from "./deleteProjectMemberForm";

interface ProjectMembersListItemProps {
	projectMember: ProjectMember;
	editMember: (role: ProjectMember) => void;
	deleteMember: (id: string) => void;
}

const idEditMemberModal = "editProjectMember";
const idDeleteMemberModal = "deleteProjectMember";

const ProjectMembersListItem: FC<ProjectMembersListItemProps> = ({ projectMember, editMember, deleteMember }) => {
	const closeEditModal = () => {
		const modal = document.getElementById(projectMember.id + idEditMemberModal) as HTMLDialogElement;
		modal.close();
	};

	const closeDeleteModal = () => {
		const modal = document.getElementById(projectMember.id + idDeleteMemberModal) as HTMLDialogElement;
		modal.close();
	};

	return (
		<div className="flex flex-row justify-around border-t p-3">
			<div className="flex flex-row justify-between items-center w-[90%] mr-5">
				<div className="">{projectMember.user.firstName + " " + projectMember.user.lastName}</div>
				<div className="">{projectMember.projectRole === null ? "No role" : projectMember.projectRole?.name}</div>
			</div>
			<div className="flex flex-row justify-around">
				<div className="flex mr-5 items-center justify-center">
					<Modal
						id={projectMember.id + idEditMemberModal}
						openButtonText={
							<i className="fa-light fa-pen-to-square rounded rounded-full hover:bg-gray-200 p-1 w-10"></i>
						}
						openButtonStyle={""}>
						<EditProjectMemberForm member={projectMember} edit={editMember} close={closeEditModal} />
					</Modal>
				</div>
				<div className="flex">
					<Modal
						id={projectMember.id + idDeleteMemberModal}
						openButtonText={<i className="fa-light fa-trash rounded rounded-full hover:bg-gray-200 p-1 w-10"></i>}
						openButtonStyle={""}>
						<DeleteProjectMemberForm
							member={projectMember}
							deleteMember={deleteMember}
							close={closeDeleteModal}
						/>
					</Modal>
				</div>
			</div>
		</div>
	);
};

export default ProjectMembersListItem;
