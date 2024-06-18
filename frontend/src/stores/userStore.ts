import UserService from "api/services/userService";
import { User } from "entities/user";
import { makeAutoObservable } from "mobx";

class UserStore {
	constructor() {
		makeAutoObservable(this);
	}

	get userId() {
		return localStorage.getItem("userId");
	}

	async getUserById(id: string): Promise<User | undefined> {
		try {
			return await UserService.getUserById(id);
		} catch (error) {
			console.error("Error fetching user by id:", error);
			return undefined;
		}
	}
}

export default new UserStore();
