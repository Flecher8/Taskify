import { CompanyMember } from "entities/companyMember";
import { FC } from "react";
import CompanyMembersListItem from "./companyMembersListItem";

interface CompanyMembersListProps {
	members: CompanyMember[];
	filterName: string;
	editMember: (member: CompanyMember) => void;
	deleteMember: (id: string) => void;
}

const CompanyMembersList: FC<CompanyMembersListProps> = ({ members, filterName, editMember, deleteMember }) => {
	return (
		<div className="flex flex-col flex-between h-full">
			<div className="flex flex-col border-b max-h-96 overflow-auto custom-scroll-sm">
				{members
					.filter(member =>
						(member.user.firstName + " " + member.user.lastName).toLowerCase().includes(filterName.toLowerCase())
					)

					.map(member => (
						<CompanyMembersListItem
							key={member.id}
							member={member}
							editMember={editMember}
							deleteMember={deleteMember}
						/>
					))}
			</div>
		</div>
	);
};

export default CompanyMembersList;
