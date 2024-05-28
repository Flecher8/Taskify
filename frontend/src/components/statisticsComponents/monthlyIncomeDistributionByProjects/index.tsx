import BarChart from "components/charts/bar";
import { ProjectIncomeDistributionStatistics } from "entities/statistics/budget/projectIncomeDistributionStatistics";
import { FC, useEffect, useState } from "react";
import budgetStore from "stores/budgetStore";

interface MonthlyIncomeDistributionByProjectsProps {
	companyId: string;
}

const MonthlyIncomeDistributionByProjects: FC<MonthlyIncomeDistributionByProjectsProps> = ({ companyId }) => {
	const [incomeDistribution, setIncomeDistribution] = useState<ProjectIncomeDistributionStatistics[] | undefined>(
		undefined
	);

	useEffect(() => {
		loadData();
	}, [companyId]);

	const loadData = async () => {
		try {
			const data = await budgetStore.getMonthlyIncomeDistributionByProjects(companyId);
			setIncomeDistribution(data);
		} catch (error) {
			console.error("Error fetching monthly income distribution by projects:", error);
		}
	};

	const chartData = {
		labels: incomeDistribution?.map(item => item.project.name) || [],
		datasets: [
			{
				label: "Monthly Income Distribution",
				data: incomeDistribution?.map(item => item.totalIncome) || [],
				backgroundColor: "rgba(53, 162, 235, 0.5)" // Adjust color as needed
			}
		]
	};

	return (
		<div className="flex justify-center items-center w-full h-full">
			{incomeDistribution && <BarChart data={chartData} />}
		</div>
	);
};

export default MonthlyIncomeDistributionByProjects;
