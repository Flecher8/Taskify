import { FC, useEffect, useState } from "react";
import UserStore from "stores/userStore";
import ProjectsStore from "stores/projectsStore";
import CompanyMembersStore from "stores/companyMembersStore";
import { User } from "entities/user";
import { Project } from "entities/project";
import { Company } from "entities/company";
import ProfileProjectList from "components/profileProjectList";
import ProfileCompanyList from "components/profileCompanyList";
import Loading from "components/loading";

interface ProfilePageProps {}

const ProfilePage: FC<ProfilePageProps> = () => {
	const [user, setUser] = useState<User | null>(null);
	const [projects, setProjects] = useState<Project[]>([]);
	const [companies, setCompanies] = useState<Company[]>([]);
	const [isLoading, setIsLoading] = useState<boolean>(true);

	useEffect(() => {
		const fetchUserData = async () => {
			try {
				const userId = UserStore.userId;
				if (userId) {
					const fetchedUser = await UserStore.getUserById(userId);
					setUser(fetchedUser || null);
					if (fetchedUser) {
						const userProjects = await ProjectsStore.getProjectsByMember(userId);
						setProjects(userProjects);

						const userCompanies = await CompanyMembersStore.getCompaniesByUserId(userId);
						setCompanies(userCompanies || []);
					}
				}
			} catch (error) {
				console.error("Error fetching user data:", error);
			} finally {
				setIsLoading(false);
			}
		};

		fetchUserData();
	}, []);

	const leaveProject = async (id: string) => {
		try {
			if (user?.id) {
				await CompanyMembersStore.leaveCompany(user.id, id);
				setProjects(projects.filter(project => project.id !== id));
			}
		} catch (error) {
			console.error("Error leaving project:", error);
		}
	};

	const leaveCompany = async (id: string) => {
		try {
			if (user?.id) {
				await CompanyMembersStore.leaveCompany(user.id, id);
				setCompanies(companies.filter(company => company.id !== id));
			}
		} catch (error) {
			console.error("Error leaving company:", error);
		}
	};

	if (isLoading) {
		return (
			<div className="flex justify-center items-center w-full h-full">
				<Loading />
			</div>
		);
	}

	if (!user) {
		return (
			<div className="flex justify-center items-center w-full h-full">
				<Loading />
			</div>
		);
	}

	return (
		<div className="flex flex-col items-center w-full h-full pt-10">
			<div className="bg-white p-8 rounded-lg shadow-lg drop-shadow-lg w-full max-w-lg">
				<div className="text-center mb-10">
					<div className="text-2xl font-bold">{`${user.firstName} ${user.lastName}`}</div>
					<div className="text-gray-600 text-sm">{user.email}</div>
				</div>
				<div className="flex mt-4 flex-col">
					<div className="text-lg font-semibold">Projects:</div>
					<ProfileProjectList projects={projects} leaveProject={leaveProject} />
				</div>
				<div className="flex mt-4 flex-col">
					<div className="text-lg font-semibold">Companies:</div>
					<ProfileCompanyList companies={companies} leaveCompany={leaveCompany} />
				</div>
			</div>
		</div>
	);
};

export default ProfilePage;
