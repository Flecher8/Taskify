import { maxProjectNameLength } from "../../constants";
import { FC } from "react";
import { Link } from "react-router-dom";
import { Project } from "api/services/projectsService";

interface ProjectCardProps {
	project: Project;
}

const ProjectCard: FC<ProjectCardProps> = ({ project }) => {
	const truncatedName =
		project.name.length > maxProjectNameLength ? `${project.name.slice(0, maxProjectNameLength)}...` : project.name;

	return (
		<Link to={`/board/${project.id}`}>
			<div className="projectCard rounded-lg shadow-md p-4 mr-5 mb-5 w-36 cursor-pointer h-24">
				<h3 className="font-semibold text-base overflow-hidden overflow-ellipsis">{truncatedName}</h3>
			</div>
		</Link>
	);
};

export default ProjectCard;
