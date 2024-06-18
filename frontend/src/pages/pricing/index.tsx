import { FC } from "react";

interface PricingPageProps {}

const PricingPage: FC<PricingPageProps> = () => {
	return (
		<div className="container mx-auto px-4 py-8">
			<h1 className="text-3xl font-bold text-center mb-8">Pricing Plans for Teams of All Sizes</h1>
			<div className="flex justify-center">
				{/* Free Plan */}
				<div className="pricing-card bg-white shadow-lg rounded-lg p-6 m-4 w-80">
					<h2 className="text-lg font-bold mb-4">Free</h2>
					<p className="text-sm text-center mb-4">Basic features for small teams</p>
					<p className="text-center text-gray-600">Free</p>
					<ul className="text-sm mb-4">
						<li className="flex items-center">
							<svg
								className="w-4 h-4 mr-2 text-green-500"
								viewBox="0 0 24 24"
								fill="none"
								stroke="currentColor"
								strokeWidth="2"
								strokeLinecap="round"
								strokeLinejoin="round">
								<path d="M5 13l4 4L19 7" />
							</svg>
							Basic Features
						</li>
						<li className="flex items-center">
							<svg
								className="w-4 h-4 mr-2 text-green-500"
								viewBox="0 0 24 24"
								fill="none"
								stroke="currentColor"
								strokeWidth="2"
								strokeLinecap="round"
								strokeLinejoin="round">
								<path d="M5 13l4 4L19 7" />
							</svg>
							Limited Storage
						</li>
					</ul>
					<button className="btn btn-primary w-full">Choose Plan</button>
				</div>

				{/* Pro Plan */}
				<div className="pricing-card bg-white shadow-lg rounded-lg p-6 m-4 w-80">
					<h2 className="text-lg font-bold mb-4">Pro</h2>
					<p className="text-sm text-center mb-4">All features for growing teams</p>
					<p className="text-center text-gray-600">$30/month</p>
					<ul className="text-sm mb-4">
						<li className="flex items-center">
							<svg
								className="w-4 h-4 mr-2 text-green-500"
								viewBox="0 0 24 24"
								fill="none"
								stroke="currentColor"
								strokeWidth="2"
								strokeLinecap="round"
								strokeLinejoin="round">
								<path d="M5 13l4 4L19 7" />
							</svg>
							All Features
						</li>
						<li className="flex items-center">
							<svg
								className="w-4 h-4 mr-2 text-green-500"
								viewBox="0 0 24 24"
								fill="none"
								stroke="currentColor"
								strokeWidth="2"
								strokeLinecap="round"
								strokeLinejoin="round">
								<path d="M5 13l4 4L19 7" />
							</svg>
							Unlimited Storage
						</li>
						<li className="flex items-center">
							<svg
								className="w-4 h-4 mr-2 text-green-500"
								viewBox="0 0 24 24"
								fill="none"
								stroke="currentColor"
								strokeWidth="2"
								strokeLinecap="round"
								strokeLinejoin="round">
								<path d="M5 13l4 4L19 7" />
							</svg>
							Advanced Features Included
						</li>
					</ul>
					<button className="btn btn-primary w-full">Choose Plan</button>
				</div>

				{/* Enterprise Plan */}
				<div className="pricing-card bg-white shadow-lg rounded-lg p-6 m-4 w-80">
					<h2 className="text-lg font-bold mb-4">Enterprise</h2>
					<p className="text-sm text-center mb-4">Customized features for large teams</p>
					<div className="flex flex-row">
						<p className="text-4xl">$150</p>
						<p className="text-center">/month</p>
					</div>
					<ul className="text-sm mb-4">
						<li className="flex items-center">
							<svg
								className="w-4 h-4 mr-2 text-green-500"
								viewBox="0 0 24 24"
								fill="none"
								stroke="currentColor"
								strokeWidth="2"
								strokeLinecap="round"
								strokeLinejoin="round">
								<path d="M5 13l4 4L19 7" />
							</svg>
							Custom Features Available
						</li>
						<li className="flex items-center">
							<svg
								className="w-4 h-4 mr-2 text-green-500"
								viewBox="0 0 24 24"
								fill="none"
								stroke="currentColor"
								strokeWidth="2"
								strokeLinecap="round"
								strokeLinejoin="round">
								<path d="M5 13l4 4L19 7" />
							</svg>
							Unlimited Storage
						</li>
						<li className="flex items-center">
							<svg
								className="w-4 h-4 mr-2 text-green-500"
								viewBox="0 0 24 24"
								fill="none"
								stroke="currentColor"
								strokeWidth="2"
								strokeLinecap="round"
								strokeLinejoin="round">
								<path d="M5 13l4 4L19 7" />
							</svg>
							Dedicated Support
						</li>
					</ul>
					<button className="btn btn-primary w-full">Choose Plan</button>
				</div>
			</div>
		</div>
	);
};
export default PricingPage;
