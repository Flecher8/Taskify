import { ProjectMember } from "entities/projectMember";
import { FC } from "react";

interface DeleteProjectMemberFormProps {
	member: ProjectMember;
	deleteMember: (id: string) => void;
	close: () => void;
}

const DeleteProjectMemberForm: FC<DeleteProjectMemberFormProps> = ({ member, deleteMember, close }) => {
	const handleDeleteMember = () => {
		deleteMember(member.id);
		close();
	};

	const handleCancel = () => {
		close();
	};

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Deleting Project Member</h2>
			</div>
			<div>
				<p>Are you sure?</p>
			</div>
			<div className="flex flex-row mt-5">
				<button
					className="border border-red-600 text-red-600 hover:bg-red-800 hover:text-white transition duration-300 ease-out py-2 px-4 rounded mr-3"
					onClick={handleDeleteMember}>
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

export default DeleteProjectMemberForm;
