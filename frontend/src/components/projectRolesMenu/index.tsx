import React from "react";

interface ProjectRolesMenuProps {}

const ProjectRolesMenu: React.FC<ProjectRolesMenuProps> = () => {
	return (
		<div className="flex flex-col items-center justify-center m-5">
			<h2 className="text-3xl font-bold mb-6">Project roles</h2>
			<p className="text-lg mb-4">Here you can create new project roles for your project.</p>
		</div>
	);
};

export default ProjectRolesMenu;
