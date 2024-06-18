import { ProjectIncome, ProjectIncomeFrequency } from "entities/projectIncome";
import { FC, useState } from "react";

interface EditProjectIncomeFormProps {
	income: ProjectIncome;
	edit: (income: ProjectIncome) => void;
	close: () => void;
}

const EditProjectIncomeForm: FC<EditProjectIncomeFormProps> = ({ income, edit, close }) => {
	const [name, setName] = useState(income.name);
	const [amount, setAmount] = useState<number>(income.amount);
	const [frequency, setFrequency] = useState<ProjectIncomeFrequency>(income.frequency);

	const handleNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setName(event.target.value);
	};

	const handleAmountChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setAmount(parseFloat(event.target.value));
	};

	const handleFrequencyChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		setFrequency(parseInt(event.target.value) as ProjectIncomeFrequency);
	};

	const handleEditIncome = () => {
		const editedIncome = { ...income, name, amount, frequency };
		edit(editedIncome);
		close();
	};

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Edit Project Income</h2>
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
					<option value={ProjectIncomeFrequency.Monthly}>Monthly</option>
					<option value={ProjectIncomeFrequency.Yearly}>Yearly</option>
				</select>
			</div>
			<button className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600" onClick={handleEditIncome}>
				Edit
			</button>
		</div>
	);
};

export default EditProjectIncomeForm;
