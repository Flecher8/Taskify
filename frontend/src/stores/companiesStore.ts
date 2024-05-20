import { makeAutoObservable } from "mobx";
import CompanyService, { CreateCompanyDto } from "api/services/companyService";
import { Company } from "entities/company";

class CompaniesStore {
	constructor() {
		makeAutoObservable(this);
	}

	async createCompany(data: CreateCompanyDto): Promise<Company | undefined> {
		try {
			const result = await CompanyService.createCompany(data);
			return result;
		} catch (error) {
			throw new Error(`Error creating company: ${error}`);
		}
	}

	async getCompanyById(id: string): Promise<Company | undefined> {
		try {
			const result = await CompanyService.getCompanyById(id);
			return result;
		} catch (error) {
			throw new Error(`Error fetching company: ${error}`);
		}
	}

	async getCompaniesByUserId(userId: string): Promise<Company | null | undefined> {
		try {
			const result = await CompanyService.getCompanyByUserId(userId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching company by user ID: ${error}`);
		}
	}

	async updateCompany(id: string, company: Company): Promise<Company | undefined> {
		try {
			const result = await CompanyService.updateCompany(id, company);
			return result;
		} catch (error) {
			throw new Error(`Error updating company: ${error}`);
		}
	}

	async deleteCompany(id: string): Promise<void> {
		try {
			await CompanyService.deleteCompany(id);
		} catch (error) {
			throw new Error(`Error deleting company: ${error}`);
		}
	}
}

export default new CompaniesStore();
