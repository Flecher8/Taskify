import { FC, useState } from "react";
import "./loginPage.scss";

interface LoginPageProps {
    
}
 
const LoginPage: FC<LoginPageProps> = () => {
    const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
  };

  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(e.target.value);
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    // Handle login logic here
    console.log("Email:", email);
    console.log("Password:", password);
  };

  return (
    <div className="loginPage flex justify-center items-center">
      <div className="loginForm p-6 rounded-md shadow-md w-full max-w-md">
        <h2 className="text-2xl font-semibold text-center mb-6"><label>Welcome back!</label></h2>
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
                value={email}
                onChange={handleEmailChange}
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
                value={password}
                onChange={handlePasswordChange}
                className="w-full border border-gray-500 rounded-md pl-10 py-2"
                placeholder="Enter your password"
                required
              />
            </div>
          </div>
          <button
            type="submit"
            className="loginButton btn btn-primary text-white px-4 py-2 rounded-md w-full"
          >
            Log In
          </button>
        </form>
        <div className="mt-4 text-center">
          <p>
          <label>Don't have an account?{" "}</label>
            <a href="/signup" className="text-blue-500">
              Sign up
            </a>
          </p>
        </div>
      </div>
    </div>
  );
}
 
export default LoginPage;