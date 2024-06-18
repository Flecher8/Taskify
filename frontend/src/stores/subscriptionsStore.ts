import SubscriptionsService from "api/services/subscriptionService";
import { Subscription } from "entities/subscription";
import { makeAutoObservable } from "mobx";

class SubscriptionsStore {
	constructor() {
		makeAutoObservable(this);
	}

	async getAllSubscriptions(): Promise<Subscription[] | undefined> {
		try {
			return await SubscriptionsService.getAllSubscriptions();
		} catch (error) {
			console.error("Error fetching subscriptions:", error);
			return undefined;
		}
	}

	async getSubscription(id: string): Promise<Subscription | undefined> {
		try {
			return await SubscriptionsService.getSubscription(id);
		} catch (error) {
			console.error("Error fetching subscription:", error);
			return undefined;
		}
	}
}

export default new SubscriptionsStore();
