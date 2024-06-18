import PieChart from "components/charts/pie";
import { FinancialStatistics } from "entities/statistics/budget/financialStatistics";
import { FC, useEffect, useState } from "react";
import budgetStore from "stores/budgetStore";

interface YearlyIncomeExpenseComparisonProps {
	companyId: string;
}

const YearlyIncomeExpenseComparison: FC<YearlyIncomeExpenseComparisonProps> = ({ companyId }) => {
	const [statistics, setStatistics] = useState<{
		labels: string[];
		data: number[];
		colors: string[];
		borderColors: string[];
	}>({
		labels: [],
		data: [],
		colors: [],
		borderColors: []
	});

	useEffect(() => {
		loadData();
	}, [companyId]);

	const loadData = async () => {
		try {
			const [incomeData, expenseData] = await Promise.all([
				budgetStore.getYearlyIncomeStatistics(companyId),
				budgetStore.getYearlyExpenseStatistics(companyId)
			]);

			if (incomeData && expenseData) {
				const labels = ["Income", "Expenses"];
				const data = [incomeData.value, expenseData.value];
				const colors = ["rgba(75, 192, 75, 0.5)", "rgba(255, 99, 99, 0.5)"];
				const borderColors = ["rgba(75, 192, 75, 1)", "rgba(255, 99, 99, 1)"];

				setStatistics({ labels, data, colors, borderColors });
			}
		} catch (error) {
			console.error("Error fetching income and expense statistics:", error);
		}
	};

	return (
		<div>
			<PieChart
				data={{
					labels: statistics.labels,
					datasets: [
						{
							label: "Yearly Income vs Expenses",
							data: statistics.data,
							backgroundColor: statistics.colors,
							borderColor: statistics.borderColors,
							borderWidth: 1
						}
					]
				}}
			/>
		</div>
	);
};

export default YearlyIncomeExpenseComparison;
