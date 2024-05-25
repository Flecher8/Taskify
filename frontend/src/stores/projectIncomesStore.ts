import ProjectIncomesService, { CreateProjectIncomeDto } from "api/services/projectIncomesService";
import { ProjectIncome } from "entities/projectIncome";
import { makeAutoObservable } from "mobx";

class ProjectIncomesStore {
	constructor() {
		makeAutoObservable(this);
	}

	async getProjectIncomesByProjectId(projectId: string | undefined): Promise<ProjectIncome[]> {
		try {
			if (projectId === undefined) {
				throw new Error("Can not find project id.");
			}

			const result = await ProjectIncomesService.getProjectIncomesByProjectId(projectId);
			if (result === undefined) {
				throw new Error("Failed to fetch project income by project ID.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error fetching project income by project ID: ${error}`);
		}
	}

	async createProjectIncome(createProjectIncomeDto: CreateProjectIncomeDto): Promise<ProjectIncome | undefined> {
		try {
			const result = await ProjectIncomesService.createProjectIncome(createProjectIncomeDto);
			if (result === undefined) {
				throw new Error("Failed to create project income.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error creating project income: ${error}`);
		}
	}

	async updateProjectIncome(id: string, updateProjectIncomeDto: ProjectIncome): Promise<void> {
		try {
			await ProjectIncomesService.updateProjectIncome(id, updateProjectIncomeDto);
		} catch (error) {
			throw new Error(`Error updating project income: ${error}`);
		}
	}

	async deleteProjectIncome(id: string): Promise<void> {
		try {
			await ProjectIncomesService.deleteProjectIncome(id);
		} catch (error) {
			throw new Error(`Error deleting project income: ${error}`);
		}
	}
}

export default new ProjectIncomesStore();
