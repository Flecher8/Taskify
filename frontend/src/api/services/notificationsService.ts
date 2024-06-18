import { api } from "api/axious/api";
import { Notification, NotificationType } from "entities/notification";

export interface CreateNotificationData {
	userId: string;
	notificationType: NotificationType;
}

export default class NotificationsService {
	static async create(data: CreateNotificationData): Promise<Notification | undefined> {
		try {
			const response = await api.post(`/api/Notifications`, data);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async markAsRead(id: string): Promise<boolean | undefined> {
		try {
			const response = await api.put(`/api/Notifications/markAsRead/${id}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getByUserId(userId: string): Promise<Notification[] | undefined> {
		try {
			const response = await api.get(`/api/Notifications/user/${userId}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getById(id: string): Promise<Notification | undefined> {
		try {
			const response = await api.get(`/api/Notifications/${id}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async getUnreadByUserId(userId: string): Promise<Notification[] | undefined> {
		try {
			const response = await api.get(`/api/Notifications/user/${userId}/unread`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}
}
