import Modal from "components/modal";
import { FC } from "react";
import LeaveProjectForm from "./leaveProjectForm";
import { Project } from "entities/project";

interface ProfileProjectListItemProps {
	project: Project;
	leaveProject: (id: string) => void;
}

const idDeleteProjectModal = "deleteProjectModal";

const ProfileProjectListItem: FC<ProfileProjectListItemProps> = ({ project, leaveProject }) => {
	const closeDeleteModal = () => {
		const modal = document.getElementById(project.id + idDeleteProjectModal) as HTMLDialogElement;
		modal.close();
	};

	return (
		<div className="grid grid-cols-7 gap-4 border-t p-3 items-center">
			<div className="truncate col-span-6">{project.name}</div>
			<div className="flex justify-around col-span-1">
				<div>
					<Modal
						id={project.id + idDeleteProjectModal}
						openButtonText={
							<i className="fa-light fa-right-from-bracket rounded-full hover:bg-gray-200 transition duration-300 p-1 w-10"></i>
						}
						openButtonStyle={""}>
						<LeaveProjectForm project={project} leaveProject={leaveProject} close={closeDeleteModal} />
					</Modal>
				</div>
			</div>
		</div>
	);
};

export default ProfileProjectListItem;
