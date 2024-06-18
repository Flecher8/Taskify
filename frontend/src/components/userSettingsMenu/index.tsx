import { User } from "entities/user";
import { FC } from "react";
import { Link } from "react-router-dom";
import authStore from "stores/authStore";

interface UserSettingsMenuProps {
	user: User | undefined;
}

const UserSettingsMenu: FC<UserSettingsMenuProps> = ({ user }) => {
	const handleLogout = async () => {
		try {
			await authStore.logout();
		} catch (error) {
			console.error("Error during logout:", error);
		}
	};

	return (
		<div className="">
			{user && (
				<div className="pb-4 mt-4 text-gray-700 border-b border-gray-200 flex items-center justify-center">
					{user.firstName} {user.lastName}
				</div>
			)}
			<Link
				to="/profile"
				className="grid grid-cols-6 gap-5 items-center hover:bg-gray-100 m-1 pt-1 px-2 rounded text-gray-700">
				<div className="col-span-1 flex items-center">
					<i className="fa-light fa-user"></i>
				</div>
				<div className="col-span-5 flex items-center">Profile</div>
			</Link>
			<div
				className="grid grid-cols-6 gap-5 items-center hover:bg-gray-100  m-1 pt-1 px-2 rounded cursor-pointer"
				onClick={handleLogout}>
				<div className="col-span-1 flex items-center text-gray-700">
					<i className="fa-light fa-right-from-bracket"></i>
				</div>
				<div className="col-span-5 flex items-center text-gray-700">Log out</div>
			</div>
		</div>
	);
};

export default UserSettingsMenu;
