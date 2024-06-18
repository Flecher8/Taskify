export const getCurrentWeekDates = (date: Date) => {
	const dayOfWeek = date.getDay();
	const startOfWeek = new Date(date);
	startOfWeek.setDate(date.getDate() - dayOfWeek + (dayOfWeek === 0 ? -6 : 1)); // Adjust for Sunday being 0

	const weekDates = Array.from({ length: 7 }).map((_, index) => {
		const weekDate = new Date(startOfWeek);
		weekDate.setDate(startOfWeek.getDate() + index);
		return weekDate;
	});

	return weekDates;
};
