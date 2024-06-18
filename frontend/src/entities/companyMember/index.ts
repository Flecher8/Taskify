import { Company } from "entities/company";
import { CompanyRole } from "entities/companyRole";
import { User } from "entities/user";

export interface CompanyMember {
	id: string;
	user: User;
	company: Company;
	role: CompanyRole | null;
	salary: number;
}
