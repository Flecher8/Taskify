import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { CustomTask } from "entities/customTask";

export interface CreateCustomTask {
	sectionId: string;
	name: string;
}

export interface MoveCustomTask {
	id: string;
	targetSequenceNumber: number;
}

export interface RedirectCustomTask {
	id: string;
	targetSectionId: string;
	targetSequenceNumber: number | null;
}

export interface UpdateCustomTask {
	id: string;
	responsibleUserId: string | null;
	name: string;
	description: string;
	startDateTimeUtc: Date | null;
	endDateTimeUtc: Date | null;
	storyPoints: number | null;
}

export default class CustomTaskService {
	// Create a custom task
	static async create(data: CreateCustomTask): Promise<CustomTask | undefined> {
		try {
			const response: AxiosResponse<CustomTask> = await api.post(`/api/customTasks`, data);
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

	// Update a custom task
	static async update(id: string, customTask: UpdateCustomTask): Promise<boolean | undefined> {
		try {
			const response: AxiosResponse<boolean> = await api.put(`/api/customTasks/${id}`, customTask);
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

	// Delete a custom task
	static async delete(id: string): Promise<boolean | undefined> {
		try {
			const response: AxiosResponse<boolean> = await api.delete(`/api/customTasks/${id}`);
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

	// Get a custom task by ID
	static async getById(id: string): Promise<CustomTask | undefined> {
		try {
			const response: AxiosResponse<CustomTask> = await api.get(`/api/customTasks/${id}`);
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

	// Get tasks by section ID
	static async getTasksBySectionId(sectionId: string): Promise<CustomTask[] | undefined> {
		try {
			const response: AxiosResponse<CustomTask[]> = await api.get(`/api/customTasks/section/${sectionId}`);
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

	// Move task between sections
	static async move(data: MoveCustomTask): Promise<boolean | undefined> {
		try {
			const response: AxiosResponse<boolean> = await api.patch(`/api/customTasks/move`, data);
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

	// Redirect task to another section
	static async redirect(data: RedirectCustomTask): Promise<boolean | undefined> {
		try {
			const response: AxiosResponse<boolean> = await api.patch(`/api/customTasks/redirect`, data);
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
