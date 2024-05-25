import { CompanyMember } from "entities/companyMember";
import { CompanyRole } from "entities/companyRole";
import { FC, useEffect, useState } from "react";
import companyRolesStore from "stores/companyRolesStore";

interface EditCompanyMemberFormProps {
	member: CompanyMember;
	edit: (member: CompanyMember) => void;
	close: () => void;
}

const EditCompanyMemberForm: FC<EditCompanyMemberFormProps> = ({ member, edit, close }) => {
	const [roles, setRoles] = useState<CompanyRole[]>([]);
	const [selectedRole, setSelectedRole] = useState<string>(member.role?.name || "No role");
	const [salary, setSalary] = useState<number>(member.salary);

	useEffect(() => {
		loadRoles();
	}, []);

	const loadRoles = async () => {
		try {
			const newRoles = await companyRolesStore.getCompanyRolesByCompanyId(member.company.id);
			setRoles(newRoles);
		} catch (error) {
			console.error(error);
		}
	};

	const handleRoleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		setSelectedRole(event.target.value);
	};

	const handleSalaryChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setSalary(parseFloat(event.target.value));
	};

	const handleEditCompanyMember = () => {
		let updatedRole: CompanyRole | null = null;
		if (selectedRole !== "No role") {
			updatedRole = roles.find(role => role.name === selectedRole) || null;
		}
		const updatedMember: CompanyMember = {
			...member,
			role: updatedRole,
			salary: salary
		};
		if (member.role !== updatedMember.role || member.salary !== updatedMember.salary) {
			edit(updatedMember);
			close();
		}
	};

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Edit Company Member</h2>
			</div>
			<div className="mb-4">
				<label htmlFor="role" className="block text-gray-700 font-bold mb-2">
					Role
				</label>
				<select id="role" className="w-full border rounded p-2" value={selectedRole} onChange={handleRoleChange}>
					<option value="No role">No role</option>
					{roles.map(role => (
						<option key={role.id} value={role.name}>
							{role.name}
						</option>
					))}
				</select>
			</div>
			<div className="mb-4">
				<label htmlFor="salary" className="block text-gray-700 font-bold mb-2">
					Salary ($)
				</label>
				<input
					type="number"
					id="salary"
					className="w-full border rounded p-2"
					value={salary}
					onChange={handleSalaryChange}
					min="0"
				/>
			</div>
			<button
				className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600"
				onClick={handleEditCompanyMember}>
				Edit
			</button>
		</div>
	);
};

export default EditCompanyMemberForm;
