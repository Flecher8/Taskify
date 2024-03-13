import { makeAutoObservable } from "mobx";

class UserStore {
	constructor() {
		makeAutoObservable(this);
	}

	get userId() {
		return localStorage.getItem("userId");
	}
}

export default new UserStore();
