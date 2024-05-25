import ProjectIncomesList from "components/projectIncomesList";
import { Project } from "entities/project";
import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import CompaniesStore from "stores/companiesStore";
import ProjectsStore from "stores/projectsStore";

interface CompanyPageProps {}

const CompanyPage: FC<CompanyPageProps> = () => {
	const { companyId } = useParams<{ companyId: string }>();
	const [projects, setProjects] = useState<Project[]>([]);
	const [isLoading, setIsLoading] = useState(true);

	useEffect(() => {
		const loadProjects = async () => {
			try {
				const company = await CompaniesStore.getCompanyById(companyId!);
				console.log(company);
				if (company) {
					const newProjects = await ProjectsStore.getProjectsByUserId(company.user.id);
					const sortedProjects = newProjects.sort((a, b) => {
						// Assuming createdAt is a Date object or a string that can be parsed to a Date
						return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime();
					});
					setProjects(sortedProjects);
				}
			} catch (error) {
				console.error(error);
			} finally {
				setIsLoading(false);
			}
		};

		loadProjects();
	}, [companyId]);

	return (
		<div className="flex flex-col m-5">
			<div className="text-2xl font-bold m-5">Incomes for each project</div>
			<ProjectIncomesList projects={projects} isLoading={isLoading} />
		</div>
	);
};

export default CompanyPage;
