import Modal from "components/modal";
import { ProjectIncome, ProjectIncomeFrequency } from "entities/projectIncome";
import { FC, useEffect, useState } from "react";
import projectIncomesStore from "stores/projectIncomesStore";
import CreateProjectIncomeForm from "./createProjectIncomeForm";
import ProjectIncomeList from "./projectIncomeList";

interface ProjectIncomeDashboardProps {
	projectId?: string;
}

const idModal = "createIncome";

const ProjectIncomeDashboard: FC<ProjectIncomeDashboardProps> = ({ projectId }) => {
	const [incomes, setIncomes] = useState<ProjectIncome[]>([]);
	const [filterByName, setFilterByName] = useState("");

	useEffect(() => {
		loadProjectIncomes();
	}, [projectId]);

	const loadProjectIncomes = async () => {
		try {
			const incomes = await projectIncomesStore.getProjectIncomesByProjectId(projectId);
			const sortedProjectIncomes = incomes.sort((a: ProjectIncome, b: ProjectIncome) => {
				return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime();
			});
			setIncomes(sortedProjectIncomes);
		} catch (error) {
			console.error("Error loading project incomes:", error);
		}
	};

	const closeModal = () => {
		const modal = document.getElementById(idModal) as HTMLDialogElement;
		modal.close();
	};

	const createIncome = async (name: string, amount: number, frequency: ProjectIncomeFrequency) => {
		try {
			if (projectId === undefined) {
				throw new Error("Cannot find projectId");
			}
			await projectIncomesStore.createProjectIncome({
				projectId: projectId,
				name: name,
				amount: amount,
				frequency: frequency
			});
			loadProjectIncomes();
		} catch (error) {
			console.error(error);
		}
	};

	const editIncome = async (income: ProjectIncome) => {
		try {
			await projectIncomesStore.updateProjectIncome(income.id, income);
			loadProjectIncomes();
		} catch (error) {
			console.error(error);
		}
	};

	const deleteIncome = async (id: string) => {
		try {
			await projectIncomesStore.deleteProjectIncome(id);
			loadProjectIncomes();
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
					<CreateProjectIncomeForm create={createIncome} close={closeModal} />
				</Modal>
			</div>
			<div className="mt-5 h-full">
				{incomes.length === 0 ? (
					<p className="flex text-xl italic justify-center overflow-auto">There are no incomes in this project.</p>
				) : (
					<ProjectIncomeList
						incomes={incomes}
						filterName={filterByName}
						editIncome={editIncome}
						deleteIncome={deleteIncome}
					/>
				)}
			</div>
		</div>
	);
};

export default ProjectIncomeDashboard;
