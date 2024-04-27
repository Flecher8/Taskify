// Function to convert local time to UTC time
export const localToUTC = (localDate: Date): Date | null => {
	try {
		const offset = new Date().getTimezoneOffset();

		const date = new Date(localDate);

		const utcTime = new Date(date.getTime() + offset * 60 * 1000);
		return utcTime;
	} catch (error) {
		console.log(error);
		console.log(localDate);
	}
	return null;
};

// Function to convert UTC time to local time
export const utcToLocal = (utcDate: Date): Date | null => {
	try {
		const offset = new Date().getTimezoneOffset();

		const date = new Date(utcDate);

		const localTime = new Date(date.getTime() - offset * 60 * 1000);
		return localTime;
	} catch (error) {
		console.log(error);
		console.log(utcDate);
	}
	return null;
};
