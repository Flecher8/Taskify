import { User } from "entities/user";

export interface Project {
	id: string;
	userId: string;
	user: User;
	name: string;
	createdAt: Date;
	normalWorkingHoursPerDay: number;
}
