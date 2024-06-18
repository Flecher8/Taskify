import { Notification } from "entities/notification";
import { Project } from "entities/project";

export interface ProjectInvitation {
	id: string;
	notification: Notification;
	project: Project;
	isAccepted: boolean | null;
}
