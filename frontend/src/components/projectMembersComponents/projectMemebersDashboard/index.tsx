import Modal from "components/modal";
import { ProjectMember } from "entities/projectMember";
import { FC, useEffect, useState } from "react";
import projectMembersStore from "stores/projectMembersStore";
import InviteProjectMemberForm from "./inviteProjectMemeberForm";
import ProjectMemebersList from "../projectMembersList";
import projectInvitationsStore from "stores/projectInvitationsStore";

interface ProjectMembersDashboardProps {
	projectId?: string;
}

const idModal = "addMember";

const ProjectMembersDashboard: FC<ProjectMembersDashboardProps> = ({ projectId }) => {
	const [projectMembers, setProjectMembers] = useState<ProjectMember[]>([]);
	const [filterByName, setFilterByName] = useState("");

	useEffect(() => {
		// Load project roles when component mounts
		loadProjectMemebers();
	}, [projectId]);

	const loadProjectMemebers = async () => {
		try {
			const members = await projectMembersStore.getProjectMembersByProjectId(projectId);
			const sortedProjectMembers = members.slice().sort((a, b) => {
				const fullNameA = `${a.user.firstName} ${a.user.lastName}`;
				const fullNameB = `${b.user.firstName} ${b.user.lastName}`;
				return fullNameA.localeCompare(fullNameB);
			});

			setProjectMembers(sortedProjectMembers);
			console.log(sortedProjectMembers);
		} catch (error) {
			console.error("Error loading project roles:", error);
		}
	};

	const closeModal = () => {
		const modal = document.getElementById(idModal) as HTMLDialogElement;
		modal.close();
	};

	const inviteMember = async (email: string) => {
		try {
			if (projectId === undefined) {
				throw new Error("Can not find projectId");
			}
			await projectInvitationsStore.createProjectInvitation(email, projectId);
		} catch (error) {
			console.error(error);
		}
	};

	const editMember = async (member: ProjectMember) => {
		try {
			if (projectId === undefined) {
				throw new Error("Can not find projectId");
			}
			const updateMember = {
				id: member.id,
				projectRoleId: member.projectRole === null ? "" : member.projectRole.id
			};

			await projectMembersStore.updateProjectMember(member.id, updateMember);

			loadProjectMemebers();
		} catch (error) {
			console.error(error);
		}
	};

	const deleteMember = async (id: string) => {
		try {
			if (projectId === undefined) {
				throw new Error("Can not find projectId");
			}
			await projectMembersStore.deleteProjectMember(id);

			loadProjectMemebers();
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<div className="flex flex-col w-full justify-centerspace-y-4 h-full">
			<div className="flex justify-between">
				<input
					type="text"
					className="p-2 border rounded"
					placeholder="Filter by name"
					value={filterByName}
					onChange={e => setFilterByName(e.target.value)}
				/>
				<Modal id={idModal} openButtonText="Invite" openButtonStyle="px-4 py-2 bg-blue-500 text-white rounded">
					<InviteProjectMemberForm invite={inviteMember} close={closeModal} />
				</Modal>
			</div>
			<div className="mt-5 h-full">
				{projectMembers.length === 0 ? (
					<p className="flex text-xl italic justify-center overflow-auto">There are no members in this project.</p>
				) : (
					<ProjectMemebersList
						projectMembers={projectMembers}
						filterName={filterByName}
						editMember={editMember}
						deleteMember={deleteMember}
					/>
				)}
			</div>
		</div>
	);
};

export default ProjectMembersDashboard;
