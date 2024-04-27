import { FC } from "react";
import { DtPicker } from "react-calendar-datetime-picker";
import "react-calendar-datetime-picker/dist/style.css";
import { IDay, IRange } from "react-calendar-datetime-picker/dist/types/type";

interface RangeDateTimePickerProps {
	initValue?: IRange;
	onChange: (date: IRange | null) => void;
}

const RangeDateTimePicker: FC<RangeDateTimePickerProps> = ({ initValue, onChange }) => {
	return (
		<DtPicker
			initValue={initValue}
			onChange={onChange}
			withTime={true}
			minDate={{ year: 2000, month: 1, day: 1 }}
			placeholder={"Choice dates"}
			autoClose={false}
			type="range"
			todayBtn={true}
			fromLabel={""}
			toLabel={""}
		/>
	);
};

export default RangeDateTimePicker;
