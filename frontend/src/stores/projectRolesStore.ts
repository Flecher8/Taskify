import ProjectRolesService, { CreateProjectRoleData } from "api/services/projectRoleService";
import { ProjectRole } from "entities/projectRole";
import { makeAutoObservable } from "mobx";

class ProjectRolesStore {
	constructor() {
		makeAutoObservable(this);
	}

	async createProjectRole(data: CreateProjectRoleData): Promise<ProjectRole> {
		try {
			const result = await ProjectRolesService.create(data);
			if (result === undefined) {
				throw new Error("Failed to create project role.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error creating project role.`);
		}
	}

	async updateProjectRole(id: string, data: ProjectRole): Promise<void> {
		try {
			const result = await ProjectRolesService.update(id, data);
			if (result === undefined) {
				throw new Error("Failed to update project role.");
			}
		} catch (error) {
			throw new Error(`Error updating project role.`);
		}
	}

	async deleteProjectRole(id: string): Promise<void> {
		try {
			await ProjectRolesService.delete(id);
		} catch (error) {
			throw new Error(`Error deleting project role.`);
		}
	}

	async getProjectRoleById(id: string | undefined): Promise<ProjectRole> {
		try {
			if (id === undefined) {
				throw new Error("Invalid project role ID.");
			}

			const result = await ProjectRolesService.getById(id);
			if (result === undefined) {
				throw new Error("Project role not found.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error fetching project role.`);
		}
	}

	async getProjectRolesByProjectId(projectId: string | undefined): Promise<ProjectRole[]> {
		try {
			if (projectId === undefined) {
				throw new Error("Invalid project ID.");
			}

			const result = await ProjectRolesService.getByProjectId(projectId);
			if (result === undefined) {
				throw new Error("Project roles not found.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error fetching project roles.`);
		}
	}
}

export default new ProjectRolesStore();
