import React, { useEffect, useRef, useState } from "react";

interface ClickToEditTextProps {
	initialText: string;
	onTextChange: (newText: string) => void;
	useHover?: boolean;
}

const ClickToEditText: React.FC<ClickToEditTextProps> = ({ initialText, onTextChange, useHover = true }) => {
	const [text, setText] = useState(initialText);
	const [previousText, setPreviousText] = useState(initialText);
	const [isEditing, setIsEditing] = useState(false);
	const [isHovered, setIsHovered] = useState(false);
	const itemRef = useRef<HTMLDivElement>(null);

	const handleTextChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		const newText = event.target.value;
		if (newText.length <= 100) {
			setText(newText);
		}
	};

	const handleStartEditing = () => {
		setIsEditing(true);
		setPreviousText(text);
	};

	const handleStopEditing = () => {
		if (text.length === 0 || text.length > 100) {
			setText(previousText);
		} else {
			onTextChange(text);
		}
		setIsEditing(false);
	};

	const handleMouseEnter = () => {
		setIsHovered(true);
	};

	const handleMouseLeave = () => {
		setIsHovered(false);
	};

	useEffect(() => {
		setText(initialText);
	}, [initialText]);

	return (
		<div
			ref={itemRef}
			className={`relative ${
				isEditing ? "bg-white" : useHover ? (isHovered ? "bg-gray-300" : "") : ""
			} duration-300 hover:cursor-pointer`}
			onBlur={handleStopEditing}
			onMouseEnter={handleMouseEnter}
			onMouseLeave={handleMouseLeave}>
			{isEditing ? (
				<input
					type="text"
					className="p-1 bg-white border border-purple-900"
					value={text}
					onChange={handleTextChange}
					autoFocus
				/>
			) : (
				// <input type="text" className="p-1 truncate" value={text} readOnly onClick={handleStartEditing} />
				<p className="p-1 truncate" onClick={handleStartEditing}>
					{text}
				</p>
			)}
		</div>
	);
};

export default ClickToEditText;
