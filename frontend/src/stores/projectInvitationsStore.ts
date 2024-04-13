import ProjectInvitationService, { CreateProjectInvitationData } from "api/services/projectInvitationsService";
import UserService from "api/services/userService";
import { ProjectInvitation } from "entities/projectInvitation";
import { makeAutoObservable } from "mobx";

class ProjectInvitationStore {
	constructor() {
		makeAutoObservable(this);
	}

	async createProjectInvitation(email: string, projectId: string): Promise<ProjectInvitation | undefined> {
		try {
			if (email === undefined || projectId === undefined) {
				throw new Error("Can not find email or project data.");
			}

			const userByEmail = await UserService.getUserByEmail(email);

			if (userByEmail === undefined) {
				throw new Error("Can not find user by email.");
			}

			const result = await ProjectInvitationService.create(userByEmail.id, { projectId: projectId });
			if (result === undefined) {
				throw new Error("Failed to create project invitation.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error creating project invitation: ${error}`);
		}
	}

	async respondToInvitation(id: string, isAccepted: boolean): Promise<boolean | undefined> {
		try {
			const result = await ProjectInvitationService.respond(id, isAccepted);
			if (result === undefined) {
				throw new Error("Failed to respond to project invitation.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error responding to project invitation: ${error}`);
		}
	}

	async getInvitationsByUserId(userId: string): Promise<ProjectInvitation[] | undefined> {
		try {
			const result = await ProjectInvitationService.getByUserId(userId);
			if (result === undefined) {
				throw new Error("Failed to fetch project invitations by user ID.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error fetching project invitations by user ID: ${error}`);
		}
	}

	async getUnreadInvitationsByUserId(userId: string): Promise<ProjectInvitation[] | undefined> {
		try {
			const result = await ProjectInvitationService.getUnreadByUserId(userId);
			if (result === undefined) {
				throw new Error("Failed to fetch unread project invitations by user ID.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error fetching unread project invitations by user ID: ${error}`);
		}
	}

	async markInvitationAsRead(id: string): Promise<boolean | undefined> {
		try {
			const result = await ProjectInvitationService.markAsRead(id);
			if (result === undefined) {
				throw new Error("Failed to mark project invitation as read.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error marking project invitation as read: ${error}`);
		}
	}

	async deleteInvitation(id: string): Promise<boolean | undefined> {
		try {
			const result = await ProjectInvitationService.delete(id);
			if (result === undefined) {
				throw new Error("Failed to delete project invitation.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error deleting project invitation: ${error}`);
		}
	}

	async getInvitationById(id: string): Promise<ProjectInvitation | undefined> {
		try {
			const result = await ProjectInvitationService.getById(id);
			if (result === undefined) {
				throw new Error("Failed to fetch project invitation by ID.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error fetching project invitation by ID: ${error}`);
		}
	}

	async getInvitationByNotificationId(notificationId: string): Promise<ProjectInvitation | undefined> {
		try {
			const result = await ProjectInvitationService.getByNotificationId(notificationId);
			if (result === undefined) {
				throw new Error("Failed to fetch project invitation by notification ID.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error fetching project invitation by notification ID: ${error}`);
		}
	}
}

export default new ProjectInvitationStore();
