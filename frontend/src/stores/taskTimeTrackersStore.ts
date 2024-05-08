import TaskTimeTrackerService, {
	CreateTaskTimeTracker,
	UpdateTaskTimeTracker
} from "api/services/taskTimeTrackersService";
import { TaskTimeTracker } from "entities/taskTimeTracker";
import { makeAutoObservable } from "mobx";

class TaskTimeTrackersStore {
	constructor() {
		makeAutoObservable(this);
	}

	async createTaskTimeTracker(data: CreateTaskTimeTracker): Promise<TaskTimeTracker | undefined> {
		try {
			const result = await TaskTimeTrackerService.create(data);
			return result;
		} catch (error) {
			throw new Error(`Error creating task time tracker: ${error}`);
		}
	}

	async updateTaskTimeTracker(id: string, taskTimeTracker: UpdateTaskTimeTracker): Promise<boolean | undefined> {
		try {
			const result = await TaskTimeTrackerService.update(id, taskTimeTracker);
			return result;
		} catch (error) {
			throw new Error(`Error updating task time tracker: ${error}`);
		}
	}

	async deleteTaskTimeTracker(id: string): Promise<boolean | undefined> {
		try {
			const result = await TaskTimeTrackerService.delete(id);
			return result;
		} catch (error) {
			throw new Error(`Error deleting task time tracker: ${error}`);
		}
	}

	async getTaskTimeTrackerById(id: string): Promise<TaskTimeTracker | undefined> {
		try {
			const result = await TaskTimeTrackerService.getById(id);
			return result;
		} catch (error) {
			throw new Error(`Error fetching task time tracker: ${error}`);
		}
	}

	async startTimer(userId: string, taskId: string): Promise<TaskTimeTracker | undefined> {
		try {
			const result = await TaskTimeTrackerService.startTimer(userId, taskId);
			return result;
		} catch (error) {
			throw new Error(`Error starting timer: ${error}`);
		}
	}

	async stopTimer(userId: string, taskId: string): Promise<boolean | undefined> {
		try {
			const result = await TaskTimeTrackerService.stopTimer(userId, taskId);
			return result;
		} catch (error) {
			throw new Error(`Error stopping timer: ${error}`);
		}
	}

	async getTaskTimeTrackersByUserForTask(userId: string, taskId: string): Promise<TaskTimeTracker[] | undefined> {
		try {
			const result = await TaskTimeTrackerService.getByUserForTask(userId, taskId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching task time trackers by user for task: ${error}`);
		}
	}

	async getTaskTimeTrackersByTask(taskId: string): Promise<TaskTimeTracker[] | undefined> {
		try {
			const result = await TaskTimeTrackerService.getByTask(taskId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching task time trackers by task: ${error}`);
		}
	}

	async getNumberOfSecondsSpentOnTask(taskId: string): Promise<number | undefined> {
		try {
			const result = await TaskTimeTrackerService.getNumberOfSecondsSpentOnTask(taskId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching number of seconds spent on task: ${error}`);
		}
	}

	async getIsTimerActive(userId: string, taskId: string): Promise<TaskTimeTracker | null | undefined> {
		try {
			const result = await TaskTimeTrackerService.getIsTimerActive(userId, taskId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching active timer: ${error}`);
		}
	}
}

export default new TaskTimeTrackersStore();
