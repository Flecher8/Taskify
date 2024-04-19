import { Section, SectionType } from "entities/section";
import { FC, useState } from "react";

interface EditSectionFormProps {
	section: Section;
	editSection: (section: Section) => void;
	close: () => void;
}

const EditSectionForm: FC<EditSectionFormProps> = ({ section, editSection, close }) => {
	const [selectedSectionType, setSelectedSectionType] = useState<SectionType>(section.sectionType);

	const handleSectionTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
		setSelectedSectionType(parseInt(event.target.value, 10));
	};

	const handleEditSection = () => {
		const updatedSection: Section = {
			...section,
			sectionType: selectedSectionType
		};
		editSection(updatedSection);
		close();
	};

	return (
		<div className="p-4">
			<div>
				<h2 className="text-xl font-bold mb-4">Edit Section</h2>
			</div>
			<div className="mb-4">
				<label htmlFor="sectionType" className="block text-gray-700 font-bold mb-2">
					Section Type
				</label>
				<select
					id="sectionType"
					className="w-full border rounded p-2"
					value={selectedSectionType}
					onChange={handleSectionTypeChange}>
					<option value={SectionType.ToDo}>To Do</option>
					<option value={SectionType.Doing}>Doing</option>
					<option value={SectionType.Done}>Done</option>
				</select>
			</div>
			<button className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600" onClick={handleEditSection}>
				Edit
			</button>
		</div>
	);
};

export default EditSectionForm;
