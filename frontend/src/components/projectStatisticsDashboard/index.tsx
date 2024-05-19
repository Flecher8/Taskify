import SectionTypeTaskCountForProjectStatisticsComponent from "components/statisticsComponents/sectionTypeTaskCountForProjectStatisticsComponent";
import TaskCountByRolesStatisticsComponent from "components/statisticsComponents/taskCountByRolesStatisticsComponent";
import TaskCountForSectionsStatisticsComponent from "components/statisticsComponents/taskCountForSectionsComponent";
import UserStoryPointsCountForProjectStatisticsComponent from "components/statisticsComponents/userStoryPointsCountForProjectStatisticsComponent";
import UserTaskCountForProjectStatisticsComponent from "components/statisticsComponents/userTaskCountForProjectStatisticsComponent";
import StatisticsContainer from "components/statisticsContainer";
import { FC } from "react";

interface ProjectStatisticsDashboardProps {
	projectId: string;
}

const ProjectStatisticsDashboard: FC<ProjectStatisticsDashboardProps> = ({ projectId }) => {
	return (
		<div className="w-full h-full flex flex-nowrap flex-col overflow-y-auto">
			<div className="flex flex-row w-full justify-around">
				<div className="flex min-w-64 h-64">
					<StatisticsContainer>
						<SectionTypeTaskCountForProjectStatisticsComponent projectId={projectId} />
					</StatisticsContainer>
				</div>
				<div className="flex h-64">
					<StatisticsContainer>
						<TaskCountForSectionsStatisticsComponent projectId={projectId} />
					</StatisticsContainer>
				</div>
				<div className="flex h-64">
					<StatisticsContainer>
						<TaskCountByRolesStatisticsComponent projectId={projectId} />
					</StatisticsContainer>
				</div>
			</div>
			<div className="flex h-64">
				<StatisticsContainer>
					<UserTaskCountForProjectStatisticsComponent projectId={projectId} />
				</StatisticsContainer>
			</div>
			<div className="flex h-64">
				<StatisticsContainer>
					<UserStoryPointsCountForProjectStatisticsComponent projectId={projectId} />
				</StatisticsContainer>
			</div>
		</div>
	);
};

export default ProjectStatisticsDashboard;
