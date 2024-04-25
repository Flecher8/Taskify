// Function to convert local time to UTC time
export const localToUTC = (localDate: Date): Date => {
	const offset = localDate.getTimezoneOffset();
	const utcTime = new Date(localDate.getTime() + offset * 60 * 1000);
	return utcTime;
};

// Function to convert UTC time to local time
export const utcToLocal = (utcDate: Date): Date => {
	const offset = utcDate.getTimezoneOffset();
	const localTime = new Date(utcDate.getTime() - offset * 60 * 1000);
	return localTime;
};
