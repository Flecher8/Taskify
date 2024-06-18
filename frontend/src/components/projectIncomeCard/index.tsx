import { Project } from "entities/project";
import { FC } from "react";
import { Link, useParams } from "react-router-dom";
import { truncateString } from "utilities/truncateString";

interface ProjectIncomeCardProps {
	project: Project;
}

const ProjectIncomeCard: FC<ProjectIncomeCardProps> = ({ project }) => {
	const { companyId } = useParams<{ companyId: string }>();

	return (
		<Link to={`/company/${companyId}/projectIncome/${project.id}`}>
			<div className="projectCard rounded-lg shadow-md p-4 mr-5 mb-5 w-36 cursor-pointer h-24">
				<h3 className="font-semibold text-base overflow-hidden overflow-ellipsis">{project.name}</h3>
			</div>
		</Link>
	);
};

export default ProjectIncomeCard;
