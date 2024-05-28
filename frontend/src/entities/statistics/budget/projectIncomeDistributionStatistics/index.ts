import { Project } from "entities/project";

export interface ProjectIncomeDistributionStatistics {
	project: Project;
	totalIncome: number;
}
