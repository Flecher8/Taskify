import { Project } from "entities/project";

export interface ProjectIncome {
	id: string;
	project: Project;
	amount: number;
	name: string;
	frequency: ProjectIncomeFrequency;
	createdAt: Date;
}

export enum ProjectIncomeFrequency {
	Monthly,
	Yearly
}

export const getIncomeFrequencyName = (frequency: ProjectIncomeFrequency) => {
	switch (frequency) {
		case ProjectIncomeFrequency.Monthly:
			return "Monthly";
		case ProjectIncomeFrequency.Yearly:
			return "Yearly";
		default:
			return "Unknown";
	}
};
