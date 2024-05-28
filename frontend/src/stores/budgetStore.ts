import BudgetService from "api/services/budgetService";
import { FinancialStatistics } from "entities/statistics/budget/financialStatistics";
import { ProjectIncomeDistributionStatistics } from "entities/statistics/budget/projectIncomeDistributionStatistics";
import { RoleSalaryStatistics } from "entities/statistics/budget/roleSalaryStatistics";
import { makeAutoObservable } from "mobx";

class BudgetStore {
	constructor() {
		makeAutoObservable(this);
	}

	async getMonthlyIncomeStatistics(companyId: string): Promise<FinancialStatistics | undefined> {
		try {
			const result = await BudgetService.getMonthlyIncomeStatistics(companyId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching monthly income statistics: ${error}`);
		}
	}

	async getMonthlyExpenseStatistics(companyId: string): Promise<FinancialStatistics | undefined> {
		try {
			const result = await BudgetService.getMonthlyExpenseStatistics(companyId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching monthly expense statistics: ${error}`);
		}
	}

	async getYearlyIncomeStatistics(companyId: string): Promise<FinancialStatistics | undefined> {
		try {
			const result = await BudgetService.getYearlyIncomeStatistics(companyId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching yearly income statistics: ${error}`);
		}
	}

	async getYearlyExpenseStatistics(companyId: string): Promise<FinancialStatistics | undefined> {
		try {
			const result = await BudgetService.getYearlyExpenseStatistics(companyId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching yearly expense statistics: ${error}`);
		}
	}

	async getMonthlyIncomeDistributionByProjects(
		companyId: string
	): Promise<ProjectIncomeDistributionStatistics[] | undefined> {
		try {
			const result = await BudgetService.getMonthlyIncomeDistributionByProjects(companyId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching monthly income distribution by projects: ${error}`);
		}
	}

	async getYearlyIncomeDistributionByProjects(
		companyId: string
	): Promise<ProjectIncomeDistributionStatistics[] | undefined> {
		try {
			const result = await BudgetService.getYearlyIncomeDistributionByProjects(companyId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching yearly income distribution by projects: ${error}`);
		}
	}

	async getTotalSalariesByRole(companyId: string): Promise<RoleSalaryStatistics[] | undefined> {
		try {
			const result = await BudgetService.getTotalSalariesByRole(companyId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching total salaries by role: ${error}`);
		}
	}
}

export default new BudgetStore();
