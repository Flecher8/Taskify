import { FC, useState } from "react";
import NotificationsList from "./notificationsList";
import "./notificationsDashboard.scss";

interface NotificationsDashboardProps {}

const NotificationsDashboard: FC<NotificationsDashboardProps> = () => {
	const [showOnlyUnreadNotifications, setShowOnlyUnreadNotifications] = useState(true);

	const switchCheckbox = () => {
		setShowOnlyUnreadNotifications(!showOnlyUnreadNotifications);
	};

	return (
		<div className="flex flex-col w-full h-96">
			<div className="w-full h-8 pb-8 border-b border-violet-900 p-3 flex flex-row justify-between items-center mt-2">
				<div>
					<h2 className="text-lg">Notifications</h2>
				</div>
				<div className="flex flex-row justify-end items-center">
					<input
						type="checkbox"
						className="toggle toggle-success toggle-xs"
						checked={showOnlyUnreadNotifications}
						onChange={switchCheckbox}
					/>
				</div>
			</div>
			<div className="w-full pt-3 overflow-auto custom-scroll-sm">
				<NotificationsList showOnlyUnreadNotifications={showOnlyUnreadNotifications} />
			</div>
		</div>
	);
};

export default NotificationsDashboard;
