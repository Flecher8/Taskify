import Modal from "components/modal";
import { CompanyRole } from "entities/companyRole";
import { FC } from "react";
import EditCompanyRoleForm from "./editCompanyRoleForm";
import DeleteCompanyRoleForm from "./deleteCompanyRoleForm";

interface CompanyRoleListItemProps {
	role: CompanyRole;
	editRole: (role: CompanyRole) => void;
	deleteRole: (id: string) => void;
}

const idEditRoleModal = "editCompanyRole";
const idDeleteRoleModal = "deleteCompanyRole";

const CompanyRoleListItem: FC<CompanyRoleListItemProps> = ({ role, editRole, deleteRole }) => {
	const closeEditModal = () => {
		const modal = document.getElementById(role.id + idEditRoleModal) as HTMLDialogElement;
		modal.close();
	};

	const closeDeleteModal = () => {
		const modal = document.getElementById(role.id + idDeleteRoleModal) as HTMLDialogElement;
		modal.close();
	};

	return (
		<div className="grid grid-cols-7 gap-4 border-t p-3 items-center">
			<div className="truncate col-span-6">{role.name}</div>
			<div className="flex justify-around col-span-1">
				<div className="mr-5">
					<Modal
						id={role.id + idEditRoleModal}
						openButtonText={
							<i className="fa-light fa-pen-to-square rounded-full hover:bg-gray-200 p-1 w-10 transition duration-200"></i>
						}
						openButtonStyle={""}>
						<EditCompanyRoleForm role={role} edit={editRole} close={closeEditModal} />
					</Modal>
				</div>
				<div>
					<Modal
						id={role.id + idDeleteRoleModal}
						openButtonText={
							<i className="fa-light fa-trash rounded-full hover:bg-gray-200 p-1 w-10 transition duration-200"></i>
						}
						openButtonStyle={""}>
						<DeleteCompanyRoleForm role={role} deleteRole={deleteRole} close={closeDeleteModal} />
					</Modal>
				</div>
			</div>
		</div>
	);
};

export default CompanyRoleListItem;
