import { IDay } from "react-calendar-datetime-picker/dist/types/type";

export const dateToIDay = (date: Date | null): IDay | undefined => {
	if (!date) return undefined;
	if (date === null) return undefined;

	return {
		year: date.getFullYear(),
		month: date.getMonth() + 1,
		day: date.getDate(),
		hour: date.getHours(),
		minute: date.getMinutes()
	};
};

export const iDayToDate = (day: IDay | undefined | null): Date | null => {
	if (day === undefined || day === null) return null;
	return new Date(day.year, day.month - 1, day.day, day.hour || 0, day.minute || 0);
};
