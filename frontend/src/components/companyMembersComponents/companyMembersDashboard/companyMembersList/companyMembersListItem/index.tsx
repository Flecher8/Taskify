import Modal from "components/modal";
import { CompanyMember } from "entities/companyMember";
import { FC } from "react";
import EditCompanyMemberForm from "./editCompanyMemberForm";
import DeleteCompanyMemberForm from "./deleteCompanyMemberForm";

interface CompanyMembersListItemProps {
	member: CompanyMember;
	editMember: (member: CompanyMember) => void;
	deleteMember: (id: string) => void;
}

const idEditMemberModal = "editCompanyMember";
const idDeleteMemberModal = "deleteCompanyMember";

const CompanyMembersListItem: FC<CompanyMembersListItemProps> = ({ member, editMember, deleteMember }) => {
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
			<div className="truncate col-span-3">{member.user.firstName + " " + member.user.lastName}</div>
			<div className="truncate col-span-2">{member.role === null ? "No role" : member.role?.name}</div>
			<div className="truncate col-span-1">{`$${member.salary.toFixed(2)}`}</div>
			<div className="flex justify-around col-span-1">
				<div className="mr-5">
					<Modal
						id={member.id + idEditMemberModal}
						openButtonText={<i className="fa-light fa-pen-to-square rounded-full hover:bg-gray-200 p-1 w-10"></i>}
						openButtonStyle={""}>
						<EditCompanyMemberForm member={member} edit={editMember} close={closeEditModal} />
					</Modal>
				</div>
				<div>
					<Modal
						id={member.id + idDeleteMemberModal}
						openButtonText={<i className="fa-light fa-trash rounded-full hover:bg-gray-200 p-1 w-10"></i>}
						openButtonStyle={""}>
						<DeleteCompanyMemberForm member={member} deleteMember={deleteMember} close={closeDeleteModal} />
					</Modal>
				</div>
			</div>
		</div>
	);
};

export default CompanyMembersListItem;
