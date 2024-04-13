import { FC, useState } from "react";
import "./loginPage.scss";
import authStore from "stores/authStore";
import Error from "components/alerts/error";
import { observer } from "mobx-react-lite";

interface LoginPageProps {}

const LoginPage: FC<LoginPageProps> = () => {
	const [form, setForm] = useState({ email: "", password: "" });

	const [showError, setShowError] = useState(false);
	const [errorMessage, setErrorMessage] = useState("");

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		setForm({
			...form,
			[e.target.name]: e.target.value
		});
	};

	const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
		e.preventDefault();
		try {
			await authStore.login(form.email, form.password);
			if (authStore.isAuth) {
				window.location.href = "/projects";
			}
		} catch (err: any) {
			setErrorMessage(err.message);
			setShowError(true);
			setTimeout(() => {
				setShowError(false);
			}, 3000);
		}
	};

	const handleNewButtonClick = () => {
		// Handle button click logic here
		console.log(localStorage.getItem("userId"));
		console.log(authStore.isAuth);
	};

	return (
		<div>
			<div className="loginPage flex justify-center items-center">
				<div className="loginForm p-6 rounded-md shadow-md w-full max-w-md">
					<h2 className="text-2xl font-semibold text-center mb-6">
						<label>Welcome back!</label>
					</h2>
					<form onSubmit={handleSubmit}>
						<div className="mb-4">
							<label htmlFor="email" className="form-row-label block text-sm mb-1">
								Email:
							</label>
							<div className="relative">
								<i className="fa-light fa-envelope absolute top-1/2 left-2 transform -translate-y-1/2 text-gray-400"></i>
								<input
									type="email"
									id="email"
									name="email"
									value={form.email}
									onChange={handleChange}
									className="w-full border border-gray-300 rounded-md pl-10 py-2"
									placeholder="Enter your email"
									required
								/>
							</div>
						</div>
						<div className="mb-4">
							<label htmlFor="password" className="form-row-label block text-sm mb-1">
								Password:
							</label>
							<div className="relative">
								<i className="fa-light fa-lock absolute top-1/2 left-2 transform -translate-y-1/2 text-gray-400"></i>
								<input
									type="password"
									id="password"
									name="password"
									value={form.password}
									onChange={handleChange}
									className="w-full border border-gray-500 rounded-md pl-10 py-2"
									placeholder="Enter your password"
									required
								/>
							</div>
						</div>
						<button
							type="submit"
							className="authButton btn btn-primary text-white px-4 py-2 rounded-md max-h-2 w-full">
							Log In
						</button>
					</form>
					<div className="mt-4 text-center">
						<p>
							<label>Don't have an account? </label>
							<a href="/signup" className="text-blue-500">
								Sign up
							</a>
						</p>
					</div>
					{/* <button onClick={handleNewButtonClick} className="btn btn-secondary mt-4 w-full">
						New Button
					</button> */}
				</div>
			</div>
			<div className={`error-container ${showError ? "show" : ""}`}>
				{showError && <Error message={errorMessage} />}
			</div>
		</div>
	);
};

export default observer(LoginPage);
