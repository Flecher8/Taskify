import { Section } from "entities/section";
import { FC, useState } from "react";

interface DeleteSectionFormProps {
	section: Section;
	deleteSection: (id: string, redirectId: string) => void;
	sections: Section[];
	close: () => void;
}

const DeleteSectionForm: FC<DeleteSectionFormProps> = ({ section, deleteSection, sections, close }) => {
	const initialRedirectSectionId = sections.find(s => s.id !== section.id)?.id || "";
	const [redirectSectionId, setRedirectSectionId] = useState<string>(initialRedirectSectionId);

	const handleDelete = () => {
		deleteSection(section.id, redirectSectionId);
	};

	if (sections.length <= 1) {
		return <div>You cannot delete this section because it is the last section in the project.</div>;
	}

	return (
		<div className="">
			<h1 className="text-xl font-bold mb-4">Delete section "{section.name}"</h1>
			<div className="mb-10">
				<p>Select a new home for tasks in this section</p>
			</div>
			<div className="mb-4 flex flex-row justify-evenly">
				<div className="flex flex-col ">
					<h3 className="mb-1">Section to delete:</h3>
					<div className="line-through">{section.name}</div>
				</div>
				<div className="flex items-center ">
					<i className="fa-light fa-arrow-right"></i>
				</div>
				<div className="">
					<label htmlFor="redirectSection" className="block ">
						Moving tasks to:
					</label>
					<select
						id="redirectSection"
						name="redirectSection"
						className="mt-1 block w-full pl-3 pr-10 py-2 border border-gray-300 bg-white rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
						value={redirectSectionId}
						onChange={e => setRedirectSectionId(e.target.value)}>
						{sections
							.filter(s => s.id !== section.id)
							.map(s => (
								<option key={s.id} value={s.id}>
									{s.name}
								</option>
							))}
					</select>
				</div>
			</div>
			<div className="flex justify-end">
				<button
					type="button"
					onClick={handleDelete}
					className="inline-flex justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 mr-2">
					Delete
				</button>
				<button
					type="button"
					onClick={close}
					className="inline-flex justify-center py-2 px-4 border border-transparent 
                    shadow-sm text-sm font-medium rounded-md text-gray-700 bg-white hover:bg-gray-200 
                    focus:outline-none focus:ring-2 focus:ring-offset-2 hover:ring-gray-300 transition duration-300">
					Cancel
				</button>
			</div>
		</div>
	);
};

export default DeleteSectionForm;
