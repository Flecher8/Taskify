import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { FinancialStatistics } from "entities/statistics/budget/financialStatistics";
import { ProjectIncomeDistributionStatistics } from "entities/statistics/budget/projectIncomeDistributionStatistics";
import { RoleSalaryStatistics } from "entities/statistics/budget/roleSalaryStatistics";

export default class BudgetService {
	private static baseUrl = "api/Budget";

	static async getMonthlyIncomeStatistics(companyId: string): Promise<FinancialStatistics | undefined> {
		try {
			const response: AxiosResponse<FinancialStatistics> = await api.get(
				`${BudgetService.baseUrl}/monthly-income/${companyId}`
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

	static async getMonthlyExpenseStatistics(companyId: string): Promise<FinancialStatistics | undefined> {
		try {
			const response: AxiosResponse<FinancialStatistics> = await api.get(
				`${BudgetService.baseUrl}/monthly-expense/${companyId}`
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

	static async getYearlyIncomeStatistics(companyId: string): Promise<FinancialStatistics | undefined> {
		try {
			const response: AxiosResponse<FinancialStatistics> = await api.get(
				`${BudgetService.baseUrl}/yearly-income/${companyId}`
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

	static async getYearlyExpenseStatistics(companyId: string): Promise<FinancialStatistics | undefined> {
		try {
			const response: AxiosResponse<FinancialStatistics> = await api.get(
				`${BudgetService.baseUrl}/yearly-expense/${companyId}`
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

	static async getMonthlyIncomeDistributionByProjects(
		companyId: string
	): Promise<ProjectIncomeDistributionStatistics[] | undefined> {
		try {
			const response: AxiosResponse<ProjectIncomeDistributionStatistics[]> = await api.get(
				`${BudgetService.baseUrl}/monthly-income-distribution/${companyId}`
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

	static async getYearlyIncomeDistributionByProjects(
		companyId: string
	): Promise<ProjectIncomeDistributionStatistics[] | undefined> {
		try {
			const response: AxiosResponse<ProjectIncomeDistributionStatistics[]> = await api.get(
				`${BudgetService.baseUrl}/yearly-income-distribution/${companyId}`
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

	static async getTotalSalariesByRole(companyId: string): Promise<RoleSalaryStatistics[] | undefined> {
		try {
			const response: AxiosResponse<RoleSalaryStatistics[]> = await api.get(
				`${BudgetService.baseUrl}/total-salaries-by-role/${companyId}`
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
