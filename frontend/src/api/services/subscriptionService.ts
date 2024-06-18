import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { Subscription } from "entities/subscription";

export interface CreateUserSubscriptionDto {
	userId: string;
	subscriptionId: string;
}

class SubscriptionsService {
	private static baseUrl = "api/Subscriptions";

	static async getAllSubscriptions(): Promise<Subscription[] | undefined> {
		try {
			const response: AxiosResponse<Subscription[]> = await api.get(`${SubscriptionsService.baseUrl}`);
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

	static async getSubscription(id: string): Promise<Subscription | undefined> {
		try {
			const response: AxiosResponse<Subscription> = await api.get(`${SubscriptionsService.baseUrl}/${id}`);
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
}

export default SubscriptionsService;
