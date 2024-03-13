import { FC, useState } from "react";
import "./signUpPage.scss";
import authStore from "stores/authStore";
import Error from "../../components/alerts/error";
import { observer } from "mobx-react-lite";

interface SignUpPageProps {}

const SignUpPage: FC<SignUpPageProps> = observer(() => {
	const [form, setForm] = useState({
		firstName: "",
		lastName: "",
		email: "",
		password: ""
	});

	const [errors, setErrors] = useState({
		firstName: "",
		lastName: "",
		email: "",
		password: ""
	});

	const [showError, setShowError] = useState(false);
	const [errorMessage, setErrorMessage] = useState("");

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const { name, value } = e.target;
		setForm({ ...form, [name]: value });
		validateField(name, value);
	};

	const validateField = (name: string, value: string) => {
		let errorMessage = "";

		switch (name) {
			case "firstName":
				errorMessage = value.trim() ? "" : "First name is required!";
				break;
			case "lastName":
				errorMessage = value.trim() ? "" : "Last name is required!";
				break;
			case "email":
				errorMessage = value.trim() ? (isValidEmail(value) ? "" : "This email is invalid!") : "Email is required!";
				break;
			case "password":
				errorMessage = validatePassword(value);
				break;
			default:
				break;
		}

		setErrors({ ...errors, [name]: errorMessage });
	};

	const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
		e.preventDefault();

		// Perform form submission logic
		const hasErrors = Object.values(errors).some(error => error !== "");
		if (hasErrors) {
			setErrorMessage("Not all data completed.");
			setShowError(true);
			setTimeout(() => {
				setShowError(false);
			}, 3000); // Hide message after 3 seconds
			return;
		}

		try {
			await authStore.register(form);
			if (authStore.isAuth) {
				window.location.href = "/projects";
			}
		} catch (err: any) {
			setErrorMessage(err.message);
			setShowError(true);
			setTimeout(() => {
				setShowError(false);
			}, 3000); // Hide message after 3 seconds
		}
	};

	const isValidEmail = (email: string): boolean => {
		// Basic email validation
		const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
		return emailRegex.test(email);
	};

	const validatePassword = (password: string): string => {
		if (password.length < 6) {
			return "Password must be 6 characters or longer!";
		}
		if (!/[A-Z]/.test(password)) {
			return "Please use at least one capital letter!";
		}
		if (!/\d/.test(password)) {
			return "Please use at least one number!";
		}
		if (!/[@$!%*?&]/.test(password)) {
			return "Please use at least one special character!";
		}
		return "";
	};

	return (
		<div>
			<div className="signUpPage flex justify-center items-center">
				<div className="signUpForm p-6 rounded-md shadow-md w-full max-w-md">
					<h2 className="text-2xl font-semibold text-center mb-6">
						<label>Let's go!</label>
					</h2>
					<form onSubmit={handleSubmit}>
						<div className="mb-4 flex flex-col lg:flex-row">
							<div className="w-full lg:w-1/2 lg:pr-2">
								<label htmlFor="firstName" className="form-row-label block text-sm mb-1">
									First name:
								</label>
								<div className="relative">
									<i className="fa-light fa-user absolute top-1/2 left-2 transform -translate-y-1/2 text-gray-400"></i>
									<input
										type="text"
										id="firstName"
										name="firstName"
										value={form.firstName}
										onChange={handleChange}
										className="w-full border border-gray-300 rounded-md pl-10 py-2"
										placeholder="John"
										required
									/>
								</div>
								{errors.firstName && (
									<p className="error">
										<i className="fa-solid fa-triangle-exclamation"></i> {errors.firstName}
									</p>
								)}
							</div>
							<div className="w-full lg:w-1/2 lg:pl-2 mt-4 lg:mt-0">
								<label htmlFor="lastName" className="form-row-label block text-sm mb-1">
									Last name:
								</label>
								<div className="relative">
									<i className="fa-light fa-user absolute top-1/2 left-2 transform -translate-y-1/2 text-gray-400"></i>
									<input
										type="text"
										id="lastName"
										name="lastName"
										value={form.lastName}
										onChange={handleChange}
										className="w-full border border-gray-300 rounded-md pl-10 py-2"
										placeholder="Doe"
										required
									/>
								</div>
								{errors.lastName && (
									<p className="error">
										<i className="fa-solid fa-triangle-exclamation"></i> {errors.lastName}
									</p>
								)}
							</div>
						</div>
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
									placeholder="example@site.com"
									required
								/>
							</div>
							{errors.email && (
								<p className="error">
									<i className="fa-solid fa-triangle-exclamation"></i> {errors.email}
								</p>
							)}
						</div>
						<div className="mb-4">
							<label htmlFor="password" className="form-row-label block text-sm mb-1">
								Choose password:
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
									placeholder="Minimum 6 characters"
									required
								/>
							</div>
							{errors.password && (
								<p className="error">
									<i className="fa-solid fa-triangle-exclamation"></i> {errors.password}
								</p>
							)}
						</div>
						<button
							type="submit"
							className="authButton btn btn-primary text-white px-4 py-2 rounded-md max-h-2 w-full">
							Sign Up
						</button>
					</form>
				</div>
			</div>
			<div className={`error-container ${showError ? "show" : ""}`}>
				{showError && <Error message={errorMessage} />}
			</div>
		</div>
	);
});

export default SignUpPage;
