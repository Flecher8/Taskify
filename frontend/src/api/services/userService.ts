import { api } from "api/axious/api";
import Result from "./result";
import { User } from "entities/user";

export default class UserService {
	static async getUserByEmail(email: string): Promise<User | undefined> {
		try {
			const response = await api.get<User>(`/api/Users/email/${email}`);
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
