import BoardMenu from "components/boardMenu";
import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import projectsStore from "stores/projectsStore";
import "./boardPage.scss";
import { Project } from "entities/project";
import sectionsStore from "stores/sectionsStore";
import { Section, SectionType } from "entities/section";
import { CustomTask } from "entities/customTask";
import Board from "components/board";

interface BoardPageProps {}

const BoardPage: FC<BoardPageProps> = () => {
	const { projectId } = useParams<{ projectId: string }>();
	const [project, setProject] = useState<Project | null>(null);

	const laodProject = async () => {
		const newProject = await projectsStore.getProjectById(projectId);
		setProject(newProject);
		try {
		} catch (error) {
			console.error(error);
		}
	};

	useEffect(() => {
		laodProject();
	}, [projectId]);

	return (
		<div className="flex flex-col h-full">
			<BoardMenu project={project} />
			{project === null ? <div>Loading...</div> : <Board project={project} />}
		</div>
	);
};

export default BoardPage;
