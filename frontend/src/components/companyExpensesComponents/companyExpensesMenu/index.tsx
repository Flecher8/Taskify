import React from "react";

interface CompanyExpensesMenuProps {}

const CompanyExpensesMenu: React.FC<CompanyExpensesMenuProps> = () => {
	return (
		<div className="flex flex-col items-center justify-center mt-5">
			<h2 className="text-3xl font-bold mb-6">Company Expenses</h2>
			<p className="text-lg text-balance mb-4">Here you can manage expenses for your company</p>
		</div>
	);
};

export default CompanyExpensesMenu;
