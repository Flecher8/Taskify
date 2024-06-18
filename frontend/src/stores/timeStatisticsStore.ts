import TimeStatisticsService from "api/services/timeStatisticsService";
import { UserTimeSpendOnDateRequest } from "entities/statistics/time/userTimeSpendOnDateRequest";
import { UserTimeSpendOnDateStatistic } from "entities/statistics/time/userTimeSpendOnDateStatistic";
import { makeAutoObservable } from "mobx";

class TimeStatisticsStore {
	constructor() {
		makeAutoObservable(this);
	}

	async getUserProjectTimeStatistics(
		requestDto: UserTimeSpendOnDateRequest
	): Promise<UserTimeSpendOnDateStatistic | undefined> {
		try {
			return await TimeStatisticsService.getUserProjectTimeStatistics(requestDto);
		} catch (error) {
			console.error("Error fetching user project time statistics:", error);
			return undefined;
		}
	}
}

export default new TimeStatisticsStore();
