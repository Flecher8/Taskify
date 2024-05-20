import { Company } from "entities/company";

export interface CompanyMemberRole {
	id: string;
	company: Company;
	name: string;
}
