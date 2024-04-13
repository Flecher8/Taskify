import { api } from "api/axious/api";
import { ProjectInvitation } from "entities/projectInvitation";

export interface CreateProjectInvitationData {
	projectId: string;
}

export default class ProjectInvitationService {
	static async create(userId: string, data: CreateProjectInvitationData): Promise<ProjectInvitation | undefined> {
		try {
			console.log(123);
			const response = await api.post(`/api/ProjectInvitations/${userId}`, data);

			return response.data;
		} catch (error) {
			console.log(error);
			throw error;
		}
	}

	static async respond(id: string, isAccepted: boolean): Promise<boolean | undefined> {
		try {
			const response = await api.put(`/api/ProjectInvitations/${id}/accepted/${isAccepted}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getByUserId(userId: string): Promise<ProjectInvitation[] | undefined> {
		try {
			const response = await api.get(`/api/ProjectInvitations/user/${userId}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getUnreadByUserId(userId: string): Promise<ProjectInvitation[] | undefined> {
		try {
			const response = await api.get(`/api/ProjectInvitations/user/${userId}/unread`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async markAsRead(id: string): Promise<boolean | undefined> {
		try {
			const response = await api.put(`/api/ProjectInvitations/${id}/markread`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async delete(id: string): Promise<boolean | undefined> {
		try {
			const response = await api.delete(`/api/ProjectInvitations/${id}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getById(id: string): Promise<ProjectInvitation | undefined> {
		try {
			const response = await api.get(`/api/ProjectInvitations/${id}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getByNotificationId(notificationId: string): Promise<ProjectInvitation | undefined> {
		try {
			const response = await api.get(`/api/ProjectInvitations/notification/${notificationId}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}
}
