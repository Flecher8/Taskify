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
		<div className="w-full h-full flex flex-nowrap flex-col overflow-auto custom-scroll-sm ml-3 mt-3">
			<div className="flex flex-row w-full justify-around flex-wrap mb-3 gap-3">
				<div className="flex flex-1">
					<StatisticsContainer name={"Monthly Budget"}>
						<MonthlyIncomeExpenseComparison companyId={companyId} />
					</StatisticsContainer>
				</div>
				<div className="flex flex-1">
					<StatisticsContainer name={"Yearly Budget"}>
						<YearlyIncomeExpenseComparison companyId={companyId} />
					</StatisticsContainer>
				</div>
				<div className="flex flex-1">
					<StatisticsContainer name={"Roles salary"}>
						<RoleSalaryStatisticsComponent companyId={companyId} />
					</StatisticsContainer>
				</div>
			</div>
			<div className="flex mb-5">
				<StatisticsContainer name={"Monthly Incomes"}>
					<MonthlyIncomeDistributionByProjects companyId={companyId} />
				</StatisticsContainer>
			</div>
			<div className="flex mb-5">
				<StatisticsContainer name={"Yearly Incomes"}>
					<YearlyIncomeDistributionByProjects companyId={companyId} />
				</StatisticsContainer>
			</div>
		</div>
	);
};

export default CompanyStatisticsDashboard;
