import { User } from "entities/user";

export interface UserStoryPointsCountStatistics {
	user: User | null;
	count: number;
}
