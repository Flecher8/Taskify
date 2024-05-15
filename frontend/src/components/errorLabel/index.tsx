import { FC } from "react";

interface ErrorLabelProps {
	message: string;
}

const ErrorLabel: FC<ErrorLabelProps> = ({ message }) => {
	return (
		<div className="flex flex-col w-full h-full">
			<div className="flex justify-center items-center w-full h-full">
				<i className="fa-regular fa-circle-xmark text-red-500 text-4xl"></i>
				<span className="ml-2 text-red-500 text-xl">{message}</span>
			</div>
		</div>
	);
};

export default ErrorLabel;
