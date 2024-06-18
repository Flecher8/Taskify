import { FC } from "react";

import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend, ChartData } from "chart.js";
import { Bar } from "react-chartjs-2";

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

interface BarChartProps {
	data: ChartData<"bar", (number | [number, number] | null)[], unknown>;
}

const BarChart: FC<BarChartProps> = ({ data }) => {
	return <Bar data={data} />;
};

export default BarChart;
