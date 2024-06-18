import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { KpiStatistics } from "entities/statistics/kpi";

export default class KpiStatisticsService {
	private static baseUrl = "api/KpiStatistics";

	static async getKpiForUser(projectId: string, userId: string): Promise<KpiStatistics | undefined> {
		try {
			const response: AxiosResponse<KpiStatistics> = await api.get(
				`${this.baseUrl}/project/${projectId}/user/${userId}/kpi`
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

	static async getKpiForAllUsersInProject(projectId: string): Promise<KpiStatistics[] | undefined> {
		try {
			const response: AxiosResponse<KpiStatistics[]> = await api.get(`${this.baseUrl}/project/${projectId}/kpi`);
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
}
