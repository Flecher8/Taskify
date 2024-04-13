import { FC } from "react";

interface ProjectMembersMenuProps {}

const ProjectMembersMenu: FC<ProjectMembersMenuProps> = () => {
	return (
		<div className="flex flex-col items-center justify-center mt-5">
			<h2 className="text-3xl font-bold mb-6">Project memebers</h2>
			<p className="text-lg text-balance mb-4">Here you can invite new members to join your project</p>
		</div>
	);
};

export default ProjectMembersMenu;
