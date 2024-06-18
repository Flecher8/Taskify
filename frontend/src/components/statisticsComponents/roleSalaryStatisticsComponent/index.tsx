import PieChart from "components/charts/pie";
import { RoleSalaryStatistics } from "entities/statistics/budget/roleSalaryStatistics";
import { FC, useEffect, useState } from "react";
import budgetStore from "stores/budgetStore";

interface RoleSalaryStatisticsComponentProps {
	companyId: string;
}

const RoleSalaryStatisticsComponent: FC<RoleSalaryStatisticsComponentProps> = ({ companyId }) => {
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
			const roleSalaryData = await budgetStore.getTotalSalariesByRole(companyId);

			if (roleSalaryData) {
				const labels = roleSalaryData.map(roleSalary => roleSalary.role?.name || "No role");
				const data = roleSalaryData.map(roleSalary => roleSalary.totalSalary);
				const { colors, borderColors } = generateRandomRgbColors(roleSalaryData.length);

				setStatistics({ labels, data, colors, borderColors });
			}
		} catch (error) {
			console.error("Error fetching role salary statistics:", error);
		}
	};

	const generateRandomRgbColors = (count: number) => {
		const colors = [];
		const borderColors = [];
		for (let i = 0; i < count; i++) {
			const rgbaColor = `rgba(${Math.floor(Math.random() * 256)}, ${Math.floor(Math.random() * 256)}, ${Math.floor(
				Math.random() * 256
			)}, 0.5)`;
			colors.push(rgbaColor);
			borderColors.push(rgbaColor.replace(", 0.5)", ", 1)")); // Change opacity to 1 for borderColor
		}
		return { colors, borderColors };
	};

	return (
		<div>
			<PieChart
				data={{
					labels: statistics.labels,
					datasets: [
						{
							label: "Total Salaries by Role",
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

export default RoleSalaryStatisticsComponent;
