import UserSubscriptionsService from "api/services/userSubscriptionService";
import { Subscription } from "entities/subscription";
import { makeAutoObservable } from "mobx";

class UserSubscriptionsStore {
	constructor() {
		makeAutoObservable(this);
	}

	async getUserSubscription(userId: string): Promise<Subscription | undefined> {
		try {
			return await UserSubscriptionsService.getUserSubscription(userId);
		} catch (error) {
			console.error("Error fetching user subscription:", error);
			return undefined;
		}
	}

	async createUserSubscription(userId: string, subscriptionId: string): Promise<void | undefined> {
		try {
			return await UserSubscriptionsService.createUserSubscription({
				userId: userId,
				subscriptionId: subscriptionId
			});
		} catch (error) {
			console.error("Error creating user subscription:", error);
			return undefined;
		}
	}
}

export default new UserSubscriptionsStore();
