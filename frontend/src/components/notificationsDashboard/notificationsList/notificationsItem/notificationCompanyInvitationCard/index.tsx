import { Notification } from "entities/notification";
import { FC } from "react";

interface NotificationCompanyInvitationCardProps {
	notification: Notification;
	markAsRead: (id: string) => void;
}

const NotificationCompanyInvitationCard: FC<NotificationCompanyInvitationCardProps> = ({
	notification,
	markAsRead
}) => {
	return <div>NotificationCompanyInvitationCard</div>;
};

export default NotificationCompanyInvitationCard;
