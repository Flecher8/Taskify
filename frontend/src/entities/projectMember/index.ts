import { Project } from "entities/project";
import { ProjectRole } from "entities/projectRole";
import { User } from "entities/user";

export interface ProjectMember {
	id: string;
	project: Project;
	user: User;
	projectRole: ProjectRole | null;
}
