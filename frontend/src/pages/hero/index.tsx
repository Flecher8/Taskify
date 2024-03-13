import { FC } from "react";
import { Link } from "react-router-dom";

interface HeroPageProps {}

const HeroPage: FC<HeroPageProps> = () => {
	return (
		<div className="hero min-h-screen bg-white text-gray-800">
			<div className="hero-content text-center">
				<div className="max-w-md">
					<h1 className="text-5xl font-bold">Welcome to Taskify</h1>
					<p className="py-6">
						Boost your productivity and stay organized with Taskify! Whether you're managing projects, tracking
						tasks, or collaborating with your team, Taskify has got you covered.
					</p>
					<Link to="/signup" className="hover:text-gray-300">
						<button className="btn btn-primary bg-gradient-to-r from-blue-500 to-purple-500 hover:from-blue-600 hover:to-purple-600">
							Get Started
						</button>
					</Link>
				</div>
			</div>
		</div>
	);
};

export default HeroPage;
