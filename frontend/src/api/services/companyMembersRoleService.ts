import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { CompanyMemberRole } from "entities/companyMemberRole";

export interface CreateCompanyMemberRoleDto {
	companyId: string;
	name: string;
}

export default class CompanyMemberRolesService {
	private static baseUrl = "api/CompanyMemberRoles";

	static async createCompanyMemberRole(
		createCompanyMemberRoleDto: CreateCompanyMemberRoleDto
	): Promise<CompanyMemberRole | undefined> {
		try {
			const response: AxiosResponse<CompanyMemberRole> = await api.post(
				`${CompanyMemberRolesService.baseUrl}`,
				createCompanyMemberRoleDto
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

	static async getCompanyMemberRoleById(id: string): Promise<CompanyMemberRole | undefined> {
		try {
			const response: AxiosResponse<CompanyMemberRole> = await api.get(`${CompanyMemberRolesService.baseUrl}/${id}`);
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

	static async updateCompanyMemberRole(
		id: string,
		updateCompanyMemberRole: CompanyMemberRole
	): Promise<CompanyMemberRole | undefined> {
		try {
			const response: AxiosResponse<CompanyMemberRole> = await api.put(
				`${CompanyMemberRolesService.baseUrl}/${id}`,
				updateCompanyMemberRole
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

	static async deleteCompanyMemberRole(id: string): Promise<void> {
		try {
			await api.delete(`${CompanyMemberRolesService.baseUrl}/${id}`);
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}

	static async getCompanyMemberRolesByCompanyId(companyId: string): Promise<CompanyMemberRole[] | undefined> {
		try {
			const response: AxiosResponse<CompanyMemberRole[]> = await api.get(
				`${CompanyMemberRolesService.baseUrl}/company/${companyId}`
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
}
