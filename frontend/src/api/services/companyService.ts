import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { Company } from "entities/company";

export interface CreateCompanyDto {
	userId: string;
	name: string;
}

export default class CompanyService {
	private static baseUrl = "api/Companies";

	static async createCompany(createCompanyDto: CreateCompanyDto): Promise<Company | undefined> {
		try {
			const response: AxiosResponse<Company> = await api.post(`${CompanyService.baseUrl}`, createCompanyDto);
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

	static async getCompanyById(id: string): Promise<Company | undefined> {
		try {
			const response: AxiosResponse<Company> = await api.get(`${CompanyService.baseUrl}/${id}`);
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

	static async getCompanyByUserId(userId: string): Promise<Company | null | undefined> {
		try {
			const response: AxiosResponse<Company> = await api.get(`${CompanyService.baseUrl}/user/${userId}`);
			if (response.status === 204) {
				return null;
			}

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

	static async updateCompany(id: string, updateCompany: Company): Promise<Company | undefined> {
		try {
			const response: AxiosResponse<Company> = await api.put(`${CompanyService.baseUrl}/${id}`, updateCompany);
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

	static async deleteCompany(id: string): Promise<void> {
		try {
			await api.delete(`${CompanyService.baseUrl}/${id}`);
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
