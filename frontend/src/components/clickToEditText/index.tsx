import React, { useEffect, useState } from "react";

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

	const handleTextChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setText(event.target.value);
	};

	const handleStartEditing = () => {
		setIsEditing(true);
		setPreviousText(text);
	};

	const handleStopEditing = () => {
		if (text.length === 0) {
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
			className={`relative ${
				isEditing ? "bg-white" : useHover ? (isHovered ? "bg-gray-300" : "") : ""
			} duration-300`}
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
				<input type="text" className="p-1 truncate" value={text} readOnly onClick={handleStartEditing} />
			)}
		</div>
	);
};

export default ClickToEditText;
