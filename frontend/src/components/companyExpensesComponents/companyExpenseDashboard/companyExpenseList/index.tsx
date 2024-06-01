import { CompanyExpense } from "entities/companyExpense";
import { FC } from "react";
import CompanyExpenseListItem from "./companyExpenseListItem";

interface CompanyExpenseListProps {
	expenses: CompanyExpense[];
	filterName: string;
	editExpense: (expense: CompanyExpense) => void;
	deleteExpense: (id: string) => void;
}

const CompanyExpenseList: FC<CompanyExpenseListProps> = ({ expenses, filterName, editExpense, deleteExpense }) => {
	return (
		<div className="flex flex-col h-full">
			<div className="flex flex-col border-b max-h-96 overflow-auto custom-scroll-sm">
				{expenses
					.filter(expense => expense.name.toLowerCase().includes(filterName.toLowerCase()))
					.map(expense => (
						<CompanyExpenseListItem
							key={expense.id}
							expense={expense}
							editExpense={editExpense}
							deleteExpense={deleteExpense}
						/>
					))}
			</div>
		</div>
	);
};

export default CompanyExpenseList;
