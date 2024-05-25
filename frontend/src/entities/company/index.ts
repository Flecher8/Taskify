import { CompanyExpense } from "entities/companyExpense";
import { CompanyInvitation } from "entities/companyInvitation";
import { CompanyMember } from "entities/companyMember";
import { CompanyRole } from "entities/companyRole";
import { User } from "entities/user";

export interface Company {
	id: string;
	user: User;
	name: string;
	companyExpenses: CompanyExpense[];
	companyMembers: CompanyMember[];
	companyRoles: CompanyRole[];
	companyInvitations: CompanyInvitation[];
}
