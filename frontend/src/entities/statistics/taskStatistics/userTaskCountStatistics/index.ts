import { User } from "entities/user";

export interface UserTaskCountStatistics {
	user: User | null;
	count: number;
}
