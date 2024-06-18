import { CompanyRole } from "entities/companyRole";
import { FC, useState } from "react";

interface EditCompanyRoleFormProps {
	role: CompanyRole;
	edit: (role: CompanyRole) => void;
	close: () => void;
}

const EditCompanyRoleForm: FC<EditCompanyRoleFormProps> = ({ role, edit, close }) => {
	const [name, setName] = useState(role.name);

	const handleNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setName(event.target.value);
	};

	const handleEditRole = () => {
		const editedRole = role;
		editedRole.name = name;
		edit(editedRole);
		close();
	};

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Edit Company Role</h2>
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
			<button className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600" onClick={handleEditRole}>
				Edit
			</button>
		</div>
	);
};

export default EditCompanyRoleForm;
