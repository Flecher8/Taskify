import { Project } from "api/services/projectsService";
import BoardMenu from "components/boardMenu";
import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import projectsStore from "stores/projectsStore";
import "./boardPage.scss";

interface BoardPageProps {}

const BoardPage: FC<BoardPageProps> = () => {
	const { projectId } = useParams<{ projectId: string }>();
	const [project, setProject] = useState<Project | null>(null);

	const loadData = async () => {
		const loadProject = await projectsStore.getProjectById(projectId);
		setProject(loadProject);

		try {
		} catch (error) {
			console.error(error);
		}
	};

	useEffect(() => {
		loadData();
	}, [projectId]);

	return (
		<div className="board flex flex-col">
			<BoardMenu project={project} />
			<div className="">
				{projectId}
				<i className="fa-light fa-bars"></i>
			</div>
		</div>
	);
};

export default BoardPage;
