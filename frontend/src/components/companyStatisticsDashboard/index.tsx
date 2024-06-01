import MonthlyIncomeDistributionByProjects from "components/statisticsComponents/monthlyIncomeDistributionByProjects";
import MonthlyIncomeExpenseComparison from "components/statisticsComponents/monthlyIncomeExpenseComparison";
import RoleSalaryStatisticsComponent from "components/statisticsComponents/roleSalaryStatisticsComponent";
import YearlyIncomeDistributionByProjects from "components/statisticsComponents/yearlyIncomeDistributionByProjects";
import YearlyIncomeExpenseComparison from "components/statisticsComponents/yearlyIncomeExpenseComparison";
import StatisticsContainer from "components/statisticsContainer";
import { FC } from "react";

interface CompanyStatisticsDashboardProps {
	companyId: string;
}

const CompanyStatisticsDashboard: FC<CompanyStatisticsDashboardProps> = ({ companyId }) => {
	return (
		<div className="w-full h-full flex flex-nowrap flex-col overflow-auto custom-scroll-sm">
			<div className="flex flex-row w-full justify-around flex-wrap mb-5 gap-3">
				<div className="flex min-w-64 h-64">
					<StatisticsContainer>
						<MonthlyIncomeExpenseComparison companyId={companyId} />
					</StatisticsContainer>
				</div>
				<div className="flex h-64">
					<StatisticsContainer>
						<YearlyIncomeExpenseComparison companyId={companyId} />
					</StatisticsContainer>
				</div>
				<div className="flex h-64">
					<StatisticsContainer>
						<RoleSalaryStatisticsComponent companyId={companyId} />
					</StatisticsContainer>
				</div>
			</div>
			<div className="flex h-64">
				<StatisticsContainer>
					<MonthlyIncomeDistributionByProjects companyId={companyId} />
				</StatisticsContainer>
			</div>
			<div className="flex h-64">
				<StatisticsContainer>
					<YearlyIncomeDistributionByProjects companyId={companyId} />
				</StatisticsContainer>
			</div>
		</div>
	);
};

export default CompanyStatisticsDashboard;
