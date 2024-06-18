import { api } from "api/axious/api";
import { AxiosResponse } from "axios";

interface LoginResponse {
	accessToken: string;
	refreshToken: string;
}

interface RegisterResponse {
	type: string;
	title: string;
	status: number;
	detail: string;
	instance: string;
	errors: {
		additionalProp1: [string];
		additionalProp2: [string];
		additionalProp3: [string];
	};
	additionalProp1: string;
	additionalProp2: string;
	additionalProp3: string;
}

interface RegisterData {
	email: string;
	password: string;
}

interface LoginData {
	email: string;
	password: string;
}

export default class AuthService {
	static async login(data: LoginData): Promise<LoginResponse | undefined> {
		try {
			const response = await api.post<LoginResponse>("/login", data);
			return response.data;
		} catch (err: any) {
			if (err.response?.status >= 400) {
				throw new Error(err.response.data);
			}
		}
		return undefined;
	}

	static async register(data: RegisterData): Promise<RegisterResponse | undefined> {
		try {
			const response = await api.post<RegisterResponse>("/register", data);
			if (response.status >= 200) {
				return response.data;
			}
		} catch (err: any) {
			if (err.response?.status >= 400) {
				throw new Error(err.response.data);
			}
		}
		return undefined;
	}

	static async refresh(refreshToken: string): Promise<AxiosResponse<LoginResponse, any>> {
		const response = await api.post<LoginResponse>("/refresh", { refreshToken: refreshToken });
		return response;
	}
}
