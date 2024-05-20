import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { CompanyInvitation } from "entities/companyInvitation";

export interface CreateCompanyInvitationDto {
	companyId: string;
}

export default class CompanyInvitationService {
	private static baseUrl = "api/CompanyInvitations";

	static async createCompanyInvitation(
		userId: string,
		createCompanyInvitationDto: CreateCompanyInvitationDto
	): Promise<CompanyInvitation | undefined> {
		try {
			const response: AxiosResponse<CompanyInvitation> = await api.post(
				`${CompanyInvitationService.baseUrl}/${userId}`,
				createCompanyInvitationDto
			);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error;
		}
	}

	static async respondToCompanyInvitation(
		companyInvitationId: string,
		isAccepted: boolean
	): Promise<CompanyInvitation | undefined> {
		try {
			const response: AxiosResponse<CompanyInvitation> = await api.put(
				`${CompanyInvitationService.baseUrl}/${companyInvitationId}/respond/${isAccepted}`
			);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error;
		}
	}

	static async getCompanyInvitationsByUserId(userId: string): Promise<CompanyInvitation[] | undefined> {
		try {
			const response: AxiosResponse<CompanyInvitation[]> = await api.get(
				`${CompanyInvitationService.baseUrl}/user/${userId}`
			);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error;
		}
	}

	static async getUnreadCompanyInvitationsByUserId(userId: string): Promise<CompanyInvitation[] | undefined> {
		try {
			const response: AxiosResponse<CompanyInvitation[]> = await api.get(
				`${CompanyInvitationService.baseUrl}/user/${userId}/unread`
			);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error;
		}
	}

	static async markCompanyInvitationAsRead(id: string): Promise<CompanyInvitation | undefined> {
		try {
			const response: AxiosResponse<CompanyInvitation> = await api.put(
				`${CompanyInvitationService.baseUrl}/${id}/markread`
			);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error;
		}
	}

	static async deleteCompanyInvitation(id: string): Promise<void> {
		try {
			await api.delete(`${CompanyInvitationService.baseUrl}/${id}`);
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}

	static async getCompanyInvitationById(id: string): Promise<CompanyInvitation | undefined> {
		try {
			const response: AxiosResponse<CompanyInvitation> = await api.get(`${CompanyInvitationService.baseUrl}/${id}`);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error;
		}
	}
}
