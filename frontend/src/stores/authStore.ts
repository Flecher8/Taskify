import { makeAutoObservable } from "mobx";
import AuthService from "api/services/authService";
import UserService from "api/services/userService";

export interface RegisterData {
	email: string;
	password: string;
	firstName: string;
	lastName: string;
}

class AuthStore {
	constructor() {
		makeAutoObservable(this);
	}

	async login(email: string, password: string): Promise<void> {
		try {
			if (this.isAuth) {
				this.redirectToProject();
			}
			const response = await AuthService.login({ email, password });
			if (response === undefined) {
				throw new Error("Login failed.");
			}

			localStorage.setItem("accessToken", response.accessToken);
			localStorage.setItem("refreshToken", response.refreshToken);
			localStorage.setItem("userEmail", email);

			const userResponse = await UserService.getUserByEmail(email);
			if (userResponse === undefined) {
				throw new Error("Login failed.");
			}
			localStorage.setItem("userId", userResponse.id);
		} catch (err: any) {
			throw new Error(err.message);
		}
	}

	async register(data: RegisterData): Promise<void> {
		try {
			if (this.isAuth) {
				this.redirectToProject();
			}
			const response = await AuthService.register({ email: data.email, password: data.password });
			if (response === undefined) {
				throw new Error("Register failed.");
			}

			await this.login(data.email, data.password);

			const userId: string | null = localStorage.getItem("userId");
			const userEmail: string | null = localStorage.getItem("userEmail");

			if (userId === null || userEmail === null) {
				throw new Error("Can not find user.");
			}

			const responseUpdateUser = await UserService.update(userId, {
				id: userId,
				email: userEmail,
				firstName: data.firstName,
				lastName: data.lastName,
				createdAt: new Date()
			});
		} catch (err: any) {
			throw new Error(err.message);
		}
	}

	async refreshAuth(): Promise<void> {
		try {
			const refreshToken = localStorage.getItem("refreshToken") || "";
			const response = await AuthService.refresh(refreshToken);
			if (response.status >= 400) {
				throw new Error("Refresh failed");
			}
			localStorage.setItem("accessToken", response.data.accessToken);
		} catch (err: any) {
			throw new Error(err.message);
		}
	}

	async logout(): Promise<void> {
		try {
			localStorage.removeItem("accessToken");
			localStorage.removeItem("refreshToken");
			localStorage.removeItem("userEmail");
			localStorage.removeItem("userId");
		} catch (err: any) {
			throw new Error(err.message);
		} finally {
			this.redirectToLogin();
		}
	}

	private redirectToLogin() {
		// Redirect to the new location
		window.location.href = "/login";
	}

	private redirectToProject() {
		window.location.href = "/projects";
	}

	get isAuth() {
		if (
			localStorage.getItem("accessToken") === null ||
			localStorage.getItem("refreshToken") === null ||
			localStorage.getItem("userEmail") === null ||
			localStorage.getItem("userId") === null
		) {
			return false;
		}

		return true;
	}
}

export default new AuthStore();
