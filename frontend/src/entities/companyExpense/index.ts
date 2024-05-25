import { Company } from "entities/company";

export interface CompanyExpense {
	id: string;
	company: Company;
	name: string;
	amount: number;
	frequency: CompanyExpenseFrequency;
	createdAt: Date;
}

export enum CompanyExpenseFrequency {
	Monthly,
	Yearly
}

export const getExpenseFrequencyName = (frequency: CompanyExpenseFrequency) => {
	switch (frequency) {
		case CompanyExpenseFrequency.Monthly:
			return "Monthly";
		case CompanyExpenseFrequency.Yearly:
			return "Yearly";
		default:
			return "Unknown";
	}
};
