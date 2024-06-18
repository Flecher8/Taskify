import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { CompanyRole } from "entities/companyRole";

export interface CreateCompanyRoleDto {
	companyId: string;
	name: string;
}

export default class CompanyRolesService {
	private static baseUrl = "api/CompanyRoles";

	static async createCompanyRole(createCompanyRoleDto: CreateCompanyRoleDto): Promise<CompanyRole | undefined> {
		try {
			const response: AxiosResponse<CompanyRole> = await api.post(
				`${CompanyRolesService.baseUrl}`,
				createCompanyRoleDto
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

	static async getCompanyRoleById(id: string): Promise<CompanyRole | undefined> {
		try {
			const response: AxiosResponse<CompanyRole> = await api.get(`${CompanyRolesService.baseUrl}/${id}`);
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

	static async updateCompanyRole(id: string, updateCompanyRole: CompanyRole): Promise<CompanyRole | undefined> {
		try {
			const response: AxiosResponse<CompanyRole> = await api.put(
				`${CompanyRolesService.baseUrl}/${id}`,
				updateCompanyRole
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

	static async deleteCompanyRole(id: string): Promise<void> {
		try {
			await api.delete(`${CompanyRolesService.baseUrl}/${id}`);
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}

	static async getCompanyRolesByCompanyId(companyId: string): Promise<CompanyRole[] | undefined> {
		try {
			const response: AxiosResponse<CompanyRole[]> = await api.get(
				`${CompanyRolesService.baseUrl}/company/${companyId}`
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
