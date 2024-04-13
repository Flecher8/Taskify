import NotificationsService, { CreateNotificationData } from "api/services/notificationsService";
import { Notification } from "entities/notification";
import { makeAutoObservable } from "mobx";

class NotificationsStore {
	constructor() {
		makeAutoObservable(this);
	}

	async createNotification(data: CreateNotificationData): Promise<Notification | undefined> {
		try {
			const result = await NotificationsService.create(data);
			if (result === undefined) {
				throw new Error("Failed to create notification.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error creating notification: ${error}`);
		}
	}

	async markNotificationAsRead(id: string): Promise<boolean | undefined> {
		try {
			const result = await NotificationsService.markAsRead(id);
			if (result === undefined) {
				throw new Error("Failed to mark notification as read.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error marking notification as read: ${error}`);
		}
	}

	async getNotificationsByUserId(userId: string | null): Promise<Notification[] | undefined> {
		try {
			if (userId === null) {
				throw new Error("Can not find user ID.");
			}

			const result = await NotificationsService.getByUserId(userId);
			if (result === undefined) {
				throw new Error("Failed to fetch notifications by user ID.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error fetching notifications by user ID: ${error}`);
		}
	}

	async getNotificationById(id: string): Promise<Notification | undefined> {
		try {
			const result = await NotificationsService.getById(id);
			if (result === undefined) {
				throw new Error("Failed to fetch notification by ID.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error fetching notification by ID: ${error}`);
		}
	}

	async getUnreadNotificationsByUserId(userId: string): Promise<Notification[] | undefined> {
		try {
			const result = await NotificationsService.getUnreadByUserId(userId);
			if (result === undefined) {
				throw new Error("Failed to fetch unread notifications by user ID.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error fetching unread notifications by user ID: ${error}`);
		}
	}
}

export default new NotificationsStore();
