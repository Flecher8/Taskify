import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { CompanyExpense, CompanyExpenseFrequency } from "entities/companyExpense";

export interface CreateCompanyExpenseDto {
	companyId: string;
	name: string;
	amount: number;
	frequency: CompanyExpenseFrequency;
}

class CompanyExpensesService {
	private static baseUrl = "api/CompanyExpenses";

	static async getCompanyExpenseById(id: string): Promise<CompanyExpense | undefined> {
		try {
			const response: AxiosResponse<CompanyExpense> = await api.get(`${CompanyExpensesService.baseUrl}/${id}`);
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

	static async createCompanyExpense(
		createCompanyExpenseDto: CreateCompanyExpenseDto
	): Promise<CompanyExpense | undefined> {
		try {
			const response = await api.post(`${CompanyExpensesService.baseUrl}`, createCompanyExpenseDto);
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

	static async updateCompanyExpense(id: string, updateCompanyExpenseDto: CompanyExpense): Promise<void> {
		try {
			await api.put(`${CompanyExpensesService.baseUrl}/${id}`, updateCompanyExpenseDto);
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}

	static async deleteCompanyExpense(id: string): Promise<void> {
		try {
			await api.delete(`${CompanyExpensesService.baseUrl}/${id}`);
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}

	static async getCompanyExpensesByCompanyId(companyId: string): Promise<CompanyExpense[] | undefined> {
		try {
			const response: AxiosResponse<CompanyExpense[]> = await api.get(
				`${CompanyExpensesService.baseUrl}/company/${companyId}`
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

export default CompanyExpensesService;
