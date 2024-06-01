import { User } from "entities/user";
import { FC, useState } from "react";

interface SelectUsersWithFilterProps {
	users: User[];
	onSelect: (user: User | null) => void;
	current: User | null;
}

const SelectUsersWithFilter: FC<SelectUsersWithFilterProps> = ({ users, onSelect, current }) => {
	const [filter, setFilter] = useState("");

	const filteredUsers = users.filter(
		user => user !== current && (user.firstName + " " + user.lastName).toLowerCase().includes(filter.toLowerCase())
	);

	const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
		setFilter(event.target.value);
	};

	const handleSelect = (user: User) => {
		if (user === current) {
			onSelect(null); // Deselect if already selected
		} else {
			onSelect(user);
		}
	};

	return (
		<div className="flex flex-col">
			<div className="flex mb-3">
				<input
					type="text"
					value={filter}
					onChange={handleInputChange}
					placeholder="Search by name..."
					className="border border-gray-300 rounded p-2 w-full"
				/>
			</div>
			{current && (
				<div className="mb-3">
					<h3>Current:</h3>
					<div
						className="p-2 border border-gray-300 rounded cursor-pointer trancate"
						onClick={() => onSelect(null)}>
						{current.firstName} {current.lastName}
					</div>
				</div>
			)}
			<div className="overflow-auto max-h-40 custom-scroll-sm p-1">
				<div className="pb-1">
					<h3>Members:</h3>
				</div>
				{filteredUsers.map(user => (
					<div
						key={user.id}
						className={`p-2 hover:bg-gray-200 cursor-pointer trancate ${current === user ? "bg-gray-200" : ""}`}
						onClick={() => handleSelect(user)}>
						{user.firstName} {user.lastName}
					</div>
				))}
			</div>
		</div>
	);
};

export default SelectUsersWithFilter;
