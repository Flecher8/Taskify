import CompanyMembersDashboard from "components/companyMembersComponents/companyMembersDashboard";
import CompanyMembersMenu from "components/companyMembersComponents/companyMembersMenu";
import { FC } from "react";
import { useParams } from "react-router-dom";

interface CompanyMembersPageProps {}

const CompanyMembersPage: FC<CompanyMembersPageProps> = () => {
	const { companyId } = useParams<{ companyId: string }>();

	return (
		<div className="flex flex-col items-center w-full h-full">
			<div className="flex justify-center px-4 w-full">
				<CompanyMembersMenu />
			</div>
			<div className="flex w-full max-w-5xl h-full">
				<CompanyMembersDashboard companyId={companyId} />
			</div>
		</div>
	);
};

export default CompanyMembersPage;
