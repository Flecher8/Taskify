import { ProjectRole, ProjectRoleType } from "entities/projectRole";
import { FC, useEffect, useState } from "react";

interface EditProjectRoleFormProps {
	role: ProjectRole;
	edit: (role: ProjectRole) => void;
	close: () => void;
}

const EditProjectRoleForm: FC<EditProjectRoleFormProps> = ({ role, edit, close }) => {
	const [name, setName] = useState(role.name);
	const [roleType, setRoleType] = useState<ProjectRoleType>(role.projectRoleType);

	const handleNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setName(event.target.value);
	};

	const handleRoleTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		setRoleType(parseInt(event.target.value) as ProjectRoleType);
	};

	const handleEditRole = () => {
		const editedRole = role;
		editedRole.name = name;
		editedRole.projectRoleType = roleType;
		edit(editedRole);
		close();
	};

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Edit Project Role</h2>
			</div>
			<div className="mb-4">
				<label htmlFor="name" className="block text-gray-700 font-bold mb-2">
					Name
				</label>
				<input
					type="text"
					id="name"
					className="w-full border rounded p-2"
					value={name}
					onChange={handleNameChange}
				/>
			</div>
			<div className="mb-4">
				<label htmlFor="roleType" className="block text-gray-700 font-bold mb-2">
					Role Type
				</label>
				<select
					id="roleType"
					className="w-full border rounded p-2"
					value={roleType}
					onChange={handleRoleTypeChange}>
					<option value={ProjectRoleType.Admin}>Admin</option>
					<option value={ProjectRoleType.Member}>Member</option>
					<option value={ProjectRoleType.Guest}>Guest</option>
				</select>
			</div>
			<button className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600" onClick={handleEditRole}>
				Edit
			</button>
		</div>
	);
};

export default EditProjectRoleForm;
