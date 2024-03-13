export default interface Result<T> {
	isSuccess: boolean;
	data: T;
	errors: string[];
}
