import ProjectsService, { Project } from "api/services/projectsService";
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
			throw new Error("Can not find projects for this user");
		}
	}

	async createProject(userId: string | null, name: string): Promise<Project> {
		try {
			if (userId === null) {
				throw new Error();
			}

			const result = await ProjectsService.create({ userId, name });
			if (result === undefined) {
				throw new Error();
			}

			return result;
		} catch (error) {
			throw new Error("Can not create new project");
		}
	}

	async updateProject(id: string, project: Project): Promise<void> {
		try {
			const result = await ProjectsService.update(id, project);

			if (result === undefined) {
				throw new Error();
			}
		} catch (error) {
			throw new Error("Can not update this project");
		}
	}
}

export default new ProjectsStore();
