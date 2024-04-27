import { FC } from "react";
import { DtPicker } from "react-calendar-datetime-picker";
import "react-calendar-datetime-picker/dist/style.css";
import { IDay } from "react-calendar-datetime-picker/dist/types/type";

interface DateTimePickerProps {
	initValue?: IDay;
	onChange: (date: IDay | null) => void;
}

const DateTimePicker: FC<DateTimePickerProps> = ({ initValue, onChange }) => {
	return (
		<DtPicker
			initValue={initValue}
			onChange={onChange}
			withTime={true}
			showTimeInput={true}
			minDate={{ year: 2000, month: 1, day: 1 }}
			placeholder={"Choice date"}
			autoClose={false}
		/>
	);
};

export default DateTimePicker;
