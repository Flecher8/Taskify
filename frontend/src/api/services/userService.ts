import { api } from "api/axious/api";
import Result from "./result";

interface GetUserByEmailResponse {
	id: string;
	email: string;
	firstName: string;
	lastName: string;
	createdAt: Date;
}

export interface User {
	id: string;
	email: string;
	firstName?: string;
	lastName?: string;
	createdAt: Date;
}

export default class UserService {
	static async getUserByEmail(email: string): Promise<GetUserByEmailResponse | undefined> {
		try {
			const response = await api.get<GetUserByEmailResponse>(`/api/Users/email/${email}`);
			return response.data;
		} catch (error) {
			throw error;
		}
	}

	static async update(id: string, user: User): Promise<Result<boolean> | undefined> {
		try {
			const response = await api.put<Result<boolean>>(`/api/Users/${id}`, user);
			return response.data;
		} catch (error) {
			throw error;
		}
	}
}
