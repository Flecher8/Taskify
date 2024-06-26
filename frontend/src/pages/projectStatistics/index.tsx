import ErrorLabel from "components/errorLabel";
import ProjectStatisticsDashboard from "components/projectStatisticsDashboard";
import ProjectStatisticsMenu from "components/projectStatisticsMenu";
import { FC } from "react";
import { useParams } from "react-router-dom";

interface ProjectStatisticsPageProps {}

const ProjectStatisticsPage: FC<ProjectStatisticsPageProps> = () => {
	const { projectId } = useParams<{ projectId: string }>();

	if (projectId === undefined) {
		return <ErrorLabel message="Error: Cannot load statistics" />;
	}

	return (
		<div className="flex flex-col w-full h-full">
			<div className="flex w-full h-full overflow-hidden">
				<ProjectStatisticsDashboard projectId={projectId} />
			</div>
		</div>
	);
};

export default ProjectStatisticsPage;
