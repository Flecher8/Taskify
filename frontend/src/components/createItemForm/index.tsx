import React, { FC, useEffect, useRef, useState } from "react";

interface CreateItemItemByNameFormProps {
	onCreate: (name: string) => void;
	placeholderText: string;
	openedButtonText: string;
	frontButtonText: string;
}

const CreateItemByNameForm: FC<CreateItemItemByNameFormProps> = ({
	onCreate,
	placeholderText,
	openedButtonText,
	frontButtonText
}) => {
	const [isCreating, setIsCreating] = useState(false);
	const [itemName, setItemName] = useState<string>("");
	const inputRef = useRef<HTMLInputElement>(null);
	const containerRef = useRef<HTMLDivElement>(null);

	useEffect(() => {
		const handleClickOutside = (event: MouseEvent) => {
			if (containerRef.current && !containerRef.current.contains(event.target as Node)) {
				setIsCreating(false);
			}
		};

		document.addEventListener("mousedown", handleClickOutside);

		return () => {
			document.removeEventListener("mousedown", handleClickOutside);
		};
	}, []);

	const handleCreateItemClick = () => {
		setIsCreating(true);
	};

	const handleCreateClick = () => {
		onCreate(itemName);
		setItemName("");
	};

	const handleCancelClick = () => {
		setIsCreating(false);
		setItemName("");
	};

	return (
		<>
			{isCreating ? (
				<div className="m-[10px] flex flex-col duration-200" ref={containerRef}>
					<div>
						<input
							type="text"
							className="border text-sm rounded-md w-full min-h-[50px] p-1"
							value={itemName}
							onChange={e => setItemName(e.target.value)}
							placeholder={placeholderText}
							ref={inputRef}
						/>
					</div>
					<div className="mt-2">
						<button
							className="bg-[#0c66e4] w-[100px] text-sm border rounded-md mr-5 text-white"
							onClick={handleCreateClick}>
							{openedButtonText}
						</button>
						<button onClick={handleCancelClick}>
							<i className="fa-light fa-xmark "></i>
						</button>
					</div>
				</div>
			) : (
				<button
					className="m-[8px] h-[30px] pr-5 text-sm bg-stone-100 border rounded-lg hover:bg-stone-200 duration-200 shrink-0 flex flex-row items-center"
					onClick={handleCreateItemClick}>
					<i className="fa-light fa-plus ml-2 mr-1"></i>
					<h3>{frontButtonText}</h3>
				</button>
			)}
		</>
	);
};

export default CreateItemByNameForm;
