import ErrorLabel from "components/errorLabel";
import ProjectWorkloadDashboard from "components/projectWorkloadDashboard";
import { FC } from "react";
import { useParams } from "react-router-dom";

interface ProjectWorkloadPageProps {}

const ProjectWorkloadPage: FC<ProjectWorkloadPageProps> = () => {
	const { projectId } = useParams<{ projectId: string }>();

	if (projectId === undefined) {
		return <ErrorLabel message="Error: Cannot load statistics" />;
	}

	return (
		<div className="w-full h-full">
			<ProjectWorkloadDashboard projectId={projectId} />
		</div>
	);
};

export default ProjectWorkloadPage;
