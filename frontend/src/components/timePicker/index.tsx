import { FC, useState } from "react";
import "./timePicker.scss";

interface TimePickerProps {
	time: string;
	onChange: (time: string) => void;
}

const TimePicker: FC<TimePickerProps> = ({ time, onChange }) => {
	const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const newTime = e.target.value;
		onChange(newTime);
	};

	return <input type="time" value={time} onChange={handleInputChange} placeholder="HH:MM" className="time-picker" />;
};

export default TimePicker;
