import { ProjectMember } from "entities/projectMember";
import { FC } from "react";
import ProjectMembersListItem from "../projectMembersListItem";

interface ProjectMemebersListProps {
	projectMembers: ProjectMember[];
	filterName: string;
	editMember: (member: ProjectMember) => void;
	deleteMember: (id: string) => void;
}

const ProjectMemebersList: FC<ProjectMemebersListProps> = ({
	projectMembers,
	filterName,
	editMember,
	deleteMember
}) => {
	return (
		<div className="flex flex-col flex-between h-full">
			<div className="flex flex-col border-b max-h-96 overflow-auto">
				{projectMembers
					.filter(projectMember =>
						(projectMember.user.firstName + " " + projectMember.user.lastName)
							.toLowerCase()
							.includes(filterName.toLowerCase())
					)

					.map(projectMember => (
						<ProjectMembersListItem
							key={projectMember.id}
							projectMember={projectMember}
							editMember={editMember}
							deleteMember={deleteMember}
						/>
					))}
			</div>
		</div>
	);
};

export default ProjectMemebersList;
