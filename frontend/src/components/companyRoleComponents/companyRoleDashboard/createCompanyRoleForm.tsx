import { FC, useEffect, useState } from "react";

interface CreateCompanyRoleFormProps {
	create: (name: string) => void;
	close: () => void;
}

const CreateCompanyRoleForm: FC<CreateCompanyRoleFormProps> = ({ create, close }) => {
	const [name, setName] = useState("");

	const handleNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setName(event.target.value);
	};

	const handleCreateProjectRole = () => {
		create(name);
		close();
	};

	useEffect(() => {}, [close]);

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Create Company Role</h2>
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
			<button
				className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600"
				onClick={handleCreateProjectRole}>
				Create
			</button>
		</div>
	);
};

export default CreateCompanyRoleForm;
