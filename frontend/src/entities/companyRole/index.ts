import { Company } from "entities/company";

export interface CompanyRole {
	id: string;
	company: Company;
	name: string;
	createdAt: Date;
}
