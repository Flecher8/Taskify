import BarChart from "components/charts/bar";
import { UserStoryPointsCountStatistics } from "entities/statistics/taskStatistics/userStoryPointsCountStatistics";
import { FC, useEffect, useState } from "react";
import TaskStatisticsStore from "stores/taskStatisticsStore";

interface UserStoryPointsCountForProjectStatisticsComponentProps {
	projectId: string;
}

const UserStoryPointsCountForProjectStatisticsComponent: FC<UserStoryPointsCountForProjectStatisticsComponentProps> = ({
	projectId
}) => {
	const [userStoryPointsCounts, setUserStoryPointsCounts] = useState<UserStoryPointsCountStatistics[] | undefined>(
		undefined
	);

	useEffect(() => {
		loadData();
	}, []);

	const loadData = async () => {
		try {
			const userStoryPointsCountsData = await TaskStatisticsStore.getUserStoryPointsCountForProjectStatistics(
				projectId
			);
			setUserStoryPointsCounts(userStoryPointsCountsData);
		} catch (error) {
			console.error("Error fetching user story points count statistics:", error);
		}
	};

	const chartData = {
		labels:
			userStoryPointsCounts?.map(userStoryPointsCount => {
				if (userStoryPointsCount.user) {
					return `${userStoryPointsCount.user.firstName} ${userStoryPointsCount.user.lastName}`;
				} else {
					return "Not Assigned";
				}
			}) || [], // Ensure labels is not undefined
		datasets: [
			{
				label: "User Story Points Counts",
				data: userStoryPointsCounts?.map(userStoryPointsCount => userStoryPointsCount.count) || [], // Ensure data is not undefined
				backgroundColor: "rgba(53, 162, 235, 0.5)" // Adjust color as needed
			}
		]
	};

	return (
		<div className="flex justify-center items-center w-full h-full">
			{userStoryPointsCounts && <BarChart data={chartData} />}
		</div>
	);
};

export default UserStoryPointsCountForProjectStatisticsComponent;
