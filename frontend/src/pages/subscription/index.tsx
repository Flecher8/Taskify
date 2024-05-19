import { FC, useEffect, useState } from "react";
import { Subscription } from "entities/subscription";
import subscriptionsStore from "stores/subscriptionsStore";
import authStore from "stores/authStore";
import userSubscriptionsStore from "stores/userSubscriptionsStore";

interface SubscriptionProps {}

const SubscriptionPage: FC<SubscriptionProps> = () => {
	const userId = authStore.userId;
	const [subscriptions, setSubscriptions] = useState<Subscription[]>([]);
	const [selectedSubscription, setSelectedSubscription] = useState<Subscription | null>(null);
	const [cardNumber, setCardNumber] = useState("");
	const [expiryDate, setExpiryDate] = useState("");
	const [cvv, setCvv] = useState("");
	const [errors, setErrors] = useState<{ [key: string]: string }>({});

	useEffect(() => {
		loadData();
	}, []);

	const handleSubscriptionChange = (subscription: Subscription) => {
		setSelectedSubscription(subscription);
	};

	const validateInputs = () => {
		let newErrors: { [key: string]: string } = {};

		if (cardNumber.replace(/\s+/g, "").length !== 16) {
			newErrors.cardNumber = "Card number must be 16 digits";
		}

		if (!/^\d{2}\/\d{2}$/.test(expiryDate)) {
			newErrors.expiryDate = "Expiry date must be in MM/YY format";
		}

		if (cvv.length !== 3) {
			newErrors.cvv = "CVV must be 3 digits";
		}

		setErrors(newErrors);

		return Object.keys(newErrors).length === 0;
	};

	const handleSubmit = async () => {
		if (selectedSubscription && validateInputs()) {
			try {
				if (userId === null) {
					return;
				}
				await userSubscriptionsStore.createUserSubscription(userId, selectedSubscription.id);
				window.location.href = "/projects";
			} catch (error) {
				console.error(error);
			}
		}
	};

	const loadData = async () => {
		try {
			const newSubscriptions = await subscriptionsStore.getAllSubscriptions();
			if (newSubscriptions) {
				// Sort subscriptions by pricePerMonth in ascending order
				const sortedSubscriptions = newSubscriptions.sort((a, b) => a.pricePerMonth - b.pricePerMonth);
				setSubscriptions(sortedSubscriptions);
			}
		} catch (error) {
			console.error(error);
		}
	};

	const handleExpiryDateChange = (value: string) => {
		if (value.length === 2 && !value.includes("/")) {
			setExpiryDate(value + "/");
		} else if (value.length <= 5) {
			setExpiryDate(value);
		}
	};

	const handleCardNumberChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		let value = e.target.value.replace(/\s+/g, ""); // Remove all spaces
		if (/^\d*$/.test(value) && value.length <= 16) {
			// Add a space every 4 digits
			value = value.match(/.{1,4}/g)?.join(" ") || value;
			setCardNumber(value);
		}
	};

	const handleInputChange = (setter: React.Dispatch<React.SetStateAction<string>>, maxLength: number) => {
		return (e: React.ChangeEvent<HTMLInputElement>) => {
			const { value } = e.target;
			if (/^\d*$/.test(value) && value.length <= maxLength) {
				setter(value);
			}
		};
	};

	return (
		<div
			className="flex flex-col items-center justify-center w-full p-4 bg-gray-100 "
			style={{
				backgroundImage: `url(${"https://tailwindcss.com/_next/static/media/docs-dark@tinypng.1bbe175e.png"})`
			}}>
			<div className="p-6 bg-transparent backdrop-blur-xl rounded-lg shadow-md w-full max-w-md">
				<h1 className="text-xl font-bold mb-4">Choose Your Subscription</h1>
				<div className="flex flex-row mb-4 space-x-2 overflow-x-auto">
					{subscriptions.map(subscription => (
						<div
							key={subscription.id}
							className={`p-4 border rounded cursor-pointer flex-shrink-0 ${
								selectedSubscription?.id === subscription.id ? "border-blue-500" : "border-gray-300"
							}`}
							onClick={() => handleSubscriptionChange(subscription)}>
							<h2 className="text-lg font-semibold">{subscription.name}</h2>
							<p className="text-gray-600 text-sm">${subscription.pricePerMonth.toFixed(2)} / month</p>
						</div>
					))}
				</div>
				<h2 className="text-lg font-bold mb-2">Enter Your Card Details</h2>
				<div className="w-full mb-4">
					<label className="block mb-1 text-sm font-semibold">Card Number</label>
					<input
						type="text"
						className="w-full p-2 text-sm border rounded mb-2"
						placeholder="Card Number"
						value={cardNumber}
						onChange={handleCardNumberChange}
					/>
					{errors.cardNumber && <p className="text-red-500 text-xs">{errors.cardNumber}</p>}
					<div className="flex space-x-2">
						<div className="w-1/2">
							<label className="block mb-1 text-sm font-semibold">Expiry Date (MM/YY)</label>
							<input
								type="text"
								className="w-full p-2 text-sm border rounded mb-2"
								placeholder="MM/YY"
								value={expiryDate}
								onChange={e => handleExpiryDateChange(e.target.value)}
							/>
							{errors.expiryDate && <p className="text-red-500 text-xs">{errors.expiryDate}</p>}
						</div>
						<div className="w-1/2">
							<label className="block mb-1 text-sm font-semibold">CVV</label>
							<input
								type="password"
								className="w-full p-2 text-sm border rounded mb-2"
								placeholder="CVV"
								value={cvv}
								onChange={handleInputChange(setCvv, 3)}
							/>
							{errors.cvv && <p className="text-red-500 text-xs">{errors.cvv}</p>}
						</div>
					</div>
				</div>
				<button className="w-full bg-blue-500 text-white p-2 rounded text-sm" onClick={handleSubmit}>
					Subscribe
				</button>
			</div>
		</div>
	);
};

export default SubscriptionPage;
