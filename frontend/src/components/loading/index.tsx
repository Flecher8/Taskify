import { FC } from "react";

interface LoadingProps {}

const Loading: FC<LoadingProps> = () => {
	return <span className="loading loading-spinner text-indigo-500"></span>;
};

export default Loading;
