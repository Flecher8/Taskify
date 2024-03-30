import { Section } from "entities/section";
import { User } from "entities/user";

export interface CustomTask {
	id: string;
	section?: Section;
	responsibleUser?: User;
	name: string;
	description: string;
	startDateTimeUtc: Date;
	endDateTimeUtc: Date;
	storyPoints: number | null;
	isArchived: boolean;
	createdAt: Date;
	sequenceNumber: number;
}
