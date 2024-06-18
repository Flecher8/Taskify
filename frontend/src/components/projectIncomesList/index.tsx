import Loading from "components/loading";
import ProjectIncomeCard from "components/projectIncomeCard";
import { Project } from "entities/project";
import { FC, useEffect } from "react";

interface ProjectIncomesListProps {
	projects: Project[];
	isLoading: boolean;
}

const ProjectIncomesList: FC<ProjectIncomesListProps> = ({ projects, isLoading }) => {
	useEffect(() => {}, [projects, isLoading]);
	return (
		<div className="flex w-full h-full justify-center">
			{isLoading ? (
				<div className="flex items-center justify-center h-96">
					<Loading />
				</div>
			) : (
				<div className="flex w-full h-full">
					{projects.length === 0 ? (
						<div className="text-center w-full">
							<i className="far fa-magnifying-glass text-5xl text-black-500 mb-4"></i>
							<p className="text-black-500 text-xl">No projects found</p>
						</div>
					) : (
						<div className="flex flex-wrap justify-start w-full h-full flex-wrap">
							{projects.map(project => (
								<div key={project.id} className="">
									<ProjectIncomeCard project={project} />
								</div>
							))}
						</div>
					)}
				</div>
			)}
		</div>
	);
};

export default ProjectIncomesList;
