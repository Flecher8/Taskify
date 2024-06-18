import { CompanyRole } from "entities/companyRole";
import { FC } from "react";
import CompanyRoleListItem from "./companyRoleListItem";

interface CompanyRoleListProps {
	roles: CompanyRole[];
	filterName: string;
	editRole: (role: CompanyRole) => void;
	deleteRole: (id: string) => void;
}

const CompanyRoleList: FC<CompanyRoleListProps> = ({ roles, filterName, editRole, deleteRole }) => {
	return (
		<div className="flex flex-col flex-between h-full">
			<div className="flex flex-col border-b max-h-96 overflow-auto custom-scroll-sm">
				{roles
					.filter(role => role.name.toLowerCase().includes(filterName.toLowerCase()))
					.map(role => (
						<CompanyRoleListItem key={role.id} role={role} editRole={editRole} deleteRole={deleteRole} />
					))}
			</div>
		</div>
	);
};

export default CompanyRoleList;
