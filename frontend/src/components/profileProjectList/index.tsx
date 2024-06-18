import { Project } from "entities/project";
import { FC } from "react";
import ProfileProjectListItem from "./profileProjectListItem";

interface ProfileProjectListProps {
	projects: Project[];
	leaveProject: (id: string) => void;
}

const ProfileProjectList: FC<ProfileProjectListProps> = ({ projects, leaveProject }) => {
	return (
		<div className="mt-4 max-h-[200px] overflow-x-auto custom-scroll-xs">
			{projects.length > 0 ? (
				<div className="space-y-2">
					{projects.map(project => (
						<ProfileProjectListItem key={project.id} project={project} leaveProject={leaveProject} />
					))}
				</div>
			) : (
				<div>You're not a member of any project</div>
			)}
		</div>
	);
};

export default ProfileProjectList;
