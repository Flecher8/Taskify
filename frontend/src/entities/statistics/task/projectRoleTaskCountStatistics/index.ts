import { ProjectRole } from "entities/projectRole";

export interface ProjectRoleTaskCountStatistics {
	projectRole: ProjectRole | null;
	count: number;
}
