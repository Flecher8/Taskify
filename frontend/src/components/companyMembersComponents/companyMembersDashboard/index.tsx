import Modal from "components/modal";
import { CompanyMember } from "entities/companyMember";
import { FC, useEffect, useState } from "react";
import companyInvitationsStore from "stores/companyInvitationsStore";
import companyMembersStore from "stores/companyMembersStore";
import InviteCompanyMemberForm from "./inviteCompanyMemberForm";
import CompanyMembersList from "./companyMembersList";

interface CompanyMembersDashboardProps {
	companyId?: string;
}

const idModal = "addMember";

const CompanyMembersDashboard: FC<CompanyMembersDashboardProps> = ({ companyId }) => {
	const [companyMembers, setCompanyMembers] = useState<CompanyMember[]>([]);
	const [filterByName, setFilterByName] = useState("");

	useEffect(() => {
		// Load company roles when component mounts
		loadCompanyMembers();
	}, [companyId]);

	const loadCompanyMembers = async () => {
		try {
			const members = await companyMembersStore.getMembersByCompanyId(companyId);
			const sortedCompanyMembers = members.slice().sort((a, b) => {
				const fullNameA = `${a.user.firstName} ${a.user.lastName}`;
				const fullNameB = `${b.user.firstName} ${b.user.lastName}`;
				return fullNameA.localeCompare(fullNameB);
			});

			setCompanyMembers(sortedCompanyMembers);
			console.log(sortedCompanyMembers);
		} catch (error) {
			console.error("Error loading company roles:", error);
		}
	};

	const closeModal = () => {
		const modal = document.getElementById(idModal) as HTMLDialogElement;
		modal.close();
	};

	const inviteMember = async (email: string) => {
		try {
			if (companyId === undefined) {
				throw new Error("Cannot find companyId");
			}
			await companyInvitationsStore.createCompanyInvitation(email, companyId);
		} catch (error) {
			console.error(error);
		}
	};

	const editMember = async (member: CompanyMember) => {
		try {
			if (companyId === undefined) {
				throw new Error("Cannot find companyId");
			}
			const updateMember = {
				id: member.id,
				roleId: member.role === null ? "" : member.role.id,
				salary: member.salary
			};

			await companyMembersStore.updateCompanyMember(member.id, updateMember);

			loadCompanyMembers();
		} catch (error) {
			console.error(error);
		}
	};

	const deleteMember = async (id: string) => {
		try {
			if (companyId === undefined) {
				throw new Error("Cannot find companyId");
			}
			await companyMembersStore.deleteCompanyMember(id);

			loadCompanyMembers();
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
					<InviteCompanyMemberForm invite={inviteMember} close={closeModal} />
				</Modal>
			</div>
			<div className="mt-5 h-full">
				{companyMembers.length === 0 ? (
					<p className="flex text-xl italic justify-center overflow-auto">There are no members in this company.</p>
				) : (
					<CompanyMembersList
						members={companyMembers}
						filterName={filterByName}
						editMember={editMember}
						deleteMember={deleteMember}
					/>
				)}
			</div>
		</div>
	);
};

export default CompanyMembersDashboard;
