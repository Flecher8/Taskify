export interface Subscription {
	id: string;
	name: string;
	pricePerMonth: number;
	durationInDays: number;
	projectsLimit: number;
	projectMembersLimit: number;
	projectSectionsLimit: number;
	projectTasksLimit: number;
	canCreateCompany: boolean;
}
