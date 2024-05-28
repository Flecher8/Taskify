import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { ProjectRoleTaskCountStatistics } from "entities/statistics/task/projectRoleTaskCountStatistics";
import { SectionTaskCountStatistics } from "entities/statistics/task/sectionTaskCountStatistics";
import { SectionTypeTaskCountStatistics } from "entities/statistics/task/sectionTypeTaskCountStatistics";
import { UserStoryPointsCountStatistics } from "entities/statistics/task/userStoryPointsCountStatistics";
import { UserTaskCountStatistics } from "entities/statistics/task/userTaskCountStatistics";

export default class TaskStatisticsService {
	private static baseUrl = "api/TaskStatistics";

	static async getSectionTypeTaskCountForProjectStatistics(
		projectId: string
	): Promise<SectionTypeTaskCountStatistics[] | undefined> {
		try {
			const response: AxiosResponse<SectionTypeTaskCountStatistics[]> = await api.get(
				`${TaskStatisticsService.baseUrl}/project/${projectId}/section-type-task-count`
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

	static async getTaskCountForSections(projectId: string): Promise<SectionTaskCountStatistics[] | undefined> {
		try {
			const response: AxiosResponse<SectionTaskCountStatistics[]> = await api.get(
				`${TaskStatisticsService.baseUrl}/project/${projectId}/section-task-count`
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

	static async getUserTaskCountForProjectStatistics(
		projectId: string
	): Promise<UserTaskCountStatistics[] | undefined> {
		try {
			const response: AxiosResponse<UserTaskCountStatistics[]> = await api.get(
				`${TaskStatisticsService.baseUrl}/project/${projectId}/user-task-count`
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

	static async getUserStoryPointsCountForProjectStatistics(
		projectId: string
	): Promise<UserStoryPointsCountStatistics[] | undefined> {
		try {
			const response: AxiosResponse<UserStoryPointsCountStatistics[]> = await api.get(
				`${TaskStatisticsService.baseUrl}/project/${projectId}/user-story-points-count`
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

	static async getTaskCountByRolesAsync(projectId: string): Promise<ProjectRoleTaskCountStatistics[] | undefined> {
		try {
			const response: AxiosResponse<ProjectRoleTaskCountStatistics[]> = await api.get(
				`${TaskStatisticsService.baseUrl}/project/${projectId}/task-count-by-roles`
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
}
