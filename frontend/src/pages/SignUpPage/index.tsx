import { FC, useState } from "react";
import "./signUpPage.scss";

interface SignUpPageProps {}

const SignUpPage: FC<SignUpPageProps> = () => {
	const [firstName, setFirstName] = useState("");
	const [lastName, setLastName] = useState("");
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [firstNameError, setFirstNameError] = useState("");
	const [lastNameError, setLastNameError] = useState("");
	const [emailError, setEmailError] = useState("");
	const [passwordError, setPasswordError] = useState("");

	const handleFirstNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const value = e.target.value;
		setFirstName(value);
		if (!value.trim()) {
			setFirstNameError("First name is required!");
		} else {
			setFirstNameError("");
		}
	};

	const handleLastNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const value = e.target.value;
		setLastName(value);
		if (!value.trim()) {
			setLastNameError("Last name is required!");
		} else {
			setLastNameError("");
		}
	};

	const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const value = e.target.value;
		setEmail(value);
		if (!value.trim()) {
			setEmailError("Email is required!");
		} else if (!isValidEmail(value)) {
			setEmailError("This email is invalid!");
		} else {
			setEmailError("");
		}
	};

	const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const value = e.target.value;
		setPassword(value);

		let errorMessage = "";

		if (value.length < 6) {
			errorMessage = "Password must be 6 characters or longer!";
		} else if (!/[A-Z]/.test(value)) {
			errorMessage = "Please use at least one capital letter!";
		} else if (!/\d/.test(value)) {
			errorMessage = "Please use at least one number!";
		} else if (!/[@$!%*?&]/.test(value)) {
			errorMessage = "Please use at least one special character!";
		}

		setPasswordError(errorMessage);
	};

	const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
		e.preventDefault();
		// Perform form submission logic
		if (!firstNameError && !lastNameError && !emailError && !passwordError) {
			console.log("Form submitted successfully!");
		}
	};

	const isValidEmail = (email: string): boolean => {
		// Basic email validation
		const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
		return emailRegex.test(email);
	};

	return (
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
									value={firstName}
									onChange={handleFirstNameChange}
									className="w-full border border-gray-300 rounded-md pl-10 py-2"
									placeholder="John"
									required
								/>
							</div>
							{firstNameError && (
								<p className="error">
									<i className="fa-solid fa-triangle-exclamation"></i> {firstNameError}
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
									value={lastName}
									onChange={handleLastNameChange}
									className="w-full border border-gray-300 rounded-md pl-10 py-2"
									placeholder="Doe"
									required
								/>
							</div>
							{lastNameError && (
								<p className="error">
									<i className="fa-solid fa-triangle-exclamation"></i> {lastNameError}
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
								value={email}
								onChange={handleEmailChange}
								className="w-full border border-gray-300 rounded-md pl-10 py-2"
								placeholder="example@site.com"
								required
							/>
						</div>
						{emailError && (
							<p className="error">
								<i className="fa-solid fa-triangle-exclamation"></i> {emailError}
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
								value={password}
								onChange={handlePasswordChange}
								className="w-full border border-gray-500 rounded-md pl-10 py-2"
								placeholder="Minimum 6 characters"
								required
							/>
						</div>
						{passwordError && (
							<p className="error">
								<i className="fa-solid fa-triangle-exclamation"></i> {passwordError}
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
	);
};

export default SignUpPage;
