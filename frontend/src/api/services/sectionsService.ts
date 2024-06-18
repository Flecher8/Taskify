import { api } from "api/axious/api";
import { Section, SectionType } from "entities/section";

export interface CreateSection {
	projectId: string;
	name: string;
}

export interface MoveSection {
	id: string;
	moveTo: number;
}

export default class SectionsService {
	static async getSectionsByProjectId(projectId: string | null): Promise<Section[] | undefined> {
		try {
			if (projectId === null) {
				throw new Error("Can not find sections for this project.");
			}
			const response = await api.get(`/api/Sections/project/${projectId}`);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async create(data: CreateSection): Promise<Section | undefined> {
		try {
			const response = await api.post(`/api/Sections`, data);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async update(id: string, section: Section): Promise<boolean | undefined> {
		try {
			const response = await api.put(`/api/Sections/${id}`, section);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async delete(id: string): Promise<boolean | undefined> {
		try {
			const response = await api.delete(`/api/Sections/${id}`);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async deleteRedirect(id: string, redirectId: string): Promise<boolean | undefined> {
		try {
			const response = await api.delete(`/api/Sections/${id}/redirect/${redirectId}`);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getById(id: string): Promise<Section | undefined> {
		try {
			const response = await api.get(`/api/Sections/${id}`);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async move(data: MoveSection): Promise<Section | undefined> {
		try {
			const response = await api.post(`/api/Sections/move`, data);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async archive(id: string): Promise<Section | undefined> {
		try {
			const response = await api.post(`/api/Sections/${id}/archive`);

			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async unarchive(id: string): Promise<Section | undefined> {
		try {
			const response = await api.post(`/api/Sections/${id}/unarchive`);

			return response.data;
		} catch (error) {
			throw error;
		}
	}
}
