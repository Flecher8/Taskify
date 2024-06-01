import { FC, useEffect, useState } from "react";
import "./boardMenu.scss";
import projectsStore from "stores/projectsStore";
import { Project } from "entities/project";
import ClickToEdit from "components/clickToEditText";
import DropDownContext from "components/dropDownContext";
import BoardMoreMenu from "components/boardMoreMenu";
import { useNavigate } from "react-router-dom";

interface BoardMenuProps {
	project: Project;
}

const BoardMenu: FC<BoardMenuProps> = ({ project }) => {
	const [projectName, setProjectName] = useState("");
	const navigate = useNavigate(); // Use the useNavigate hook

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

	const deleteProject = async () => {
		try {
			await projectsStore.deleteProject(project.id);
			navigate("/projects");
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<div className="boardMenu flex-shrink-0 flex flex-row justify-between items-center  border-b-stone-900">
			<div className="m-5 p-1 flex flex-row justify-start">
				<ClickToEdit initialValue={projectName} onValueChange={handleProjectNameChange} />
			</div>
			<div className="p-1 m-5 flex flex-row justify-end hover:bg-gray-300 duration-300">
				<DropDownContext
					dropDownDirection={"dropdown-end"}
					openDropDownButtonContent={<i className="fa-light fa-ellipsis"></i>}
					openDropDownButtonStyle={
						"p-1 flex items-center hover:bg-gray-200 hover:cursor-pointer transition duration-300"
					}
					dropDownContentStyle={"bg-white rounded-md"}>
					<BoardMoreMenu project={project} deleteProject={deleteProject} />
				</DropDownContext>
			</div>
		</div>
	);
};

export default BoardMenu;
