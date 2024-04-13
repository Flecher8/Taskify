import { User } from "entities/user";

export enum NotificationType {
	ProjectInvitation,
	CompanyInvitation
}

export interface Notification {
	id: string;
	user: User;
	notificationType: NotificationType;
	createdAt: Date;
	isRead: boolean;
}
