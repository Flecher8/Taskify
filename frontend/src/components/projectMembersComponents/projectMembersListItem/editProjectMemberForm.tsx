import { ProjectMember } from "entities/projectMember";
import { ProjectRole } from "entities/projectRole";
import { FC, useEffect, useState } from "react";
import projectRolesStore from "stores/projectRolesStore";

interface EditProjectMemberFormProps {
	member: ProjectMember;
	edit: (role: ProjectMember) => void;
	close: () => void;
}

const EditProjectMemberForm: FC<EditProjectMemberFormProps> = ({ member, edit, close }) => {
	const [roles, setRoles] = useState<ProjectRole[]>([]);
	const [selectedRole, setSelectedRole] = useState<string>(member.projectRole?.name || "No role");

	useEffect(() => {
		loadRoles();
	}, []);

	const loadRoles = async () => {
		try {
			const newRoles = await projectRolesStore.getProjectRolesByProjectId(member.project.id);
			setRoles(newRoles);
		} catch (error) {
			console.error(error);
		}
	};

	const handleRoleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		setSelectedRole(event.target.value);
	};

	const handleEditProjectMember = () => {
		let updatedRole: ProjectRole | null = null;
		if (selectedRole !== "No role") {
			updatedRole = roles.find(role => role.name === selectedRole) || null;
		}
		const updatedMember: ProjectMember = {
			...member,
			projectRole: updatedRole
		};
		if (member.projectRole !== updatedMember.projectRole) {
			edit(updatedMember);
			close();
		}
	};

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Edit Project Member</h2>
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
			<button
				className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600"
				onClick={handleEditProjectMember}>
				Edit
			</button>
		</div>
	);
};

export default EditProjectMemberForm;
