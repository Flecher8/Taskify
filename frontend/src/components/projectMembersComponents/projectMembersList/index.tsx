import { ProjectMember } from "entities/projectMember";
import { FC } from "react";

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
	return <div>ProjectMemebersList</div>;
};

export default ProjectMemebersList;
