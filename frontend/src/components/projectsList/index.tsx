import ProjectCard from "components/projectCard";
import { Project } from "entities/project";
import { observer } from "mobx-react-lite";
import { FC, useEffect, useState } from "react";

interface ProjectsListProps {
	projects: Project[];
	isLoading: boolean;
}

const ProjectsList: FC<ProjectsListProps> = observer(({ projects, isLoading }) => {
	useEffect(() => {}, [projects, isLoading]);

	return (
		<div className="flex w-100">
			{isLoading ? (
				<div className="flex items-center justify-center h-96">
					<span className="loading loading-spinner text-primary"></span>
				</div>
			) : (
				<div className="flex justify-center items-center">
					{projects.length === 0 ? (
						<div className="text-center">
							<i className="far fa-magnifying-glass text-5xl text-black-500 mb-4"></i>
							<p className="text-black-500 text-xl">No projects found for your request</p>
						</div>
					) : (
						<div className="flex flex-wrap justify-start">
							{projects.map(project => (
								<div key={project.id} className="">
									<ProjectCard project={project} />
								</div>
							))}
						</div>
					)}
				</div>
			)}
		</div>
	);
});

export default ProjectsList;
