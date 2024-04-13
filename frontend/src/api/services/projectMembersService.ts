import { api } from "api/axious/api";
import { Project } from "entities/project";
import { ProjectMember } from "entities/projectMember";

export interface CreateProjectMemberData {
	projectId: string;
	userId: string;
}

export interface UpdateProjectMemberData {
	id: string;
	projectRoleId: string;
}

export default class ProjectMembersService {
	static async create(data: CreateProjectMemberData): Promise<ProjectMember | undefined> {
		try {
			const response = await api.post(`/api/ProjectMembers`, data);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async update(id: string, data: UpdateProjectMemberData): Promise<boolean | undefined> {
		try {
			const response = await api.put(`/api/ProjectMembers/${id}`, data);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async delete(id: string): Promise<boolean | undefined> {
		try {
			const response = await api.delete(`/api/ProjectMembers/${id}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getByProjectId(projectId: string): Promise<ProjectMember[] | undefined> {
		try {
			const response = await api.get(`/api/ProjectMembers/project/${projectId}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getById(id: string): Promise<ProjectMember | undefined> {
		try {
			const response = await api.get(`/api/ProjectMembers/${id}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getProjectsByMember(userId: string): Promise<Project[] | undefined> {
		try {
			const response = await api.get(`/api/ProjectMembers/user/${userId}/projects`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}
}
