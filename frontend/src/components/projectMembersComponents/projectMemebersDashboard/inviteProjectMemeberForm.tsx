import { FC, useState } from "react";

interface InviteProjectMemberFormProps {
	invite: (email: string) => void;
	close: () => void;
}

const InviteProjectMemberForm: FC<InviteProjectMemberFormProps> = ({ invite, close }) => {
	const [email, setEmail] = useState("");

	const handleEmailChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setEmail(event.target.value);
	};

	const handleCreateProjectRole = () => {
		invite(email);
		close();
	};

	return (
		<div>
			<div>
				<h2 className="text-xl font-bold mb-4">Invite user to project</h2>
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
				onClick={handleCreateProjectRole}>
				Invite
			</button>
		</div>
	);
};

export default InviteProjectMemberForm;
