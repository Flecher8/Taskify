import BoardMenu from "components/boardMenu";
import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import projectsStore from "stores/projectsStore";
import "./boardPage.scss";
import { Project } from "entities/project";
import Board from "components/board";
import Loading from "components/loading";

interface BoardPageProps {}

const BoardPage: FC<BoardPageProps> = () => {
	const { projectId } = useParams<{ projectId: string }>();
	const [project, setProject] = useState<Project>();

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

	if (!project) {
		return (
			<div className="w-full h-full flex justify-center items-center">
				<Loading />
			</div>
		);
	}

	return (
		<div className="flex flex-col w-full h-full">
			<BoardMenu project={project} />
			{project === null ? (
				<div>
					<Loading />
				</div>
			) : (
				<Board project={project} />
			)}
		</div>
	);
};

export default BoardPage;
