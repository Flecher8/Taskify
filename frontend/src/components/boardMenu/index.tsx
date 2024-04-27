import { FC, useEffect, useState } from "react";
import "./boardMenu.scss";
import projectsStore from "stores/projectsStore";
import { Project } from "entities/project";
import ClickToEdit from "components/clickToEditText";

interface BoardMenuProps {
	project: Project | null;
}

const BoardMenu: FC<BoardMenuProps> = ({ project }) => {
	const [projectName, setProjectName] = useState("");

	useEffect(() => {
		if (project !== null) {
			setProjectName(project.name);
		}
	}, [project]);

	const handleProjectNameChange = (newName: string) => {
		if (project) {
			project.name = newName;
			projectsStore.updateProject(project.id, project);
		}
	};

	return (
		<div className="boardMenu flex-shrink-0 overflow-auto flex flex-row justify-between items-center  border-b-stone-900">
			<div className="m-5 p-1 flex flex-row justify-start">
				<ClickToEdit initialValue={projectName} onValueChange={handleProjectNameChange} />
			</div>
			<div className="p-1 m-5 flex flex-row justify-end hover:bg-gray-300 duration-300">
				<button className="flex items-center">
					<i className="fa-light fa-ellipsis"></i>
				</button>
			</div>
		</div>
	);
};

export default BoardMenu;
