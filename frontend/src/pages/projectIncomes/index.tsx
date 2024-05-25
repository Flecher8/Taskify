import ProjectIncomeDashboard from "components/projectIncomesComponents/projectIncomeDashboard";
import ProjectIncomesMenu from "components/projectIncomesComponents/projectIncomeMenu";
import { FC } from "react";
import { useParams } from "react-router-dom";

interface ProjectIncomesPageProps {}

const ProjectIncomesPage: FC<ProjectIncomesPageProps> = () => {
	const { projectId } = useParams<{ projectId: string }>();

	return (
		<div className="flex flex-col items-center w-full h-full">
			<div className="flex justify-center px-4 w-full">
				<ProjectIncomesMenu />
			</div>
			<div className="flex w-full max-w-5xl h-full">
				<ProjectIncomeDashboard projectId={projectId} />
			</div>
		</div>
	);
};

export default ProjectIncomesPage;
