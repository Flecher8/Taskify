import { FC } from "react";
import { Link } from "react-router-dom";

interface HeroPageProps {}

const HeroPage: FC<HeroPageProps> = () => {
	return (
		<div className="hero h-full bg-white text-gray-800 overflow-auto custom-scroll-base">
			<div className="flex flex-col">
				<div className="max-w-lg flex flex-col justify-center items-center">
					<h1 className="text-5xl font-bold">Welcome to Taskify!</h1>
					<div className="w-3/4 flex flex-col justify-center">
						<p className="py-4 text-center">Get everything you need in one app!</p>
						<Link to="/signup" className="">
							<button className="w-full btn btn-primary bg-gradient-to-r from-indigo-500 from-10% to-purple-500 to-50% text-white text-2xl font-semibold">
								Get Started
							</button>
						</Link>
					</div>
				</div>
			</div>
		</div>
	);
};

export default HeroPage;
