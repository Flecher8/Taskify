import { ProjectRoleType } from "entities/projectRole";
import { FC, useEffect, useState } from "react";

interface CreateProjectRoleFormProps {
	create: (name: string, roleType: ProjectRoleType) => void;
	close: () => void;
}

const CreateProjectRoleForm: FC<CreateProjectRoleFormProps> = ({ create, close }) => {
	const [name, setName] = useState("");
	const [roleType, setRoleType] = useState<ProjectRoleType>(ProjectRoleType.Admin);

	const handleNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setName(event.target.value);
	};

	const handleRoleTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		setRoleType(parseInt(event.target.value) as ProjectRoleType);
	};

	const handleCreateProjectRole = () => {
		create(name, roleType);
		close();
	};

	useEffect(() => {}, [close]);

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Create Project Role</h2>
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
			<button
				className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600"
				onClick={handleCreateProjectRole}>
				Create
			</button>
		</div>
	);
};

export default CreateProjectRoleForm;
