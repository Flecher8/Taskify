import { FC } from "react";
import { DtPicker } from "react-calendar-datetime-picker";
import "react-calendar-datetime-picker/dist/style.css";
import { IDay } from "react-calendar-datetime-picker/dist/types/type";

interface DateTimePickerProps {
	initValue?: IDay;
	onChange: (date: IDay | null) => void;
	withTime?: boolean;
	calenderModalClass?: string;
	placeholder?: string;
	inputClass?: string;
}

const DateTimePicker: FC<DateTimePickerProps> = ({
	initValue,
	onChange,
	withTime = true,
	calenderModalClass = "",
	placeholder = "Choice date",
	inputClass = ""
}) => {
	return (
		<DtPicker
			initValue={initValue}
			onChange={onChange}
			withTime={withTime}
			showTimeInput={true}
			minDate={{ year: 2000, month: 1, day: 1 }}
			placeholder={placeholder}
			autoClose={false}
			calenderModalClass={calenderModalClass}
			inputClass={inputClass}
		/>
	);
};

export default DateTimePicker;
