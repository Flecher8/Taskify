import { CompanyInvitation } from "entities/companyInvitation";
import { Notification } from "entities/notification";
import { FC, useEffect, useState } from "react";
import companyInvitationsStore from "stores/companyInvitationsStore";

interface NotificationCompanyInvitationCardProps {
	notification: Notification;
	markAsRead: (id: string) => void;
}

const NotificationCompanyInvitationCard: FC<NotificationCompanyInvitationCardProps> = ({
	notification,
	markAsRead
}) => {
	const [companyInvitation, setCompanyInvitation] = useState<CompanyInvitation>();

	const loadCompanyInvitation = async () => {
		try {
			const newInvitation = await companyInvitationsStore.getInvitationByNotificationId(notification.id);
			if (newInvitation === undefined) {
				return;
			}

			setCompanyInvitation(newInvitation);
		} catch (error) {
			console.error(error);
		}
	};

	const handleAccepted = async () => {
		try {
			// Change local state
			setCompanyInvitation(prevInvitation => ({ ...prevInvitation!, isAccepted: true }));

			// Call API to accept invitation
			const result = await companyInvitationsStore.respondToCompanyInvitation(companyInvitation?.id!, true);
			console.log("Result: ", result);

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
			setCompanyInvitation(prevInvitation => ({ ...prevInvitation!, isAccepted: false }));

			// Call API to decline invitation
			await companyInvitationsStore.respondToCompanyInvitation(companyInvitation?.id!, false);

			if (!notification.isRead) {
				markAsRead(notification.id);
			}
		} catch (error) {
			console.error(error);
		}
	};

	useEffect(() => {
		loadCompanyInvitation();
	}, [notification]);

	return (
		<div className="flex flex-col w-full">
			<div className="text-lg border-b mb-2">
				<h1>Company invitation</h1>
			</div>
			<div className="text-xs">
				<div className="">
					You were invited to company «<span className="text-blue-300">{companyInvitation?.company.name}</span>»
				</div>
			</div>
			{companyInvitation === undefined ? (
				<div></div>
			) : companyInvitation?.isAccepted === null ? (
				<div className="flex flex-row pt-3 justify-center items-center">
					<div
						className="rounded-full w-8 h-8 border border-green-300 text-green-300 
						hover:bg-green-300 hover:text-white transition duration-300 mr-3 flex justify-center items-center"
						onClick={handleAccepted}>
						<i className="fa-light fa-check"></i>
					</div>
					<div
						className="rounded-full w-8 h-8 border border-red-300 text-red-300 
						hover:bg-red-300 hover:text-white transition duration-300 flex justify-center items-center"
						onClick={handleDeclined}>
						<i className="fa-light fa-xmark"></i>
					</div>
				</div>
			) : companyInvitation?.isAccepted === true ? (
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

export default NotificationCompanyInvitationCard;
