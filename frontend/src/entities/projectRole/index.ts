import { Project } from "entities/project";

export enum ProjectRoleType {
	Admin,
	Member,
	Guest
}

export interface ProjectRole {
	id: string;
	project: Project;
	name: string;
	projectRoleType: ProjectRoleType;
}
