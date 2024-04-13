import { Notification } from "entities/notification";
import { FC, useEffect, useState } from "react";
import notificationsStore from "stores/notificationsStore";
import userStore from "stores/userStore";
import NotificationsItem from "./notificationsItem";

interface NotificationsListProps {
	showOnlyUnreadNotifications: boolean;
}

const NotificationsList: FC<NotificationsListProps> = ({ showOnlyUnreadNotifications }) => {
	const userId = userStore.userId;
	const [notifications, setNotifications] = useState<Notification[]>([]);
	const [loading, setLoading] = useState<boolean>(true);

	const loadNotifications = async () => {
		try {
			const newNotifications = await notificationsStore.getNotificationsByUserId(userId);
			if (newNotifications === undefined) {
				return;
			}
			const sortedNotifications = newNotifications.sort((a: Notification, b: Notification) => {
				return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
			});

			setNotifications(sortedNotifications);
		} catch (error) {
			console.error(error);
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		loadNotifications();
	}, [userId]);

	const markAsRead = async (id: string) => {
		try {
			// Update the notification in the notifications array
			const updatedNotifications = notifications.map(notification =>
				notification.id === id ? { ...notification, isRead: true } : notification
			);
			setNotifications(updatedNotifications);

			// Call the API to mark the notification as read
			await notificationsStore.markNotificationAsRead(id);
		} catch (error) {
			console.error("Error marking notification as read:", error);
		}
	};

	const filteredNotifications = showOnlyUnreadNotifications
		? notifications.filter(notification => !notification.isRead)
		: notifications;

	return (
		<div className="notificationsList overflow-auto">
			{loading ? (
				<div>Loading...</div>
			) : showOnlyUnreadNotifications ? (
				filteredNotifications.length === 0 ? (
					<div className="flex justify-center items-center">No unread notifications</div>
				) : (
					filteredNotifications.map(notification => (
						<NotificationsItem key={notification.id} notification={notification} markAsRead={markAsRead} />
					))
				)
			) : filteredNotifications.length === 0 ? (
				<div className="flex justify-center items-center">No notifications</div>
			) : (
				filteredNotifications.map(notification => (
					<NotificationsItem key={notification.id} notification={notification} markAsRead={markAsRead} />
				))
			)}
		</div>
	);
};

export default NotificationsList;
