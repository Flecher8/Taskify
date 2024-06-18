import CompanyExpenseDashboard from "components/companyExpensesComponents/companyExpenseDashboard";
import CompanyExpensesMenu from "components/companyExpensesComponents/companyExpensesMenu";
import { FC } from "react";
import { useParams } from "react-router-dom";

interface CompanyExpensesPageProps {}

const CompanyExpensesPage: FC<CompanyExpensesPageProps> = () => {
	const { companyId } = useParams<{ companyId: string }>();

	return (
		<div className="flex flex-col items-center w-full h-full">
			<div className="flex justify-center px-4 w-full">
				<CompanyExpensesMenu />
			</div>
			<div className="flex w-full max-w-5xl h-full">
				<CompanyExpenseDashboard companyId={companyId} />
			</div>
		</div>
	);
};

export default CompanyExpensesPage;
