import React from "react";

interface ProjectIncomesMenuProps {}

const ProjectIncomesMenu: React.FC<ProjectIncomesMenuProps> = () => {
	return (
		<div className="flex flex-col items-center justify-center mt-5">
			<h2 className="text-3xl font-bold mb-6">Project Incomes</h2>
			<p className="text-lg text-balance mb-4">Here you can manage incomes for your project</p>
		</div>
	);
};

export default ProjectIncomesMenu;
