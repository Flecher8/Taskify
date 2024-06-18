import BarChart from "components/charts/bar";
import { ProjectIncomeDistributionStatistics } from "entities/statistics/budget/projectIncomeDistributionStatistics";
import { FC, useEffect, useState } from "react";
import budgetStore from "stores/budgetStore";

interface YearlyIncomeDistributionByProjectsProps {
	companyId: string;
}

const YearlyIncomeDistributionByProjects: FC<YearlyIncomeDistributionByProjectsProps> = ({ companyId }) => {
	const [incomeDistribution, setIncomeDistribution] = useState<ProjectIncomeDistributionStatistics[] | undefined>(
		undefined
	);

	useEffect(() => {
		loadData();
	}, [companyId]);

	const loadData = async () => {
		try {
			const data = await budgetStore.getYearlyIncomeDistributionByProjects(companyId);
			setIncomeDistribution(data);
		} catch (error) {
			console.error("Error fetching yearly income distribution by projects:", error);
		}
	};

	const chartData = {
		labels: incomeDistribution?.map(item => item.project.name) || [],
		datasets: [
			{
				label: "Yearly Income Distribution",
				data: incomeDistribution?.map(item => item.totalIncome) || [],
				backgroundColor: "rgba(75, 192, 192, 0.5)" // Adjust color as needed
			}
		]
	};

	return (
		<div className="flex justify-center items-center w-full h-full">
			{incomeDistribution && <BarChart data={chartData} />}
		</div>
	);
};

export default YearlyIncomeDistributionByProjects;
