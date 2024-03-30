import { FC, useEffect, useState } from "react";
import "./boardMenu.scss";
import projectsStore from "stores/projectsStore";
import { Project } from "entities/project";

interface BoardMenuProps {
	project: Project | null;
}

const BoardMenu: FC<BoardMenuProps> = ({ project }) => {
	const [projectName, setProjectName] = useState("");

	const [isHovered, setIsHovered] = useState(false);
	const [isEditable, setIsEditable] = useState(false);

	useEffect(() => {
		if (project !== null) {
			setProjectName(project.name);
		}
	}, [project]);

	const handleProjectNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setProjectName(event.target.value);
	};

	const handleInputClick = () => {
		setIsEditable(true);
	};

	const handleInputBlur = () => {
		setIsEditable(false);
		updateProject(projectName);
	};

	const updateProject = (newName: string) => {
		if (project) {
			project.name = newName;
			projectsStore.updateProject(project.id, project);
			console.log("New project");
			console.log(project);
		}
	};

	return (
		<div className="boardMenu flex flex-row justify-between items-center border-b-stone-900">
			<div className="m-5 p-1 flex flex-row justify-start">
				<div
					className={`${isHovered ? (isEditable ? "bg-white" : "bg-gray-300") : ""} duration-300`}
					onMouseEnter={() => setIsHovered(true)}
					onMouseLeave={() => setIsHovered(false)}>
					{isEditable ? (
						<input
							type="text"
							className="p-1 bg-white border border-purple-900"
							value={projectName}
							onChange={handleProjectNameChange}
							onClick={handleInputClick}
							onBlur={handleInputBlur}
							autoFocus
						/>
					) : (
						<input type="text" className="p-1 truncate" value={projectName} readOnly onClick={handleInputClick} />
					)}
				</div>
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
