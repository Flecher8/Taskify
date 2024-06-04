import BarChart from "components/charts/bar";
import { KpiStatistics } from "entities/statistics/kpi";
import { FC, useEffect, useState } from "react";
import kpiStatisticsStore from "stores/kpiStatisticsStore";

interface KpiStatisticsComponentProps {
	projectId: string;
}

const KpiStatisticsComponent: FC<KpiStatisticsComponentProps> = ({ projectId }) => {
	const [kpis, setKpis] = useState<KpiStatistics[] | undefined>(undefined);

	useEffect(() => {
		loadData();
	}, []);

	const loadData = async () => {
		try {
			const kpisData = await kpiStatisticsStore.getKpiForAllUsersInProject(projectId);
			setKpis(kpisData);
		} catch (error) {
			console.error("Error fetching project users kpis:", error);
		}
	};

	const chartData = {
		labels:
			kpis?.map(statistics => {
				if (statistics.user) {
					return `${statistics.user.firstName} ${statistics.user.lastName}`;
				} else {
					return "Not Assigned";
				}
			}) || [], // Ensure labels is not undefined
		datasets: [
			{
				label: "KPI",
				data: kpis?.map(statistics => statistics.kpi) || [], // Ensure data is not undefined
				backgroundColor: "rgba(53, 162, 235, 0.5)" // Adjust color as needed
			}
		]
	};

	return <div className="flex justify-center items-center w-full h-full">{kpis && <BarChart data={chartData} />}</div>;
};

export default KpiStatisticsComponent;
