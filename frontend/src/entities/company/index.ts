import { User } from "entities/user";

export interface Company {
	id: string;
	user: User;
	name: string;
}
