import { CustomTask } from "entities/customTask";
import { User } from "entities/user";

export interface TaskTimeTracker {
	id: string;
	customTask: CustomTask;
	user: User;
	startDateTime: Date;
	endDateTime: Date | null;
	durationInSeconds: number;
	description: string;
	trackerType: TaskTimeTrackerType;
	createdAt: Date;
}

export enum TaskTimeTrackerType {
	Stopwatch,
	Range
}
