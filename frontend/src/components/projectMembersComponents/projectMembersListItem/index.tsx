import Modal from "components/modal";
import { ProjectMember } from "entities/projectMember";
import { FC } from "react";
import EditProjectMemberForm from "./editProjectMemberForm";
import DeleteProjectMemberForm from "./deleteProjectMemberForm";

interface ProjectMembersListItemProps {
	member: ProjectMember;
	editMember: (member: ProjectMember) => void;
	deleteMember: (id: string) => void;
}

const idEditMemberModal = "editProjectMember";
const idDeleteMemberModal = "deleteProjectMember";

const ProjectMembersListItem: FC<ProjectMembersListItemProps> = ({ member, editMember, deleteMember }) => {
	const closeEditModal = () => {
		const modal = document.getElementById(member.id + idEditMemberModal) as HTMLDialogElement;
		modal.close();
	};

	const closeDeleteModal = () => {
		const modal = document.getElementById(member.id + idDeleteMemberModal) as HTMLDialogElement;
		modal.close();
	};

	return (
		<div className="grid grid-cols-7 gap-4 border-t p-3 items-center">
			<div className="truncate col-span-4">{member.user.firstName + " " + member.user.lastName}</div>
			<div className="truncate col-span-2">{member.projectRole === null ? "No role" : member.projectRole?.name}</div>
			<div className="flex justify-around col-span-1">
				<div className="mr-5">
					<Modal
						id={member.id + idEditMemberModal}
						openButtonText={<i className="fa-light fa-pen-to-square rounded-full hover:bg-gray-200 p-1 w-10"></i>}
						openButtonStyle={""}>
						<EditProjectMemberForm member={member} edit={editMember} close={closeEditModal} />
					</Modal>
				</div>
				<div>
					<Modal
						id={member.id + idDeleteMemberModal}
						openButtonText={<i className="fa-light fa-trash rounded-full hover:bg-gray-200 p-1 w-10"></i>}
						openButtonStyle={""}>
						<DeleteProjectMemberForm member={member} deleteMember={deleteMember} close={closeDeleteModal} />
					</Modal>
				</div>
			</div>
		</div>
	);
};

export default ProjectMembersListItem;
