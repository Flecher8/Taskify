import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { TaskTimeTracker, TaskTimeTrackerType } from "entities/taskTimeTracker";

export interface CreateTaskTimeTracker {
	customTaskId: string;
	userId: string;
	startDateTime: Date;
	endDateTime: Date | null;
	description: string;
	trackerType: TaskTimeTrackerType;
}

export interface UpdateTaskTimeTracker {
	id: string;
	startDateTime: Date;
	endDateTime: Date | null;
	description: string;
}

export default class TaskTimeTrackerService {
	// Create a task time tracker
	static async create(data: CreateTaskTimeTracker): Promise<TaskTimeTracker | undefined> {
		try {
			const response: AxiosResponse<TaskTimeTracker> = await api.post(`/api/TaskTimeTrackers`, data);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error; // Rethrow error if it's not AxiosError or doesn't have a response
		}
	}

	// Update a task time tracker
	static async update(id: string, taskTimeTracker: UpdateTaskTimeTracker): Promise<boolean | undefined> {
		try {
			const response: AxiosResponse<boolean> = await api.put(`/api/TaskTimeTrackers/${id}`, taskTimeTracker);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error; // Rethrow error if it's not AxiosError or doesn't have a response
		}
	}

	// Delete a task time tracker
	static async delete(id: string): Promise<boolean | undefined> {
		try {
			const response: AxiosResponse<boolean> = await api.delete(`/api/TaskTimeTrackers/${id}`);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error; // Rethrow error if it's not AxiosError or doesn't have a response
		}
	}

	// Get a task time tracker by ID
	static async getById(id: string): Promise<TaskTimeTracker | undefined> {
		try {
			const response: AxiosResponse<TaskTimeTracker> = await api.get(`/api/TaskTimeTrackers/${id}`);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error; // Rethrow error if it's not AxiosError or doesn't have a response
		}
	}

	// Start timer for a task
	static async startTimer(userId: string, taskId: string): Promise<TaskTimeTracker | undefined> {
		try {
			const response: AxiosResponse<TaskTimeTracker> = await api.post(
				`/api/TaskTimeTrackers/timer-start/${userId}/${taskId}`
			);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error; // Rethrow error if it's not AxiosError or doesn't have a response
		}
	}

	// Stop timer for a task
	static async stopTimer(userId: string, taskId: string): Promise<boolean | undefined> {
		try {
			const response: AxiosResponse<boolean> = await api.post(
				`/api/taskTimeTrackers/timer-stop/${userId}/${taskId}`
			);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error; // Rethrow error if it's not AxiosError or doesn't have a response
		}
	}

	// Get task time trackers by user for a task
	static async getByUserForTask(userId: string, taskId: string): Promise<TaskTimeTracker[] | undefined> {
		try {
			const response: AxiosResponse<TaskTimeTracker[]> = await api.get(
				`/api/TaskTimeTrackers/user/${userId}/task/${taskId}`
			);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error; // Rethrow error if it's not AxiosError or doesn't have a response
		}
	}

	// Get task time trackers by task
	static async getByTask(taskId: string): Promise<TaskTimeTracker[] | undefined> {
		try {
			const response: AxiosResponse<TaskTimeTracker[]> = await api.get(`/api/TaskTimeTrackers/task/${taskId}`);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error; // Rethrow error if it's not AxiosError or doesn't have a response
		}
	}

	// Get number of seconds spent on task
	static async getNumberOfSecondsSpentOnTask(taskId: string): Promise<number | undefined> {
		try {
			const response: AxiosResponse<number> = await api.get(`/api/TaskTimeTrackers/task/${taskId}/total-seconds`);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error; // Rethrow error if it's not AxiosError or doesn't have a response
		}
	}

	// Get is timer active
	static async getIsTimerActive(userId: string, taskId: string): Promise<TaskTimeTracker | null | undefined> {
		try {
			const response: AxiosResponse<TaskTimeTracker | null> = await api.get(
				`/api/TaskTimeTrackers/user/${userId}/task/${taskId}/active-timer`
			);
			if (response.status === 204) {
				return null;
			}

			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error; // Rethrow error if it's not AxiosError or doesn't have a response
		}
	}
}
