import React, { useEffect, useRef, useState } from "react";
import { types } from "util";

interface ClickToEditTextProps {
	initialValue: string | number;
	initialTextStyle?: string;
	inputStyle?: string;
	onValueChange: (newText: string) => void;
	useHover?: boolean;
	checkEmptyText?: boolean;
	maxLength?: number;
	isTextArea?: boolean;
	type?: string;
	minValue?: number;
	maxValue?: number;
}

const ClickToEditText: React.FC<ClickToEditTextProps> = ({
	initialValue,
	initialTextStyle = "",
	inputStyle = "",
	onValueChange,
	useHover = true,
	checkEmptyText = true,
	maxLength = 100,
	isTextArea = false,
	type = "text",
	minValue = 0,
	maxValue = 10000000
}) => {
	const [value, setValue] = useState<string | number>(initialValue);
	const [previousValue, setPreviousValue] = useState<string | number>(initialValue);
	const [isEditing, setIsEditing] = useState(false);
	const [isHovered, setIsHovered] = useState(false);
	const itemRef = useRef<HTMLDivElement>(null);

	const handleTextChange = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
		const newText = event.target.value;
		if (newText.length <= maxLength) {
			setValue(newText);
		}
	};

	const handleStartEditing = () => {
		setIsEditing(true);
		setPreviousValue(value);
	};

	const handleStopEditing = () => {
		if (typeof value === "string") {
			if (checkEmptyText) {
				if (value.length === 0 || value.length > maxLength) {
					setValue(previousValue);
				} else {
					onValueChange(value);
				}
				setIsEditing(false);
			} else {
				onValueChange(value);
				setIsEditing(false);
			}
		} else if (typeof value === "number") {
			if (value >= minValue && value <= maxValue) {
				onValueChange(value.toString());
			} else {
				// Reset to previous value if out of range
				setValue(previousValue);
			}
			setIsEditing(false);
		}
	};

	const handleMouseEnter = () => {
		setIsHovered(true);
	};

	const handleMouseLeave = () => {
		setIsHovered(false);
	};

	useEffect(() => {
		setValue(initialValue);
	}, [initialValue]);

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
				isTextArea ? (
					<textarea
						className={`p-1 bg-white border border-purple-900 resize-none ${inputStyle}`}
						value={value}
						onChange={handleTextChange}
						autoFocus
					/>
				) : (
					<input
						type={`${type}`}
						className={`p-1 bg-white border border-purple-900 resize-none ${inputStyle}`}
						value={value}
						onChange={handleTextChange}
						autoFocus
					/>
				)
			) : (
				<p className={`p-1 truncate ${initialTextStyle}`} onClick={handleStartEditing}>
					{value}
				</p>
			)}
		</div>
	);
};

export default ClickToEditText;
