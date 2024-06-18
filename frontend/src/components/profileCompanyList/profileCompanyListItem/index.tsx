import Modal from "components/modal";
import { FC } from "react";
import { Company } from "entities/company";
import LeaveCompanyForm from "./leaveCompanyForm";

interface ProfileCompanyListItemProps {
	company: Company;
	leaveCompany: (id: string) => void;
}

const idLeaveCompanyModal = "leaveCompanyModal";

const ProfileCompanyListItem: FC<ProfileCompanyListItemProps> = ({ company, leaveCompany }) => {
	const closeLeaveModal = () => {
		const modal = document.getElementById(company.id + idLeaveCompanyModal) as HTMLDialogElement;
		modal.close();
	};

	return (
		<div className="grid grid-cols-7 gap-4 border-t p-3 items-center">
			<div className="truncate col-span-6">{company.name}</div>
			<div className="flex justify-around col-span-1">
				<div>
					<Modal
						id={company.id + idLeaveCompanyModal}
						openButtonText={
							<i className="fa-light fa-right-from-bracket rounded-full hover:bg-gray-200 transition duration-300 p-1 w-10"></i>
						}
						openButtonStyle={""}>
						<LeaveCompanyForm company={company} leaveCompany={leaveCompany} close={closeLeaveModal} />
					</Modal>
				</div>
			</div>
		</div>
	);
};

export default ProfileCompanyListItem;
