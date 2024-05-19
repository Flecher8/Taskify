import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { Subscription } from "entities/subscription";

export interface CreateUserSubscriptionDto {
	userId: string;
	subscriptionId: string;
}

class UserSubscriptionsService {
	private static baseUrl = "api/UserSubscriptions";

	static async getUserSubscription(userId: string): Promise<Subscription | undefined> {
		try {
			const response: AxiosResponse<Subscription> = await api.get(`${UserSubscriptionsService.baseUrl}/${userId}`);
			return response.data;
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					return error.response.data;
				}
			}
			throw error;
		}
	}

	static async createUserSubscription(createUserSubscriptionDto: CreateUserSubscriptionDto): Promise<void> {
		try {
			await api.post(`${UserSubscriptionsService.baseUrl}`, createUserSubscriptionDto);
		} catch (error) {
			if (error instanceof AxiosError) {
				if (error.response) {
					throw new Error(error.response.data);
				}
			}
			throw error;
		}
	}
}

export default UserSubscriptionsService;
