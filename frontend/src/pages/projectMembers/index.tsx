import ProjectMembersMenu from "components/projectMembersComponents/projectMembersMenu";
import ProjectMembersDashboard from "components/projectMembersComponents/projectMemebersDashboard";
import { FC } from "react";
import { useParams } from "react-router-dom";

interface ProjectMembersPageProps {}

const ProjectMembersPage: FC<ProjectMembersPageProps> = () => {
	const { projectId } = useParams<{ projectId: string }>();

	return (
		<div className="flex flex-col items-center w-full h-full">
			<div className="flex justify-center px-4 w-full">
				<ProjectMembersMenu />
			</div>
			<div className="flex w-full max-w-5xl h-full">
				<ProjectMembersDashboard projectId={projectId} />
			</div>
		</div>
	);
};

export default ProjectMembersPage;
