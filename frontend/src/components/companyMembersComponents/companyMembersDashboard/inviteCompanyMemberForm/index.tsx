import { FC, useState } from "react";

interface InviteCompanyMemberFormProps {
	invite: (email: string) => void;
	close: () => void;
}

const InviteCompanyMemberForm: FC<InviteCompanyMemberFormProps> = ({ invite, close }) => {
	const [email, setEmail] = useState("");

	const handleEmailChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setEmail(event.target.value);
	};

	const handleCreateCompanyRole = () => {
		invite(email);
		close();
	};

	return (
		<div>
			<div>
				<h2 className="text-xl font-bold mb-4">Invite user to company</h2>
			</div>
			<div className="mb-4">
				<input
					type="text"
					id="name"
					className="w-full border rounded p-2"
					placeholder="example@site.com"
					value={email}
					onChange={handleEmailChange}
				/>
			</div>
			<button
				className="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600"
				onClick={handleCreateCompanyRole}>
				Invite
			</button>
		</div>
	);
};

export default InviteCompanyMemberForm;
