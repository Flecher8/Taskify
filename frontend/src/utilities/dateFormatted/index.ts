export const getFormattedDate = (date: Date) => {
	const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
	const days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

	const month = months[date.getMonth()];
	const dayOfWeek = days[date.getDay()];
	const dayOfMonth = date.getDate().toString().padStart(2, "0");

	return { month, dayOfWeek, dayOfMonth };
};
