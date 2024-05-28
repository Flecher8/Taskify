import { CompanyRole } from "entities/companyRole";

export interface RoleSalaryStatistics {
	role: CompanyRole;
	totalSalary: number;
}
