import { FC } from "react";

interface DeleteCompanyExpenseFormProps {
	expenseId: string;
	deleteExpense: (id: string) => void;
	close: () => void;
}

const DeleteCompanyExpenseForm: FC<DeleteCompanyExpenseFormProps> = ({ expenseId, deleteExpense, close }) => {
	const handleDelete = () => {
		deleteExpense(expenseId);
		close();
	};

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Delete Company Expense</h2>
				<p>Are you sure you want to delete this expense?</p>
			</div>
			<div className="flex mt-4">
				<button className="bg-red-500 text-white py-2 px-4 rounded hover:bg-red-600 mr-2" onClick={handleDelete}>
					Delete
				</button>
				<button className="bg-gray-300 text-gray-700 py-2 px-4 rounded hover:bg-gray-400" onClick={close}>
					Cancel
				</button>
			</div>
		</div>
	);
};

export default DeleteCompanyExpenseForm;
