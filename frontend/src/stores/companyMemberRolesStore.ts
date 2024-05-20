import { makeAutoObservable } from "mobx";
import { CompanyMemberRole } from "entities/companyMemberRole";
import CompanyMemberRolesService, { CreateCompanyMemberRoleDto } from "api/services/companyMembersRoleService";

class CompanyMemberRolesStore {
	companyMemberRoles: CompanyMemberRole[] = [];

	constructor() {
		makeAutoObservable(this);
	}

	async createCompanyMemberRole(data: CreateCompanyMemberRoleDto): Promise<CompanyMemberRole | undefined> {
		try {
			const result = await CompanyMemberRolesService.createCompanyMemberRole(data);
			if (result) {
				this.companyMemberRoles.push(result);
			}
			return result;
		} catch (error) {
			throw new Error(`Error creating company member role: ${error}`);
		}
	}

	async getCompanyMemberRoleById(id: string): Promise<CompanyMemberRole | undefined> {
		try {
			const result = await CompanyMemberRolesService.getCompanyMemberRoleById(id);
			return result;
		} catch (error) {
			throw new Error(`Error fetching company member role: ${error}`);
		}
	}

	async updateCompanyMemberRole(id: string, data: CompanyMemberRole): Promise<CompanyMemberRole | undefined> {
		try {
			const result = await CompanyMemberRolesService.updateCompanyMemberRole(id, data);
			if (result) {
				this.companyMemberRoles = this.companyMemberRoles.map(role => (role.id === id ? result : role));
			}
			return result;
		} catch (error) {
			throw new Error(`Error updating company member role: ${error}`);
		}
	}

	async deleteCompanyMemberRole(id: string): Promise<void> {
		try {
			await CompanyMemberRolesService.deleteCompanyMemberRole(id);
			this.companyMemberRoles = this.companyMemberRoles.filter(role => role.id !== id);
		} catch (error) {
			throw new Error(`Error deleting company member role: ${error}`);
		}
	}

	async getCompanyMemberRolesByCompanyId(companyId: string): Promise<CompanyMemberRole[] | undefined> {
		try {
			const result = await CompanyMemberRolesService.getCompanyMemberRolesByCompanyId(companyId);
			this.companyMemberRoles = result ?? [];
			return result;
		} catch (error) {
			throw new Error(`Error fetching roles by company ID: ${error}`);
		}
	}
}

export default new CompanyMemberRolesStore();
