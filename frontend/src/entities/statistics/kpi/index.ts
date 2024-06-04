import { User } from "entities/user";

export interface KpiStatistics {
	user: User;
	kpi: number;
}
