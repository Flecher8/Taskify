import ProjectRoleDashboard from "components/projectRoleDashboard";
import ProjectRolesMenu from "components/projectRolesMenu";
import { FC } from "react";
import { useParams } from "react-router-dom";

interface ProjectRolesPageProps {}

const ProjectRolesPage: FC<ProjectRolesPageProps> = () => {
	const { projectId } = useParams<{ projectId: string }>();

	return (
		<div className="flex flex-col items-center w-full">
			<div className="flex justify-center px-4 w-full">
				<ProjectRolesMenu />
			</div>
			<div className="flex w-full max-w-5xl">
				<ProjectRoleDashboard projectId={projectId} />
			</div>
		</div>
	);
};

export default ProjectRolesPage;
