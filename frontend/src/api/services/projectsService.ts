import { api } from "api/axious/api";
import Result from "./result";

export interface Project {
	id: string;
	userId: string;
	name: string;
	createdAt: string;
}

interface CreateProjectData {
	userId: string;
	name: string;
}

export default class ProjectsService {
	static async getProjectsByUserId(userId: string | null): Promise<Project[] | undefined> {
		try {
			if (userId === null) {
				throw new Error("Can not find projects for this user");
			}
			const response = await api.get(`/api/Projects/user/${userId}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async create(data: CreateProjectData): Promise<Project | undefined> {
		try {
			const response = await api.post(`/api/Projects`, data);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async update(id: string, project: Project): Promise<boolean | undefined> {
		try {
			const response = await api.put(`/api/Projects/${id}`, project);
			return response.data;
		} catch (error) {
			throw error;
		}
	}
}
