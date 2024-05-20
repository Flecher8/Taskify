import CompanyRoleDashboard from "components/companyRoleComponents/companyRoleDashboard";
import CompanyRolesMenu from "components/companyRoleComponents/companyRolesMenu";
import { FC } from "react";
import { useParams } from "react-router-dom";

interface CompanyRolesPageProps {}

const CompanyRolesPage: FC<CompanyRolesPageProps> = () => {
	const { companyId } = useParams<{ companyId: string }>();

	return (
		<div className="flex flex-col items-center w-full h-full">
			<div className="flex justify-center px-4 w-full">
				<CompanyRolesMenu />
			</div>
			<div className="flex w-full max-w-5xl h-full">
				<CompanyRoleDashboard companyId={companyId} />
			</div>
		</div>
	);
};

export default CompanyRolesPage;
