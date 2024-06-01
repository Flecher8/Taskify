import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { Company } from "entities/company";
import { CompanyMember } from "entities/companyMember";

export interface CreateCompanyMemberDto {
	userId: string;
	companyId: string;
	salary: number;
}

export interface UpdateCompanyMemberDto {
	id: string;
	roleId: string;
	salary: number;
}

export default class CompanyMembersService {
	private static baseUrl = "api/CompanyMembers";

	static async createCompanyMember(
		createCompanyMemberDto: CreateCompanyMemberDto
	): Promise<CompanyMember | undefined> {
		try {
			const response: AxiosResponse<CompanyMember> = await api.post(
				`${CompanyMembersService.baseUrl}`,
				createCompanyMemberDto
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

	static async getCompanyMemberById(id: string): Promise<CompanyMember | undefined> {
		try {
			const response: AxiosResponse<CompanyMember> = await api.get(`${CompanyMembersService.baseUrl}/${id}`);
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

	static async updateCompanyMember(
		id: string,
		updateCompanyMember: UpdateCompanyMemberDto
	): Promise<CompanyMember | undefined> {
		try {
			const response: AxiosResponse<CompanyMember> = await api.put(
				`${CompanyMembersService.baseUrl}/${id}`,
				updateCompanyMember
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

	static async deleteCompanyMember(id: string): Promise<void> {
		try {
			await api.delete(`${CompanyMembersService.baseUrl}/${id}`);
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}

	static async getMembersByCompanyId(companyId: string): Promise<CompanyMember[] | undefined> {
		try {
			const response: AxiosResponse<CompanyMember[]> = await api.get(
				`${CompanyMembersService.baseUrl}/company/${companyId}`
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

	static async getMembersByRoleId(roleId: string): Promise<CompanyMember[] | undefined> {
		try {
			const response: AxiosResponse<CompanyMember[]> = await api.get(
				`${CompanyMembersService.baseUrl}/role/${roleId}`
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

	static async leaveCompany(userId: string, companyId: string): Promise<void> {
		try {
			await api.delete(`${CompanyMembersService.baseUrl}/leave/user/${userId}/company/${companyId}`);
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}

	static async getCompaniesByUserId(userId: string): Promise<Company[]> {
		try {
			const response = await api.get(`${CompanyMembersService.baseUrl}/user/${userId}/companies`);

			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}
}
