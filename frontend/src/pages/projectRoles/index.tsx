import { FC } from "react";
import { useParams } from "react-router-dom";

interface ProjectRolesPageProps {}

const ProjectRolesPage: FC<ProjectRolesPageProps> = () => {
	const { projectId } = useParams<{ projectId: string }>();

	return <div>Project Roles</div>;
};

export default ProjectRolesPage;
