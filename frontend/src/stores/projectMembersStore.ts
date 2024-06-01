import ProjectMembersService, {
	CreateProjectMemberData,
	UpdateProjectMemberData
} from "api/services/projectMembersService";
import { ProjectMember } from "entities/projectMember";
import { makeAutoObservable } from "mobx";

class ProjectMembersStore {
	constructor() {
		makeAutoObservable(this);
	}

	async createProjectMember(data: CreateProjectMemberData): Promise<ProjectMember | undefined> {
		try {
			const result = await ProjectMembersService.create(data);
			if (result === undefined) {
				throw new Error("Failed to create project member.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error creating project member.`);
		}
	}

	async updateProjectMember(id: string, data: UpdateProjectMemberData): Promise<void> {
		try {
			const result = await ProjectMembersService.update(id, data);
			if (result === undefined) {
				throw new Error("Failed to update project member.");
			}
		} catch (error) {
			throw new Error(`Error updating project member.`);
		}
	}

	async deleteProjectMember(id: string): Promise<void> {
		try {
			await ProjectMembersService.delete(id);
		} catch (error) {
			throw new Error(`Error deleting project member.`);
		}
	}

	async getProjectMemberById(id: string | undefined): Promise<ProjectMember> {
		try {
			if (id === undefined) {
				throw new Error("Invalid project member ID.");
			}

			const result = await ProjectMembersService.getById(id);
			if (result === undefined) {
				throw new Error("Project member not found.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error fetching project member.`);
		}
	}

	async getProjectMembersByProjectId(projectId: string | undefined): Promise<ProjectMember[]> {
		try {
			if (projectId === undefined) {
				throw new Error("Invalid project ID.");
			}

			const result = await ProjectMembersService.getByProjectId(projectId);
			if (result === undefined) {
				throw new Error("Project members not found.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error fetching project members.`);
		}
	}

	async leaveProject(userId: string, projectId: string): Promise<void> {
		try {
			await ProjectMembersService.leaveProject(userId, projectId);
		} catch (error) {
			throw new Error(`Error leaving project.`);
		}
	}
}

export default new ProjectMembersStore();
