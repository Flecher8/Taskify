import ProjectStatisticsDashboard from "components/projectStatisticsDashboard";
import ProjectStatisticsMenu from "components/projectStatisticsMenu";
import { FC } from "react";
import { useParams } from "react-router-dom";

interface ProjectStatisticsPageProps {}

const ProjectStatisticsPage: FC<ProjectStatisticsPageProps> = () => {
	const { projectId } = useParams<{ projectId: string }>();
	return (
		<div className="flex flex-col w-full h-full">
			<div className="flex px-4 w-full">
				<ProjectStatisticsMenu />
			</div>
			<div className="flex px-4 w-full max-w-5xl h-full">
				<ProjectStatisticsDashboard projectId={projectId} />
			</div>
		</div>
	);
};

export default ProjectStatisticsPage;
