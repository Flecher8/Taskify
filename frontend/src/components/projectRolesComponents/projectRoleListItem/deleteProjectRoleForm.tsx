import { ProjectRole, ProjectRoleType } from "entities/projectRole";
import { FC, useEffect, useState } from "react";

interface DeleteProjectRoleFormProps {
	role: ProjectRole;
	deleteRole: (id: string) => void;
	close: () => void;
}

const DeleteProjectRoleForm: FC<DeleteProjectRoleFormProps> = ({ role, deleteRole, close }) => {
	const handleDeleteProjectRole = () => {
		deleteRole(role.id);
		close();
	};

	const handleCancel = () => {
		close();
	};

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Deleting Project Role</h2>
			</div>
			<div>
				<p>Are you sure?</p>
			</div>
			<div className="flex flex-row mt-5">
				<button
					className="border border-red-600 text-red-600 hover:bg-red-800 hover:text-white transition duration-300 ease-out text-white py-2 px-4 rounded mr-3"
					onClick={handleDeleteProjectRole}>
					Delete
				</button>
				<button
					className="py-2 px-4 rounded border border-white hover:border hover:border-blue-600 text-blue-600 transition duration-300 ease-out"
					onClick={handleCancel}>
					Cancel
				</button>
			</div>
		</div>
	);
};

export default DeleteProjectRoleForm;
