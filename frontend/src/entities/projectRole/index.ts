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
	createdAt: Date;
}

export const getRoleTypeName = (type: ProjectRoleType): string => {
	switch (type) {
		case ProjectRoleType.Admin:
			return "Admin";
		case ProjectRoleType.Guest:
			return "Guest";
		case ProjectRoleType.Member:
			return "Member";
		// Add more cases for other role types if necessary
		default:
			return "Unknown";
	}
};
