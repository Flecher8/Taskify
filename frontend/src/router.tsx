import { BrowserRouter, Routes, Route } from "react-router-dom";
import NotFound from "components/notFound";
import Layout from "Layout";

const Router = () => {
	return (
		<BrowserRouter>
			<Routes>
				<Route path="/" element={<Layout showHeader={true} showMenu={true} />}>
					<Route index element={<div>Home</div>} />
					<Route path="home" element={<div>Home1</div>} />
					{/* More nested routes under "/" */}
				</Route>
				<Route path="/blog" element={<Layout showHeader={true} showMenu={false} />}>
					<Route index element={<div>Blog Home</div>} />
					<Route path="firstBlog" element={<div>First Blog Post</div>} />
					{/* More nested routes under "/blog" */}
				</Route>
				<Route path="*" element={<NotFound />} />
			</Routes>
		</BrowserRouter>
	);
};

export default Router;
