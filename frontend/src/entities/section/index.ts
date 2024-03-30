import { CustomTask } from "entities/customTask";
import { Project } from "entities/project";

export interface Section {
	id: string;
	project?: Project;
	name: string;
	createdAt: Date;
	sequenceNumber: number;
	sectionType: SectionType;
	isArchived: boolean;
	customTasks: CustomTask[];
}

export enum SectionType {
	ToDo,
	Doing,
	Done
}
