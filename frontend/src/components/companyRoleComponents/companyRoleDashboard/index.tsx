import Modal from "components/modal";
import { CompanyRole } from "entities/companyRole";
import { FC, useEffect, useState } from "react";
import companyRolesStore from "stores/companyRolesStore";
import CreateCompanyRoleForm from "./createCompanyRoleForm";
import CompanyRoleList from "./companyRoleList";

interface CompanyRoleDashboardProps {
	companyId?: string;
}

const idModal = "createRole";

const CompanyRoleDashboard: FC<CompanyRoleDashboardProps> = ({ companyId }) => {
	const [roles, setRoles] = useState<CompanyRole[]>([]);
	const [filterByName, setFilterByName] = useState("");

	useEffect(() => {
		// Load company roles when component mounts
		loadCompanyRoles();
	}, [companyId]);

	const loadCompanyRoles = async () => {
		try {
			const roles = await companyRolesStore.getCompanyRolesByCompanyId(companyId);
			const sortedCompanyRoles = roles.slice().sort((a: CompanyRole, b: CompanyRole) => {
				return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime();
			});

			setRoles(sortedCompanyRoles);
		} catch (error) {
			console.error("Error loading company roles:", error);
		}
	};

	const closeModal = () => {
		const modal = document.getElementById(idModal) as HTMLDialogElement;
		modal.close();
	};

	const createRole = async (name: string) => {
		try {
			if (companyId === undefined) {
				throw new Error("Can not find companyId");
			}
			const newRole = await companyRolesStore.createCompanyRole({
				companyId: companyId,
				name: name
			});

			loadCompanyRoles();
		} catch (error) {
			console.error(error);
		}
	};

	const editRole = async (role: CompanyRole) => {
		try {
			if (companyId === undefined) {
				throw new Error("Can not find companyId");
			}
			await companyRolesStore.updateCompanyRole(role.id, role);

			loadCompanyRoles();
		} catch (error) {
			console.error(error);
		}
	};

	const deleteRole = async (id: string) => {
		try {
			if (companyId === undefined) {
				throw new Error("Can not find companyId");
			}
			await companyRolesStore.deleteCompanyRole(id);

			loadCompanyRoles();
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
				<Modal id={idModal} openButtonText="Create" openButtonStyle="px-4 py-2 bg-blue-500 text-white rounded">
					<CreateCompanyRoleForm create={createRole} close={closeModal} />
				</Modal>
			</div>
			<div className="mt-5 h-full">
				{roles.length === 0 ? (
					<p className="flex text-xl italic justify-center overflow-auto">There are no roles in this company.</p>
				) : (
					<CompanyRoleList roles={roles} filterName={filterByName} editRole={editRole} deleteRole={deleteRole} />
				)}
			</div>
		</div>
	);
};

export default CompanyRoleDashboard;
