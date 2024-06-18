import KpiStatisticsService from "api/services/kpiStatisticsService";

import { KpiStatistics } from "entities/statistics/kpi";
import { makeAutoObservable } from "mobx";

class KpiStatisticsStore {
	constructor() {
		makeAutoObservable(this);
	}

	async getKpiForUser(projectId: string, userId: string): Promise<KpiStatistics | undefined> {
		try {
			return await KpiStatisticsService.getKpiForUser(projectId, userId);
		} catch (error) {
			console.error("Error fetching users kpi:", error);
			return undefined;
		}
	}

	async getKpiForAllUsersInProject(projectId: string): Promise<KpiStatistics[] | undefined> {
		try {
			return await KpiStatisticsService.getKpiForAllUsersInProject(projectId);
		} catch (error) {
			console.error("Error fetching project users kpis:", error);
			return undefined;
		}
	}
}

export default new KpiStatisticsStore();
