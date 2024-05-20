import { Company } from "entities/company";
import { CompanyMemberRole } from "entities/companyMemberRole";
import { User } from "entities/user";

export interface CompanyMember {
	id: string;
	user: User;
	company: Company;
	companyMemberRole: CompanyMemberRole;
	salary: number;
}
