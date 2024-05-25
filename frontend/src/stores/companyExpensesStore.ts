import CompanyExpensesService, { CreateCompanyExpenseDto } from "api/services/companyExpensesService";
import { CompanyExpense } from "entities/companyExpense";
import { makeAutoObservable } from "mobx";

class CompanyExpensesStore {
	constructor() {
		makeAutoObservable(this);
	}

	async getCompanyExpenseById(id: string): Promise<CompanyExpense | undefined> {
		try {
			const result = await CompanyExpensesService.getCompanyExpenseById(id);
			if (result === undefined) {
				throw new Error("Failed to fetch company expense by ID.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error fetching company expense by ID: ${error}`);
		}
	}

	async createCompanyExpense(createCompanyExpenseDto: CreateCompanyExpenseDto): Promise<CompanyExpense | undefined> {
		try {
			const result = await CompanyExpensesService.createCompanyExpense(createCompanyExpenseDto);
			if (result === undefined) {
				throw new Error("Failed to create company expense.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error creating company expense: ${error}`);
		}
	}

	async updateCompanyExpense(id: string, updateCompanyExpenseDto: CompanyExpense): Promise<void> {
		try {
			await CompanyExpensesService.updateCompanyExpense(id, updateCompanyExpenseDto);
		} catch (error) {
			throw new Error(`Error updating company expense: ${error}`);
		}
	}

	async deleteCompanyExpense(id: string): Promise<void> {
		try {
			await CompanyExpensesService.deleteCompanyExpense(id);
		} catch (error) {
			throw new Error(`Error deleting company expense: ${error}`);
		}
	}

	async getCompanyExpensesByCompanyId(companyId: string | undefined): Promise<CompanyExpense[]> {
		try {
			if (companyId === undefined) {
				throw new Error("Can not find companyId.");
			}
			const result = await CompanyExpensesService.getCompanyExpensesByCompanyId(companyId);
			if (result === undefined) {
				throw new Error("Failed to fetch company expenses by company ID.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error fetching company expenses by company ID: ${error}`);
		}
	}
}

export default new CompanyExpensesStore();
