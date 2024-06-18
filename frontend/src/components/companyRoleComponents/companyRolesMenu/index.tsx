import { FC } from "react";

interface CompanyRolesMenuProps {}

const CompanyRolesMenu: FC<CompanyRolesMenuProps> = () => {
	return (
		<div className="flex flex-col items-center justify-center mt-5">
			<h2 className="text-3xl font-bold mb-6">Company roles</h2>
			<p className="text-lg text-balance mb-4">Here you can create new company roles for your company</p>
		</div>
	);
};

export default CompanyRolesMenu;
