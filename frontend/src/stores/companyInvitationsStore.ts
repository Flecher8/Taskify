import { makeAutoObservable } from "mobx";
import { CompanyInvitation } from "entities/companyInvitation";
import CompanyInvitationService, { CreateCompanyInvitationDto } from "api/services/companyInvitationsService";

class CompanyInvitationsStore {
	constructor() {
		makeAutoObservable(this);
	}

	async createCompanyInvitation(
		userId: string,
		data: CreateCompanyInvitationDto
	): Promise<CompanyInvitation | undefined> {
		try {
			const result = await CompanyInvitationService.createCompanyInvitation(userId, data);
			return result;
		} catch (error) {
			throw new Error(`Error creating company invitation: ${error}`);
		}
	}

	async respondToCompanyInvitation(
		companyInvitationId: string,
		isAccepted: boolean
	): Promise<CompanyInvitation | undefined> {
		try {
			const result = await CompanyInvitationService.respondToCompanyInvitation(companyInvitationId, isAccepted);
			return result;
		} catch (error) {
			throw new Error(`Error responding to company invitation: ${error}`);
		}
	}

	async getCompanyInvitationsByUserId(userId: string): Promise<CompanyInvitation[] | undefined> {
		try {
			const result = await CompanyInvitationService.getCompanyInvitationsByUserId(userId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching company invitations by user ID: ${error}`);
		}
	}

	async getUnreadCompanyInvitationsByUserId(userId: string): Promise<CompanyInvitation[] | undefined> {
		try {
			const result = await CompanyInvitationService.getUnreadCompanyInvitationsByUserId(userId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching unread company invitations by user ID: ${error}`);
		}
	}

	async markCompanyInvitationAsRead(id: string): Promise<CompanyInvitation | undefined> {
		try {
			const result = await CompanyInvitationService.markCompanyInvitationAsRead(id);
			return result;
		} catch (error) {
			throw new Error(`Error marking company invitation as read: ${error}`);
		}
	}

	async deleteCompanyInvitation(id: string): Promise<void> {
		try {
			await CompanyInvitationService.deleteCompanyInvitation(id);
		} catch (error) {
			throw new Error(`Error deleting company invitation: ${error}`);
		}
	}

	async getCompanyInvitationById(id: string): Promise<CompanyInvitation | undefined> {
		try {
			const result = await CompanyInvitationService.getCompanyInvitationById(id);
			return result;
		} catch (error) {
			throw new Error(`Error fetching company invitation by ID: ${error}`);
		}
	}
}

export default new CompanyInvitationsStore();
