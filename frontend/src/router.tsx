import { useEffect } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import NotFound from "components/notFound";
import Layout from "Layout";
import PublicLayout from "components/layouts/publicLayout";
import HeroPage from "pages/hero";
import PricingPage from "pages/pricing";
import AuthLayout from "components/layouts/authLayout";
import LoginPage from "pages/login";
import SignUpPage from "pages/signUp";
import MainLayout from "components/layouts/mainLayout";
import ProjectsPage from "pages/projects";
import authStore from "stores/authStore";
import BoardPage from "pages/board";
import ProfilePage from "pages/profile";
import ProjectRolesPage from "pages/projectRoles";
import ProjectMembersPage from "pages/projectMembers";

const Router = () => {
	return (
		<BrowserRouter>
			<Routes>
				<Route path="/" element={<PublicLayout showHeader={true} />}>
					<Route index element={<HeroPage />} />
					<Route path="pricing" element={<PricingPage />} />
					{/* More nested routes under "/" */}
				</Route>
				<Route path="/login" element={<AuthLayout showLoginHeader={true} />}>
					<Route index element={<LoginPage />} />
				</Route>
				<Route path="/signup" element={<AuthLayout showLoginHeader={false} />}>
					<Route index element={<SignUpPage />} />
				</Route>

				<Route path="/billing" element={<MainLayout showHeader={true} showMenu={false} />}>
					<Route index element={<PricingPage />} />
				</Route>

				<Route path="/projects" element={<MainLayout showHeader={true} showMenu={false} />}>
					<Route index element={<ProjectsPage />} />
				</Route>

				<Route path="/project/:projectId" element={<MainLayout showHeader={true} showMenu={true} />}>
					<Route index element={<BoardPage />} />
					<Route path="roles" element={<ProjectRolesPage />} />
					<Route path="members" element={<ProjectMembersPage />} />
				</Route>

				<Route path="/profile" element={<MainLayout showHeader={true} showMenu={false} />}>
					<Route index element={<ProfilePage />} />
				</Route>

				<Route path="*" element={<NotFound />} />
			</Routes>
		</BrowserRouter>
	);
};

export default Router;
