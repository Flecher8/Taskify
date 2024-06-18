import { CompanyExpense, CompanyExpenseFrequency } from "entities/companyExpense";
import { FC, useState } from "react";

interface EditCompanyExpenseFormProps {
	expense: CompanyExpense;
	editExpense: (expense: CompanyExpense) => void;
	close: () => void;
}

const EditCompanyExpenseForm: FC<EditCompanyExpenseFormProps> = ({ expense, editExpense, close }) => {
	const [name, setName] = useState(expense.name);
	const [amount, setAmount] = useState<number>(expense.amount);
	const [frequency, setFrequency] = useState<CompanyExpenseFrequency>(expense.frequency);

	const handleNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setName(event.target.value);
	};

	const handleAmountChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setAmount(parseFloat(event.target.value));
	};

	const handleFrequencyChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		setFrequency(parseInt(event.target.value) as CompanyExpenseFrequency);
	};

	const handleEditCompanyExpense = () => {
		editExpense({
			...expense,
			name: name,
			amount: amount,
			frequency: frequency
		});
		close();
	};

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Edit Company Expense</h2>
			</div>
			<div className="mb-4">
				<label htmlFor="name" className="block text-gray-700 font-bold mb-2">
					Name
				</label>
				<input
					type="text"
					id="name"
					className="w-full border rounded p-2"
					value={name}
					onChange={handleNameChange}
				/>
			</div>
			<div className="mb-4">
				<label htmlFor="amount" className="block text-gray-700 font-bold mb-2">
					Amount
				</label>
				<input
					type="number"
					id="amount"
					className="w-full border rounded p-2"
					value={amount}
					onChange={handleAmountChange}
				/>
			</div>
			<div className="mb-4">
				<label htmlFor="frequency" className="block text-gray-700 font-bold mb-2">
					Frequency
				</label>
				<select
					id="frequency"
					className="w-full border rounded p-2"
					value={frequency}
					onChange={handleFrequencyChange}>
					<option value={CompanyExpenseFrequency.Monthly}>Monthly</option>
					<option value={CompanyExpenseFrequency.Yearly}>Yearly</option>
				</select>
			</div>
			<button
				className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600"
				onClick={handleEditCompanyExpense}>
				Save
			</button>
		</div>
	);
};

export default EditCompanyExpenseForm;
