import CustomTaskService, {
	CreateCustomTask,
	MoveCustomTask,
	RedirectCustomTask
} from "api/services/customTasksService";
import { CustomTask } from "entities/customTask";
import { makeAutoObservable } from "mobx";

class CustomTasksStore {
	constructor() {
		makeAutoObservable(this);
	}

	async createCustomTask(data: CreateCustomTask): Promise<CustomTask> {
		try {
			if (data === null) {
				throw new Error("Invalid data for creating custom task.");
			}

			const result = await CustomTaskService.create(data);
			if (result === undefined) {
				throw new Error("Failed to create custom task.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error creating custom task.`);
		}
	}

	async updateCustomTask(id: string, customTask: CustomTask): Promise<void> {
		try {
			const result = await CustomTaskService.update(id, customTask);
			if (result === undefined) {
				throw new Error("Failed to update custom task.");
			}
		} catch (error) {
			throw new Error(`Error updating custom task.`);
		}
	}

	async deleteCustomTask(id: string): Promise<void> {
		try {
			await CustomTaskService.delete(id);
		} catch (error) {
			throw new Error(`Error deleting custom task.`);
		}
	}

	async getCustomTaskById(id: string | undefined): Promise<CustomTask> {
		try {
			if (id === undefined) {
				throw new Error("Invalid custom task ID.");
			}

			const result = await CustomTaskService.getById(id);
			if (result === undefined) {
				throw new Error("Custom task not found.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error fetching custom task.`);
		}
	}

	async getCustomTasksBySectionId(sectionId: string | undefined): Promise<CustomTask[]> {
		try {
			if (sectionId === undefined) {
				throw new Error("Invalid section ID.");
			}

			const result = await CustomTaskService.getTasksBySectionId(sectionId);
			if (result === undefined) {
				throw new Error("Custom tasks not found.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error fetching custom tasks.`);
		}
	}

	async moveCustomTask(data: MoveCustomTask): Promise<boolean> {
		try {
			const result = await CustomTaskService.move(data);
			if (result === undefined) {
				throw new Error("Failed to move custom task.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error moving custom task.`);
		}
	}

	async redirectCustomTask(data: RedirectCustomTask): Promise<boolean> {
		try {
			const result = await CustomTaskService.redirect(data);
			if (result === undefined) {
				throw new Error("Failed to redirect custom task.");
			}
			return result;
		} catch (error: any) {
			if (error instanceof Error) {
				throw new Error(`Error redirecting custom task: ${error.message}`);
			} else {
				throw new Error(`Error redirecting custom task: ${String(error)}`);
			}
		}
	}
}

export default new CustomTasksStore();
