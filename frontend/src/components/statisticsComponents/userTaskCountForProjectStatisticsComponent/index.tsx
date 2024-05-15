import BarChart from "components/charts/bar";
import { UserTaskCountStatistics } from "entities/statistics/taskStatistics/userTaskCountStatistics";
import { FC, useEffect, useState } from "react";
import TaskStatisticsStore from "stores/taskStatisticsStore";

interface UserTaskCountForProjectStatisticsComponentProps {
	projectId: string;
}

const UserTaskCountForProjectStatisticsComponent: FC<UserTaskCountForProjectStatisticsComponentProps> = ({
	projectId
}) => {
	const [userTaskCounts, setUserTaskCounts] = useState<UserTaskCountStatistics[] | undefined>(undefined);

	useEffect(() => {
		loadData();
	}, []);

	const loadData = async () => {
		try {
			const userTaskCountsData = await TaskStatisticsStore.getUserTaskCountForProjectStatistics(projectId);
			setUserTaskCounts(userTaskCountsData);
		} catch (error) {
			console.error("Error fetching user task count statistics:", error);
		}
	};

	const chartData = {
		labels:
			userTaskCounts?.map(userTaskCount => {
				if (userTaskCount.user) {
					return `${userTaskCount.user.firstName} ${userTaskCount.user.lastName}`;
				} else {
					return "Not Assigned";
				}
			}) || [], // Ensure labels is not undefined
		datasets: [
			{
				label: "User Task Counts",
				data: userTaskCounts?.map(userTaskCount => userTaskCount.count) || [], // Ensure data is not undefined
				backgroundColor: "rgba(53, 162, 235, 0.5)" // Adjust color as needed
			}
		]
	};

	return (
		<div className="flex justify-center items-center w-full h-full">
			{userTaskCounts && <BarChart data={chartData} />}
		</div>
	); // You can render user task count statistics here
};

export default UserTaskCountForProjectStatisticsComponent;
