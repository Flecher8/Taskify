import Modal from "components/modal";
import { CompanyExpense, CompanyExpenseFrequency } from "entities/companyExpense";
import { FC, useEffect, useState } from "react";
import companyExpensesStore from "stores/companyExpensesStore";
import CreateCompanyExpenseForm from "./createCompanyExpenseForm";
import CompanyExpenseList from "./companyExpenseList";

interface CompanyExpenseDashboardProps {
	companyId?: string;
}

const idModal = "createExpense";

const CompanyExpenseDashboard: FC<CompanyExpenseDashboardProps> = ({ companyId }) => {
	const [expenses, setExpenses] = useState<CompanyExpense[]>([]);
	const [filterByName, setFilterByName] = useState("");

	useEffect(() => {
		loadCompanyExpenses();
	}, [companyId]);

	const loadCompanyExpenses = async () => {
		try {
			const expenses = await companyExpensesStore.getCompanyExpensesByCompanyId(companyId);
			const sortedCompanyExpenses = expenses.sort((a: CompanyExpense, b: CompanyExpense) => {
				return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime();
			});
			setExpenses(sortedCompanyExpenses);
		} catch (error) {
			console.error("Error loading company expenses:", error);
		}
	};

	const closeModal = () => {
		const modal = document.getElementById(idModal) as HTMLDialogElement;
		modal.close();
	};

	const createExpense = async (name: string, amount: number, frequency: CompanyExpenseFrequency) => {
		try {
			if (companyId === undefined) {
				throw new Error("Cannot find companyId");
			}
			await companyExpensesStore.createCompanyExpense({
				companyId: companyId,
				name: name,
				amount: amount,
				frequency: frequency
			});
			loadCompanyExpenses();
		} catch (error) {
			console.error(error);
		}
	};

	const editExpense = async (expense: CompanyExpense) => {
		try {
			await companyExpensesStore.updateCompanyExpense(expense.id, expense);
			loadCompanyExpenses();
		} catch (error) {
			console.error(error);
		}
	};

	const deleteExpense = async (id: string) => {
		try {
			await companyExpensesStore.deleteCompanyExpense(id);
			loadCompanyExpenses();
		} catch (error) {
			console.error(error);
		}
	};

	return (
		<div className="flex flex-col w-full justify-centerspace-y-4 h-full">
			<div className="flex justify-between">
				<input
					type="text"
					className="p-2 border rounded"
					placeholder="Filter by name"
					value={filterByName}
					onChange={e => setFilterByName(e.target.value)}
				/>
				<Modal id={idModal} openButtonText="Create" openButtonStyle="px-4 py-2 bg-blue-500 text-white rounded">
					<CreateCompanyExpenseForm create={createExpense} close={closeModal} />
				</Modal>
			</div>
			<div className="mt-5 h-full">
				{expenses.length === 0 ? (
					<p className="flex text-xl italic justify-center overflow-auto">
						There are no expenses in this company.
					</p>
				) : (
					<CompanyExpenseList
						expenses={expenses}
						filterName={filterByName}
						editExpense={editExpense}
						deleteExpense={deleteExpense}
					/>
				)}
			</div>
		</div>
	);
};

export default CompanyExpenseDashboard;
