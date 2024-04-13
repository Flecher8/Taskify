import ProjectMembersService from "api/services/projectMembersService";
import ProjectsService from "api/services/projectsService";
import { Project } from "entities/project";
import { makeAutoObservable } from "mobx";

class ProjectsStore {
	constructor() {
		makeAutoObservable(this);
	}

	async getProjectsByUserId(userId: string | null): Promise<Project[]> {
		try {
			const result = await ProjectsService.getProjectsByUserId(userId);
			if (result === undefined) {
				throw new Error();
			}

			return result;
		} catch (error) {
			throw new Error(`Can not find projects for this user: ${error}`);
		}
	}

	async createProject(userId: string | null, name: string): Promise<Project> {
		try {
			if (userId === null) {
				throw new Error("Invalid user ID.");
			}

			const result = await ProjectsService.create({ userId, name });
			if (result === undefined) {
				throw new Error();
			}

			return result;
		} catch (error) {
			throw new Error(`Can not create new project: ${error}`);
		}
	}

	async updateProject(id: string, project: Project): Promise<void> {
		try {
			const result = await ProjectsService.update(id, project);

			if (result === undefined) {
				throw new Error();
			}
		} catch (error) {
			throw new Error(`Can not update this project: ${error}`);
		}
	}

	async getProjectById(id: string | undefined): Promise<Project> {
		try {
			if (id === undefined) {
				throw new Error();
			}

			const result = await ProjectsService.getById(id);

			if (result === undefined) {
				throw new Error();
			}

			return result;
		} catch (error) {
			throw new Error(`Can not get project by this id: ${error}`);
		}
	}

	async getProjectsByMember(userId: string | null): Promise<Project[]> {
		try {
			if (userId === null) {
				throw new Error("Invalid user ID.");
			}

			const result = await ProjectMembersService.getProjectsByMember(userId);
			if (result === undefined) {
				throw new Error("Projects not found.");
			}

			return result;
		} catch (error) {
			throw new Error(`Error fetching projects by member: ${error}`);
		}
	}
}

export default new ProjectsStore();
