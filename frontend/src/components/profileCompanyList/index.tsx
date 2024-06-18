import { Company } from "entities/company";
import { FC } from "react";
import ProfileCompanyListItem from "./profileCompanyListItem";

interface ProfileCompanyListProps {
	companies: Company[];
	leaveCompany: (id: string) => void;
}

const ProfileCompanyList: FC<ProfileCompanyListProps> = ({ companies, leaveCompany }) => {
	return (
		<div className="mt-4 max-h-[200px] overflow-x-auto custom-scroll-xs">
			{companies.length > 0 ? (
				<div className="space-y-2">
					{companies.map(company => (
						<ProfileCompanyListItem key={company.id} company={company} leaveCompany={leaveCompany} />
					))}
				</div>
			) : (
				<div>You're not a member of any company</div>
			)}
		</div>
	);
};

export default ProfileCompanyList;
