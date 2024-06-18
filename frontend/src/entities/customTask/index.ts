import { Section } from "entities/section";
import { User } from "entities/user";

export interface CustomTask {
	id: string;
	section?: Section;
	responsibleUser: User | null;
	name: string;
	description: string;
	startDateTimeUtc: Date | null;
	endDateTimeUtc: Date | null;
	storyPoints: number | null;
	isArchived: boolean;
	createdAt: Date;
	sequenceNumber: number;
}
