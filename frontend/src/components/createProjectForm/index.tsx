import { FC, useEffect, useRef, useState } from "react";

interface CreateProjectFormProps {
	onCreate: (name: string) => void;
	isVisible: boolean;
	closeComponent: () => void;
}
// TODO Add a limit on the number of projects per user subscription

const CreateProjectForm: FC<CreateProjectFormProps> = ({ onCreate, isVisible, closeComponent }) => {
	const [projectName, setProjectName] = useState("");
	const formRef = useRef<HTMLDivElement>(null); // Ref for the form element

	useEffect(() => {
		const handleClickOutside = (event: MouseEvent) => {
			if (formRef.current && !formRef.current.contains(event.target as Node)) {
				// Clicked outside the form, close it
				setProjectName("");
				closeComponent();
			}
		};

		if (isVisible) {
			// Attach event listener when form is visible
			document.addEventListener("mousedown", handleClickOutside);
		} else {
			// Remove event listener when form is not visible
			document.removeEventListener("mousedown", handleClickOutside);
		}

		return () => {
			// Cleanup: remove event listener on unmount
			document.removeEventListener("mousedown", handleClickOutside);
		};
	}, [isVisible, onCreate, closeComponent]);

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		setProjectName(e.target.value);
	};

	const handleSubmit = () => {
		if (projectName.trim() !== "") {
			onCreate(projectName);
			setProjectName(""); // Reset project name
		}
	};

	return (
		<div
			ref={formRef}
			className={`dropdown-content ${isVisible ? "block" : "hidden"} z-[1] card w-64 p-4 shadow-md bg-white`}
			style={{ visibility: isVisible ? "visible" : "hidden", opacity: isVisible ? 1 : 0 }}>
			<div className="flex flex-col">
				<div className="flex justify-center mb-5">
					<h5 className="">Create new project</h5>
				</div>
				<div className="flex justify-center mb-5">
					<input
						type="text"
						value={projectName}
						onChange={handleChange}
						placeholder="Enter project name"
						className="w-full border border-solid border-gray-200 rounded-md text-base font-light p-2"
						required
					/>
				</div>
				<div className="flex">
					<button type="button" className="btn btn-secondary w-full" onClick={handleSubmit}>
						Create
					</button>
				</div>
			</div>
		</div>
	);
};

export default CreateProjectForm;
