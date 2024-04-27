import React, { ReactNode, useEffect, useRef, useState } from "react";
import { types } from "util";

interface ClickToEditProps {
	initialValue: string | number | null;
	initialTextStyle?: string;
	inputStyle?: string;
	onValueChange: (newValue: any) => void;
	useHover?: boolean;
	checkEmptyText?: boolean;
	maxLength?: number;
	isTextArea?: boolean;
	type?: string;
	minValue?: number;
	maxValue?: number;
	placeholder?: ReactNode;
}

const ClickToEdit: React.FC<ClickToEditProps> = ({
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
	maxValue = 10000000,
	placeholder = ""
}) => {
	const [value, setValue] = useState<string | number | null>(initialValue);
	const [previousValue, setPreviousValue] = useState<string | number | null>(initialValue);
	const [isEditing, setIsEditing] = useState(false);
	const [isHovered, setIsHovered] = useState(false);
	const itemRef = useRef<HTMLDivElement>(null);

	const handleTextChange = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
		const newText = event.target.value;
		if (newText.length <= maxLength) {
			if (type === "number") {
				const numericValue = parseInt(newText);
				if (!isNaN(numericValue)) {
					setValue(numericValue);
				} else {
					setValue(null);
				}
			} else {
				setValue(newText);
			}
		}
	};

	const handleStartEditing = () => {
		setIsEditing(true);
		setPreviousValue(value);
	};

	const handleStopEditing = () => {
		if (value === null) {
			onValueChange(null);
		} else if (typeof value === "string") {
			if (checkEmptyText) {
				if (value.length === 0 || value.length > maxLength) {
					setValue(previousValue);
				} else {
					onValueChange(value);
				}
			} else {
				onValueChange(value);
			}
		} else if (typeof value === "number") {
			if (value >= minValue && value <= maxValue) {
				onValueChange(value);
			} else {
				setValue(previousValue);
			}
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
						value={value !== null ? value.toString() : ""}
						onChange={handleTextChange}
						autoFocus
					/>
				) : (
					<input
						type={`${type}`}
						className={`p-1 bg-white border border-purple-900 resize-none ${inputStyle}`}
						value={value !== null ? value.toString() : ""}
						onChange={handleTextChange}
						autoFocus
						min={minValue}
						max={maxValue}
					/>
				)
			) : (
				<p className={`p-1 truncate ${initialTextStyle}`} onClick={handleStartEditing}>
					{value != null && value !== "" ? value : placeholder !== "" ? placeholder : value}
				</p>
			)}
		</div>
	);
};

export default ClickToEdit;
