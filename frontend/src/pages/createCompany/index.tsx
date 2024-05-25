import Loading from "components/loading";
import { FC, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import authStore from "stores/authStore";
import companiesStore from "stores/companiesStore";
import userSubscriptionsStore from "stores/userSubscriptionsStore";

interface CreateCompanyPageProps {}

const CreateCompanyPage: FC<CreateCompanyPageProps> = () => {
	const navigate = useNavigate();
	const [loading, setLoading] = useState(true);
	const [companyName, setCompanyName] = useState("");
	const [error, setError] = useState<string | null>(null);

	useEffect(() => {
		const checkAccess = async () => {
			if (!authStore.isAuth || authStore.userId === null) {
				navigate("/login");
				return;
			}

			const subscription = await userSubscriptionsStore.getUserSubscription(authStore.userId);
			if (!subscription || !subscription.canCreateCompany) {
				navigate("/subscription");
				return;
			}

			const company = await companiesStore.getCompanyByUserId(authStore.userId);
			if (company) {
				navigate(`/company/${company.id}`);
			} else {
				setLoading(false);
			}
		};

		checkAccess();
	}, [navigate]);

	const handleCreateCompany = async () => {
		if (companyName.trim() === "") {
			setError("Company name is required");
			return;
		}

		try {
			if (authStore.userId === null) return;
			const newCompany = await companiesStore.createCompany({
				name: companyName,
				userId: authStore.userId
			});
			if (newCompany) {
				navigate(`/company/${newCompany.id}`);
			}
		} catch (error) {
			setError("Failed to create company. Please try again.");
		}
	};

	if (loading) {
		return (
			<div className="w-full h-full">
				<Loading />
			</div>
		);
	}

	return (
		<div
			className="flex flex-col items-center justify-center w-full p-4 bg-gray-100 h-full"
			style={{
				backgroundImage: `url(${"https://tailwindcss.com/_next/static/media/docs-dark@tinypng.1bbe175e.png"})`
			}}>
			<div className="p-6 bg-transparent backdrop-blur-xl rounded-lg shadow-md w-full max-w-md">
				<h1 className="text-xl font-bold mb-4">Create Your Company</h1>
				<div className="w-full mb-4">
					<label className="block mb-1 text-sm font-semibold">Company Name</label>
					<input
						type="text"
						className="w-full p-2 text-sm border rounded mb-2"
						placeholder="Enter company name"
						value={companyName}
						onChange={e => setCompanyName(e.target.value)}
					/>
					{error && <p className="text-red-500 text-xs">{error}</p>}
				</div>
				<button className="w-full bg-blue-500 text-white p-2 rounded text-sm" onClick={handleCreateCompany}>
					Create Company
				</button>
			</div>
		</div>
	);
};

export default CreateCompanyPage;
