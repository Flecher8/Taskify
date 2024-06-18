import CompanyStatisticsDashboard from "components/companyStatisticsDashboard";
import ErrorLabel from "components/errorLabel";
import { FC } from "react";
import { useParams } from "react-router-dom";

interface CompanyStatisticsPageProps {}

const CompanyStatisticsPage: FC<CompanyStatisticsPageProps> = () => {
	const { companyId } = useParams<{ companyId: string }>();

	if (companyId === undefined) {
		return <ErrorLabel message="Error: Cannot load statistics" />;
	}

	return (
		<div className="flex flex-col w-full h-full">
			<div className="flex w-full h-full overflow-hidden">
				<CompanyStatisticsDashboard companyId={companyId} />
			</div>
		</div>
	);
};

export default CompanyStatisticsPage;
