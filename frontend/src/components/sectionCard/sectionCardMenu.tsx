import Modal from "components/modal";
import { Section } from "entities/section";
import { FC } from "react";
import EditSectionForm from "./editSectionForm";
import DeleteSectionForm from "./deleteSectionForm";

interface SectionCardMenuProps {
	section: Section;
	editSection: (section: Section) => void;
	deleteSection: (id: string, redirectId: string) => void;
	sections: Section[];
}

const SectionCardMenu: FC<SectionCardMenuProps> = ({ section, editSection, deleteSection, sections }) => {
	const modalEditId = section.id + "editModal";
	const modalDeleteId = section.id + "deleteModal";

	const closeEditModal = () => {
		const modal = document.getElementById(modalEditId) as HTMLDialogElement;
		modal.close();
	};

	const closeDeleteModal = () => {
		const modal = document.getElementById(modalDeleteId) as HTMLDialogElement;
		modal.close();
	};

	return (
		<div className="flex flex-col w-32 m-1">
			<Modal
				id={modalEditId}
				openButtonText={
					<div className="flex flex-row gap-2">
						<i className="fa-light fa-pen-to-square rounded-full p-1"></i>
						<div>Edit</div>
					</div>
				}
				openButtonStyle={"flex hover:bg-gray-200 transition duration-200 hover:cursor-pointer pl-2 rounded"}>
				<EditSectionForm section={section} editSection={editSection} close={closeEditModal} />
			</Modal>
			<Modal
				id={modalDeleteId}
				openButtonText={
					<div className="flex flex-row gap-2">
						<i className="fa-light fa-trash rounded-full p-1"></i>
						<div>Delete</div>
					</div>
				}
				openButtonStyle={"flex hover:bg-gray-200 transition duration-200 hover:cursor-pointer pl-2 rounded"}>
				<DeleteSectionForm
					section={section}
					deleteSection={deleteSection}
					sections={sections}
					close={closeDeleteModal}
				/>
			</Modal>
		</div>
	);
};

export default SectionCardMenu;
