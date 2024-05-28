import PieChart from "components/charts/pie";
import { SectionTaskCountStatistics } from "entities/statistics/task/sectionTaskCountStatistics";
import { FC, useEffect, useState } from "react";
import TaskStatisticsStore from "stores/taskStatisticsStore";

interface TaskCountForSectionsStatisticsComponentProps {
	projectId: string;
}

const TaskCountForSectionsStatisticsComponent: FC<TaskCountForSectionsStatisticsComponentProps> = ({ projectId }) => {
	const [sectionTaskCount, setSectionTaskCount] = useState<{
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
			const taskStatistics = await TaskStatisticsStore.getTaskCountForSections(projectId);
			if (taskStatistics) {
				const labels = taskStatistics.map(section => section.section.name);
				const data = taskStatistics.map(section => section.count);
				const { colors, borderColors } = generateRandomRgbColors(taskStatistics.length);
				setSectionTaskCount({ labels, data, colors, borderColors });
			}
		} catch (error) {
			console.error("Error fetching task count for sections:", error);
		}
	};

	// Generate an array of random colors and border colors
	const generateRandomRgbColors = (count: number) => {
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
					labels: sectionTaskCount.labels,
					datasets: [
						{
							label: "# of Tasks",
							data: sectionTaskCount.data,
							backgroundColor: sectionTaskCount.colors,
							borderColor: sectionTaskCount.borderColors,
							borderWidth: 1
						}
					]
				}}
			/>
		</div>
	);
};

export default TaskCountForSectionsStatisticsComponent;
