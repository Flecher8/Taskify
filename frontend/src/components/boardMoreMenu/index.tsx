import DeleteProjectForm from "components/deleteProjectForm";
import Modal from "components/modal";
import { Project } from "entities/project";
import { FC } from "react";

interface BoardMoreMenuProps {
	project: Project;
	deleteProject(): void;
}

const BoardMoreMenu: FC<BoardMoreMenuProps> = ({ project, deleteProject }) => {
	const modalDeleteId = project.id + "deleteModal";

	const closeDeleteModal = () => {
		const modal = document.getElementById(modalDeleteId) as HTMLDialogElement;
		modal.close();
	};
	return (
		<div className="flex flex-col w-32 m-1">
			<Modal
				id={modalDeleteId}
				openButtonText={
					<div className="flex flex-row gap-2">
						<i className="fa-light fa-trash rounded-full p-1"></i>
						<div>Delete</div>
					</div>
				}
				openButtonStyle={"flex hover:bg-gray-200 transition duration-200 hover:cursor-pointer pl-3 rounded"}>
				<DeleteProjectForm deleteProject={deleteProject} close={closeDeleteModal} />
			</Modal>
		</div>
	);
};

export default BoardMoreMenu;
