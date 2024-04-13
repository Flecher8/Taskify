import { Notification } from "entities/notification";
import { ProjectInvitation } from "entities/projectInvitation";
import { FC, useEffect, useState } from "react";
import projectInvitationsStore from "stores/projectInvitationsStore";

interface NotificationProjectInvitationCardProps {
	notification: Notification;
	markAsRead: (id: string) => void;
}

const NotificationProjectInvitationCard: FC<NotificationProjectInvitationCardProps> = ({
	notification,
	markAsRead
}) => {
	const [projectInvitation, setProjectInvitation] = useState<ProjectInvitation>();

	const loadProjectInvitation = async () => {
		try {
			const newInvitation = await projectInvitationsStore.getInvitationByNotificationId(notification.id);
			if (newInvitation === undefined) {
				return;
			}
			setProjectInvitation(newInvitation);
		} catch (error) {
			console.error(error);
		}
	};

	const handleAccepted = async () => {
		try {
			// Change local state
			setProjectInvitation(prevInvitation => ({ ...prevInvitation!, isAccepted: true }));

			// Call API to accept invitation
			await projectInvitationsStore.respondToInvitation(projectInvitation?.id!, true);

			if (!notification.isRead) {
				markAsRead(notification.id);
			}
		} catch (error) {
			console.error(error);
		}
	};

	const handleDeclined = async () => {
		try {
			// Change local state
			setProjectInvitation(prevInvitation => ({ ...prevInvitation!, isAccepted: false }));

			// Call API to decline invitation
			await projectInvitationsStore.respondToInvitation(projectInvitation?.id!, false);

			if (!notification.isRead) {
				markAsRead(notification.id);
			}
		} catch (error) {
			console.error(error);
		}
	};

	useEffect(() => {
		loadProjectInvitation();
	}, [notification]);

	return (
		<div className="flex flex-col w-full">
			<div className="text-lg border-b mb-2">
				<h1>Project invitation</h1>
			</div>
			<div className="text-xs">
				<div className="">
					You were invited to project «<span className="text-blue-300">{projectInvitation?.project.name}</span>»
				</div>
			</div>
			{projectInvitation === undefined ? (
				<div></div>
			) : projectInvitation?.isAccepted === null ? (
				<div className="flex flex-row pt-3 justify-center items-center">
					<div
						className="rounded-full w-8 h-8 border border-green-300 text-green-300 
						hover:bg-green-300 hover:text-white trunsaction duration-300 mr-3 flex justify-center items-center"
						onClick={handleAccepted}>
						<i className="fa-light fa-check"></i>
					</div>
					<div
						className="rounded-full w-8 h-8 border border-red-300 text-red-300 
						hover:bg-red-300 hover:text-white trunsaction duration-300 flex justify-center items-center"
						onClick={handleDeclined}>
						<i className="fa-light fa-xmark"></i>
					</div>
				</div>
			) : projectInvitation?.isAccepted === true ? (
				<div className="rounded-full p-1 text-white bg-green-300 max-w-24 flex justify-center items-center mt-5">
					Accepted
				</div>
			) : (
				<div className="rounded-full p-1 text-white bg-red-300 max-w-24 flex justify-center items-center mt-5">
					Declined
				</div>
			)}
		</div>
	);
};

export default NotificationProjectInvitationCard;
