import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { ProjectIncome, ProjectIncomeFrequency } from "entities/projectIncome";

export interface CreateProjectIncomeDto {
	projectId: string;
	amount: number;
	name: string;
	frequency: ProjectIncomeFrequency;
}

export default class ProjectIncomesService {
	private static baseUrl = "api/ProjectIncomes";

	static async getProjectIncomesByProjectId(projectId: string): Promise<ProjectIncome[] | undefined> {
		try {
			const response: AxiosResponse<ProjectIncome[]> = await api.get(
				`${ProjectIncomesService.baseUrl}/project/${projectId}`
			);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error;
		}
	}

	static async createProjectIncome(
		createProjectIncomeDto: CreateProjectIncomeDto
	): Promise<ProjectIncome | undefined> {
		try {
			const response = await api.post(`${ProjectIncomesService.baseUrl}`, createProjectIncomeDto);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}

	static async updateProjectIncome(id: string, updateProjectIncomeDto: ProjectIncome): Promise<void> {
		try {
			await api.put(`${ProjectIncomesService.baseUrl}/${id}`, updateProjectIncomeDto);
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}

	static async deleteProjectIncome(id: string): Promise<void> {
		try {
			await api.delete(`${ProjectIncomesService.baseUrl}/${id}`);
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}
}
