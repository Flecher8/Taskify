import { api } from "api/axious/api";
import { ProjectRole, ProjectRoleType } from "entities/projectRole";

export interface CreateProjectRoleData {
	projectId: string;
	name: string;
	projectRoleType: ProjectRoleType;
}

export default class ProjectRolesService {
	static async create(data: CreateProjectRoleData): Promise<ProjectRole | undefined> {
		try {
			const response = await api.post(`/api/ProjectRoles`, data);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async update(id: string, data: ProjectRole): Promise<boolean | undefined> {
		try {
			const response = await api.put(`/api/ProjectRoles/${id}`, data);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async delete(id: string): Promise<boolean | undefined> {
		try {
			const response = await api.delete(`/api/ProjectRoles/${id}`);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getByProjectId(projectId: string): Promise<ProjectRole[] | undefined> {
		try {
			const response = await api.get(`/api/ProjectRoles/project/${projectId}`);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getById(id: string): Promise<ProjectRole | undefined> {
		try {
			const response = await api.get(`/api/ProjectRoles/${id}`);

			return response.data;
		} catch (error) {
			throw error;
		}
	}
}
