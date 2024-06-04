import KpiStatisticsComponent from "components/statisticsComponents/kpiComponent";
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
		<div className="w-full h-full flex flex-nowrap flex-col overflow-auto custom-scroll-sm ml-3 mt-3">
			<div className="flex flex-row w-full justify-around flex-wrap mb-3 gap-3">
				<div className="flex flex-1">
					<StatisticsContainer name={"Tasks status"}>
						<SectionTypeTaskCountForProjectStatisticsComponent projectId={projectId} />
					</StatisticsContainer>
				</div>
				<div className="flex flex-1">
					<StatisticsContainer name={"Tasks by sections"}>
						<TaskCountForSectionsStatisticsComponent projectId={projectId} />
					</StatisticsContainer>
				</div>
				<div className="flex flex-1">
					<StatisticsContainer name={"Tasks by roles"}>
						<TaskCountByRolesStatisticsComponent projectId={projectId} />
					</StatisticsContainer>
				</div>
			</div>
			<div className="flex mb-5">
				<StatisticsContainer name={"Tasks by users"}>
					<UserTaskCountForProjectStatisticsComponent projectId={projectId} />
				</StatisticsContainer>
			</div>
			<div className="flex mb-5">
				<StatisticsContainer name={"Story points by users"}>
					<UserStoryPointsCountForProjectStatisticsComponent projectId={projectId} />
				</StatisticsContainer>
			</div>
			<div className="flex mb-5">
				<StatisticsContainer name={"User productivity"}>
					<KpiStatisticsComponent projectId={projectId} />
				</StatisticsContainer>
			</div>
		</div>
	);
};

export default ProjectStatisticsDashboard;
