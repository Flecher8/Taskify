import { makeAutoObservable } from "mobx";
import { CompanyRole } from "entities/companyRole";
import CompanyRolesService, { CreateCompanyRoleDto } from "api/services/companyRoleService";

class CompanyRolesStore {
	companyMemberRoles: CompanyRole[] = [];

	constructor() {
		makeAutoObservable(this);
	}

	async createCompanyRole(data: CreateCompanyRoleDto): Promise<CompanyRole | undefined> {
		try {
			const result = await CompanyRolesService.createCompanyRole(data);
			if (result) {
				this.companyMemberRoles.push(result);
			}
			return result;
		} catch (error) {
			throw new Error(`Error creating company role: ${error}`);
		}
	}

	async getCompanyRoleById(id: string): Promise<CompanyRole | undefined> {
		try {
			const result = await CompanyRolesService.getCompanyRoleById(id);
			return result;
		} catch (error) {
			throw new Error(`Error fetching company role: ${error}`);
		}
	}

	async updateCompanyRole(id: string, data: CompanyRole): Promise<CompanyRole | undefined> {
		try {
			const result = await CompanyRolesService.updateCompanyRole(id, data);
			if (result) {
				this.companyMemberRoles = this.companyMemberRoles.map(role => (role.id === id ? result : role));
			}
			return result;
		} catch (error) {
			throw new Error(`Error updating company role: ${error}`);
		}
	}

	async deleteCompanyRole(id: string): Promise<void> {
		try {
			await CompanyRolesService.deleteCompanyRole(id);
			this.companyMemberRoles = this.companyMemberRoles.filter(role => role.id !== id);
		} catch (error) {
			throw new Error(`Error deleting company role: ${error}`);
		}
	}

	async getCompanyRolesByCompanyId(companyId: string | undefined): Promise<CompanyRole[]> {
		try {
			if (companyId === undefined) {
				throw new Error("Invalid company ID.");
			}
			const result = await CompanyRolesService.getCompanyRolesByCompanyId(companyId);
			if (result === undefined) {
				throw new Error("Company roles not found.");
			}
			this.companyMemberRoles = result ?? [];
			return result;
		} catch (error) {
			throw new Error(`Error fetching roles by company ID: ${error}`);
		}
	}
}

export default new CompanyRolesStore();
