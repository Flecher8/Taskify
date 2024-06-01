import { ProjectIncome } from "entities/projectIncome";
import { FC } from "react";
import ProjectIncomeListItem from "./projectIncomeListItem";

interface ProjectIncomeListProps {
	incomes: ProjectIncome[];
	filterName: string;
	editIncome: (income: ProjectIncome) => void;
	deleteIncome: (id: string) => void;
}

const ProjectIncomeList: FC<ProjectIncomeListProps> = ({ incomes, filterName, editIncome, deleteIncome }) => {
	return (
		<div className="flex flex-col h-full">
			<div className="flex flex-col border-b max-h-96 overflow-auto custom-scroll-sm">
				{incomes
					.filter(income => income.name.toLowerCase().includes(filterName.toLowerCase()))
					.map(income => (
						<ProjectIncomeListItem
							key={income.id}
							income={income}
							editIncome={editIncome}
							deleteIncome={deleteIncome}
						/>
					))}
			</div>
		</div>
	);
};

export default ProjectIncomeList;
