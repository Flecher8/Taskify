import { ProjectMember } from "entities/projectMember";
import { FC } from "react";
import ProjectMembersListItem from "../projectMembersListItem";

interface ProjectMemebersListProps {
	members: ProjectMember[];
	filterName: string;
	editMember: (member: ProjectMember) => void;
	deleteMember: (id: string) => void;
}

const ProjectMemebersList: FC<ProjectMemebersListProps> = ({ members, filterName, editMember, deleteMember }) => {
	return (
		<div className="flex flex-col flex-between h-full">
			<div className="flex flex-col border-b max-h-96 overflow-auto">
				{members
					.filter(member =>
						(member.user.firstName + " " + member.user.lastName).toLowerCase().includes(filterName.toLowerCase())
					)

					.map(member => (
						<ProjectMembersListItem
							key={member.id}
							member={member}
							editMember={editMember}
							deleteMember={deleteMember}
						/>
					))}
			</div>
		</div>
	);
};

export default ProjectMemebersList;
