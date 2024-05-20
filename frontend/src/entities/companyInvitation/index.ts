import { Company } from "entities/company";
import { Notification } from "entities/notification";

export interface CompanyInvitation {
	id: string;
	notification: Notification;
	company: Company;
	isAccepted: boolean | null;
}
