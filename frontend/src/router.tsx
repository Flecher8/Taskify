import { BrowserRouter, Routes, Route } from "react-router-dom";
import NotFound from "components/notFound";
import Layout from "Layout";
import PublicLayout from "components/layouts/publicLayout";
import HeroPage from "pages/HeroPage";
import PricingPage from "pages/PricingPage";
import GlassComponent from "GlassComponent";
import AuthLayout from "components/layouts/authLayout";
import LoginPage from "pages/LoginPage";
import SignUpPage from "pages/SignUpPage";

const Router = () => {
	return (
		<BrowserRouter>
			<Routes>
				<Route path="/" element={<PublicLayout showHeader={true} />}>
					<Route index element={<HeroPage/>} />
					<Route path="pricing" element={<PricingPage/>} />
					{/* More nested routes under "/" */}
				</Route>
				<Route path="/login" element={<AuthLayout showLoginHeader={true} />}>
					<Route index element={<LoginPage/>} />
				</Route>
				<Route path="/signup" element={<AuthLayout showLoginHeader={false} />}>
					<Route index element={<SignUpPage/>} />
				</Route>
				<Route path="/glass" element={<GlassComponent />}>
				{/* <Route index element={<AuthLayout showLoginHeader={true} />} /> */}
				</Route>
				<Route path="*" element={<NotFound />} />
			</Routes>
		</BrowserRouter>
	);
};

export default Router;
