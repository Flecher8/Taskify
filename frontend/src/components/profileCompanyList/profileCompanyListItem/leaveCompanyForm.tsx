import { FC } from "react";
import { Company } from "entities/company";

interface LeaveCompanyFormProps {
	company: Company;
	leaveCompany: (id: string) => void;
	close: () => void;
}

const LeaveCompanyForm: FC<LeaveCompanyFormProps> = ({ company, leaveCompany, close }) => {
	const handleLeave = () => {
		leaveCompany(company.id);
		close();
	};

	return (
		<div>
			<h2 className="text-lg font-semibold">Leave Company</h2>
			<p>Are you sure you want to leave the company "{company.name}"?</p>
			<div className="mt-4 flex justify-end">
				<button className="btn btn-secondary mr-2" onClick={close}>
					Cancel
				</button>
				<button className="btn btn-danger" onClick={handleLeave}>
					Leave
				</button>
			</div>
		</div>
	);
};

export default LeaveCompanyForm;
