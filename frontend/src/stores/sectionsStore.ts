import SectionsService, { CreateSection, MoveSection } from "api/services/sectionsService";
import { Section } from "entities/section";
import { makeAutoObservable } from "mobx";
import customTaskStore from "./customTasksStore";
import customTasksStore from "./customTasksStore";

class SectionsStore {
	constructor() {
		makeAutoObservable(this);
	}

	async getSectionsByProjectId(projectId: string | undefined): Promise<Section[]> {
		try {
			if (projectId === undefined) {
				throw new Error("Can not load project.");
			}

			const newSections = await SectionsService.getSectionsByProjectId(projectId);
			if (newSections === undefined) {
				throw new Error();
			}

			// Fetch custom tasks for each section concurrently
			const customTasksPromises = newSections.map(section => customTasksStore.getCustomTasksBySectionId(section.id));
			const customTasksResults = await Promise.all(customTasksPromises);

			// Assign custom tasks to sections
			newSections.forEach((section, index) => {
				section.customTasks = customTasksResults[index];
			});

			return newSections;
		} catch (error) {
			throw new Error("Can not get sections by project.");
		}
	}

	async createSection(data: CreateSection): Promise<Section> {
		try {
			if (data === null) {
				throw new Error();
			}

			const result = await SectionsService.create(data);
			if (result === undefined) {
				throw new Error();
			}

			return result;
		} catch (error) {
			throw new Error("Can not create new section.");
		}
	}

	async updateSection(id: string, section: Section): Promise<void> {
		try {
			const result = await SectionsService.update(id, section);

			if (result === undefined) {
				throw new Error();
			}
		} catch (error) {
			throw new Error("Can not update this section.");
		}
	}

	async getSectionById(id: string | undefined): Promise<Section> {
		try {
			if (id === undefined) {
				throw new Error();
			}

			const result = await SectionsService.getById(id);

			if (result === undefined) {
				throw new Error();
			}

			return result;
		} catch (error) {
			throw new Error("Can not get project by this id.");
		}
	}

	async deleteSection(id: string): Promise<void> {
		try {
			await SectionsService.delete(id);
		} catch (error) {
			throw new Error("Can not delete this section.");
		}
	}

	async deleteRedirect(id: string, redirectId: string): Promise<void> {
		try {
			await SectionsService.deleteRedirect(id, redirectId);
		} catch (error) {
			throw new Error("Can not delete and redirect for this section.");
		}
	}

	async moveSection(data: MoveSection): Promise<void> {
		try {
			await SectionsService.move(data);
		} catch (error) {
			throw new Error("Can not move this section.");
		}
	}

	async archiveSection(id: string): Promise<void> {
		try {
			await SectionsService.archive(id);
		} catch (error) {
			throw new Error("Can not archive this section.");
		}
	}

	async unarchiveSection(id: string): Promise<void> {
		try {
			await SectionsService.unarchive(id);
		} catch (error) {
			throw new Error("Can not unarchive this section.");
		}
	}
}

export default new SectionsStore();
