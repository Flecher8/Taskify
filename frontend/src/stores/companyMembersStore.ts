import { makeAutoObservable } from "mobx";
import { CompanyMember } from "entities/companyMember";
import CompanyMembersService, {
	CreateCompanyMemberDto,
	UpdateCompanyMemberDto
} from "api/services/companyMemberService";
import { Company } from "entities/company";

class CompanyMembersStore {
	companyMembers: CompanyMember[] = [];

	constructor() {
		makeAutoObservable(this);
	}

	async createCompanyMember(data: CreateCompanyMemberDto): Promise<CompanyMember | undefined> {
		try {
			const result = await CompanyMembersService.createCompanyMember(data);
			return result;
		} catch (error) {
			throw new Error(`Error creating company member: ${error}`);
		}
	}

	async getCompanyMemberById(id: string): Promise<CompanyMember | undefined> {
		try {
			const result = await CompanyMembersService.getCompanyMemberById(id);
			return result;
		} catch (error) {
			throw new Error(`Error fetching company member: ${error}`);
		}
	}

	async updateCompanyMember(id: string, data: UpdateCompanyMemberDto): Promise<CompanyMember | undefined> {
		try {
			const result = await CompanyMembersService.updateCompanyMember(id, data);
			return result;
		} catch (error) {
			throw new Error(`Error updating company member: ${error}`);
		}
	}

	async deleteCompanyMember(id: string): Promise<void> {
		try {
			await CompanyMembersService.deleteCompanyMember(id);
		} catch (error) {
			throw new Error(`Error deleting company member: ${error}`);
		}
	}

	async getMembersByCompanyId(companyId: string | undefined): Promise<CompanyMember[]> {
		try {
			if (companyId === undefined) {
				throw new Error("Invalid company ID.");
			}

			const result = await CompanyMembersService.getMembersByCompanyId(companyId);
			if (result === undefined) {
				throw new Error("Project members not found.");
			}
			return result;
		} catch (error) {
			throw new Error(`Error fetching members by company ID: ${error}`);
		}
	}

	async getMembersByRoleId(roleId: string): Promise<CompanyMember[] | undefined> {
		try {
			const result = await CompanyMembersService.getMembersByRoleId(roleId);
			return result;
		} catch (error) {
			throw new Error(`Error fetching members by role ID: ${error}`);
		}
	}

	async leaveCompany(userId: string, companyId: string): Promise<void> {
		try {
			await CompanyMembersService.leaveCompany(userId, companyId);
		} catch (error) {
			throw new Error(`Error deleting company member: ${error}`);
		}
	}

	async getCompaniesByUserId(userId: string): Promise<Company[]> {
		try {
			const result = await CompanyMembersService.getCompaniesByUserId(userId);
			return result;
		} catch (error) {
			throw new Error(`Error deleting company member: ${error}`);
		}
	}
}

export default new CompanyMembersStore();
