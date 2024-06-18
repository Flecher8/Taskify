import TaskStatisticsService from "api/services/taskStatisticsService";
import { ProjectRoleTaskCountStatistics } from "entities/statistics/task/projectRoleTaskCountStatistics";
import { SectionTaskCountStatistics } from "entities/statistics/task/sectionTaskCountStatistics";
import { SectionTypeTaskCountStatistics } from "entities/statistics/task/sectionTypeTaskCountStatistics";
import { UserStoryPointsCountStatistics } from "entities/statistics/task/userStoryPointsCountStatistics";
import { UserTaskCountStatistics } from "entities/statistics/task/userTaskCountStatistics";
import { makeAutoObservable } from "mobx";

class TaskStatisticsStore {
	constructor() {
		makeAutoObservable(this);
	}

	async getSectionTypeTaskCountForProjectStatistics(
		projectId: string
	): Promise<SectionTypeTaskCountStatistics[] | undefined> {
		try {
			return await TaskStatisticsService.getSectionTypeTaskCountForProjectStatistics(projectId);
		} catch (error) {
			console.error("Error fetching section type task count statistics:", error);
			return undefined;
		}
	}

	async getTaskCountForSections(projectId: string): Promise<SectionTaskCountStatistics[] | undefined> {
		try {
			return await TaskStatisticsService.getTaskCountForSections(projectId);
		} catch (error) {
			console.error("Error fetching section task count statistics:", error);
			return undefined;
		}
	}

	async getUserTaskCountForProjectStatistics(projectId: string): Promise<UserTaskCountStatistics[] | undefined> {
		try {
			return await TaskStatisticsService.getUserTaskCountForProjectStatistics(projectId);
		} catch (error) {
			console.error("Error fetching user task count statistics:", error);
			return undefined;
		}
	}

	async getUserStoryPointsCountForProjectStatistics(
		projectId: string
	): Promise<UserStoryPointsCountStatistics[] | undefined> {
		try {
			return await TaskStatisticsService.getUserStoryPointsCountForProjectStatistics(projectId);
		} catch (error) {
			console.error("Error fetching user story points count statistics:", error);
			return undefined;
		}
	}

	async getTaskCountByRolesAsync(projectId: string): Promise<ProjectRoleTaskCountStatistics[] | undefined> {
		try {
			return await TaskStatisticsService.getTaskCountByRolesAsync(projectId);
		} catch (error) {
			console.error("Error fetching task count by roles statistics:", error);
			return undefined;
		}
	}
}

export default new TaskStatisticsStore();
