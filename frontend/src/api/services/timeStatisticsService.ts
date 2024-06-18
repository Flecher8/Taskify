import { api } from "api/axious/api";
import { AxiosError, AxiosResponse } from "axios";
import { UserTimeSpendOnDateRequest } from "entities/statistics/time/userTimeSpendOnDateRequest";
import { UserTimeSpendOnDateStatistic } from "entities/statistics/time/userTimeSpendOnDateStatistic";

export default class TimeStatisticsService {
	private static baseUrl = "api/TimeStatistics";

	static async getUserProjectTimeStatistics(
		requestDto: UserTimeSpendOnDateRequest
	): Promise<UserTimeSpendOnDateStatistic | undefined> {
		try {
			const response: AxiosResponse<UserTimeSpendOnDateStatistic> = await api.post(
				`${TimeStatisticsService.baseUrl}/user-project-time-statistics`,
				requestDto
			);
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
