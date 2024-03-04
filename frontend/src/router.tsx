import { BrowserRouter, Routes, Route } from "react-router-dom";
import NotFound from "components/notFound";
import Layout from "Layout";
import PublicLayout from "components/layouts/publicLayout";
import HeroPage from "pages/HeroPage";
import PricingPage from "pages/PricingPage";
import GlassComponent from "GlassComponent";

const Router = () => {
	return (
		<BrowserRouter>
			<Routes>
				<Route path="/" element={<PublicLayout showHeader={true} />}>
					<Route index element={<HeroPage/>} />
					<Route path="pricing" element={<PricingPage/>} />
					{/* More nested routes under "/" */}
				</Route>
				<Route path="/login" element={<Layout showHeader={true} showMenu={false} />}>
				{/* <Route index element={<AuthorizationPage/>} /> */}
				</Route>
				<Route path="/glass" element={<GlassComponent />}>
				{/* <Route index element={<AuthorizationPage/>} /> */}
				</Route>
				<Route path="*" element={<NotFound />} />
			</Routes>
		</BrowserRouter>
	);
};

export default Router;
