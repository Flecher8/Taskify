import { FC } from "react";

import { Chart as ChartJS, ArcElement, Tooltip, Legend, ChartData } from "chart.js";
import { Pie } from "react-chartjs-2";

ChartJS.register(ArcElement, Tooltip, Legend);

interface PieChartProps {
	data: ChartData<"pie", number[], unknown>;
}

const PieChart: FC<PieChartProps> = ({ data }) => {
	return <Pie data={data} />;
};

export default PieChart;
