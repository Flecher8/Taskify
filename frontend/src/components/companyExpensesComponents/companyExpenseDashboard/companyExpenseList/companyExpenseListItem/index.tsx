import { CompanyExpense, getExpenseFrequencyName } from "entities/companyExpense";
import Modal from "components/modal";
import { FC } from "react";
import EditCompanyExpenseForm from "./editCompanyExpenseForm";
import DeleteCompanyExpenseForm from "./deleteCompanyExpenseForm";

interface CompanyExpenseListItemProps {
	expense: CompanyExpense;
	editExpense: (expense: CompanyExpense) => void;
	deleteExpense: (id: string) => void;
}

const CompanyExpenseListItem: FC<CompanyExpenseListItemProps> = ({ expense, editExpense, deleteExpense }) => {
	const idModalEdit = `editExpense${expense.id}`;
	const idModalDelete = `deleteExpense${expense.id}`;

	const closeModal = (modalId: string) => {
		const modal = document.getElementById(modalId) as HTMLDialogElement;
		modal.close();
	};

	return (
		<div className="grid grid-cols-7 gap-4 border-t p-3 items-center">
			<div className="truncate col-span-3">{expense.name}</div>
			<div className="truncate col-span-2">{`$${expense.amount.toFixed(2)}`}</div>
			<div className="truncate col-span-1">{getExpenseFrequencyName(expense.frequency)}</div>
			<div className="flex justify-around col-span-1">
				<div className="mr-5">
					<Modal
						id={idModalEdit}
						openButtonText={
							<i className="fa-light fa-pen-to-square rounded-full hover:bg-gray-200 p-1 w-10 transition duration-200"></i>
						}
						openButtonStyle={""}>
						<EditCompanyExpenseForm
							expense={expense}
							editExpense={editExpense}
							close={() => closeModal(idModalEdit)}
						/>
					</Modal>
				</div>
				<div>
					<Modal
						id={idModalDelete}
						openButtonText={
							<i className="fa-light fa-trash rounded-full hover:bg-gray-200 p-1 w-10 transition duration-200"></i>
						}
						openButtonStyle={""}>
						<DeleteCompanyExpenseForm
							expenseId={expense.id}
							deleteExpense={deleteExpense}
							close={() => closeModal(idModalDelete)}
						/>
					</Modal>
				</div>
			</div>
		</div>
	);
};

export default CompanyExpenseListItem;
