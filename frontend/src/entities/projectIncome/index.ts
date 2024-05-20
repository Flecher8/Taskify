import { Project } from "entities/project";

export interface ProjectIncome {
	id: string;
	project: Project;
	amount: number;
	frequency: ProjectIncomeFrequency;
	createdAt: Date;
}

export enum ProjectIncomeFrequency {
	Monthly,
	Yearly
}
