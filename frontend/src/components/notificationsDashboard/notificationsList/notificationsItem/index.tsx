import { Notification, NotificationType } from "entities/notification";
import { FC } from "react";
import NotificationProjectInvitationCard from "./notificationProjectInvitationCard";
import NotificationCompanyInvitationCard from "./notificationCompanyInvitationCard";

interface NotificationsItemProps {
	notification: Notification;
	markAsRead: (id: string) => void;
}

const NotificationsItem: FC<NotificationsItemProps> = ({ notification, markAsRead }) => {
	const { notificationType, isRead } = notification;

	const handleClick = () => {
		try {
			markAsRead(notification.id);
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<div className={`flex flex-row w-full h-40 mb-2 ${!isRead ? "bg-violet-100" : "bg-white"}`}>
			<div className="flex bg-white z-[1] rounded-lg my-3 ml-3 p-1 shadow-lg min-w-60">
				{(() => {
					switch (notificationType) {
						case NotificationType.ProjectInvitation:
							return <NotificationProjectInvitationCard notification={notification} markAsRead={markAsRead} />;
						case NotificationType.CompanyInvitation:
							return <NotificationCompanyInvitationCard notification={notification} markAsRead={markAsRead} />;
						default:
							return <div>Default Notification</div>;
					}
				})()}
			</div>
			<div className="ml-3 mt-3">
				<div className="hover:bg-blue-300 p-1 transition duration-300">
					<div
						onClick={handleClick}
						className={`${
							isRead ? "hidden" : "bg-blue-500"
						} w-4 h-4 rounded-full flex justify-center items-center hover:cursor-pointer transition duration-300`}>
						{" "}
					</div>
				</div>
			</div>
		</div>
	);
};

export default NotificationsItem;
