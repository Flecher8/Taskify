import Modal from "components/modal";
import { ProjectIncome, getIncomeFrequencyName } from "entities/projectIncome";
import { FC } from "react";
import DeleteProjectIncomeForm from "./deleteProjectIncomeForm";
import EditProjectIncomeForm from "./editProjectIncomeForm";

interface ProjectIncomeListItemProps {
	income: ProjectIncome;
	editIncome: (income: ProjectIncome) => void;
	deleteIncome: (id: string) => void;
}

const idEditIncomeModal = "editProjectIncome";
const idDeleteIncomeModal = "deleteProjectIncome";

const ProjectIncomeListItem: FC<ProjectIncomeListItemProps> = ({ income, editIncome, deleteIncome }) => {
	const closeEditModal = () => {
		const modal = document.getElementById(income.id + idEditIncomeModal) as HTMLDialogElement;
		modal.close();
	};

	const closeDeleteModal = () => {
		const modal = document.getElementById(income.id + idDeleteIncomeModal) as HTMLDialogElement;
		modal.close();
	};

	return (
		<div className="grid grid-cols-7 gap-4 border-t p-3 items-center">
			<div className="truncate col-span-3">{income.name}</div>
			<div className="truncate col-span-2">${income.amount}</div>
			<div className="truncate col-span-1">{getIncomeFrequencyName(income.frequency)}</div>
			<div className="flex justify-around col-span-1">
				<div className="mr-5">
					<Modal
						id={income.id + idEditIncomeModal}
						openButtonText={<i className="fa-light fa-pen-to-square rounded-full hover:bg-gray-200 p-1 w-10"></i>}
						openButtonStyle={""}>
						<EditProjectIncomeForm income={income} edit={editIncome} close={closeEditModal} />
					</Modal>
				</div>
				<div>
					<Modal
						id={income.id + idDeleteIncomeModal}
						openButtonText={<i className="fa-light fa-trash rounded-full hover:bg-gray-200 p-1 w-10"></i>}
						openButtonStyle={""}>
						<DeleteProjectIncomeForm income={income} deleteIncome={deleteIncome} close={closeDeleteModal} />
					</Modal>
				</div>
			</div>
		</div>
	);
};

export default ProjectIncomeListItem;
