import PieChart from "components/charts/pie";
import { FC, useEffect, useState } from "react";
import TaskStatisticsStore from "stores/taskStatisticsStore";

interface TaskCountByRolesStatisticsComponentProps {
	projectId: string;
}

const TaskCountByRolesStatisticsComponent: FC<TaskCountByRolesStatisticsComponentProps> = ({ projectId }) => {
	const [roleTaskCount, setRoleTaskCount] = useState<{
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
	}, []);

	const loadData = async () => {
		try {
			const taskStatistics = await TaskStatisticsStore.getTaskCountByRolesAsync(projectId);
			if (taskStatistics) {
				const labels = taskStatistics.map(role => role.projectRole?.name ?? "No Role");
				const data = taskStatistics.map(role => role.count);
				const { colors, borderColors } = generateRandomColors(taskStatistics.length);
				setRoleTaskCount({ labels, data, colors, borderColors });
				console.log(taskStatistics);
			}
		} catch (error) {
			console.error("Error fetching task count by roles:", error);
		}
	};

	// Generate an array of random colors and border colors
	const generateRandomColors = (count: number) => {
		const colors = [];
		const borderColors = [];
		for (let i = 0; i < count; i++) {
			const rgbaColor = `rgba(${Math.floor(Math.random() * 256)}, ${Math.floor(Math.random() * 256)}, ${Math.floor(
				Math.random() * 256
			)}, 0.5)`;
			colors.push(rgbaColor);
			borderColors.push(rgbaColor.replace(", 0.2)", ", 1)")); // Change opacity to 1 for borderColor
		}
		return { colors, borderColors };
	};

	return (
		<div>
			<PieChart
				data={{
					labels: roleTaskCount.labels,
					datasets: [
						{
							label: "# of Tasks",
							data: roleTaskCount.data,
							backgroundColor: roleTaskCount.colors,
							borderColor: roleTaskCount.borderColors,
							borderWidth: 1
						}
					]
				}}
			/>
		</div>
	);
};

export default TaskCountByRolesStatisticsComponent;
