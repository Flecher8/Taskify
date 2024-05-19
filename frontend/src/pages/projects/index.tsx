import { FC, useEffect, useState } from "react";
import "./projectsPage.scss";
import ProjectsList from "components/projectsList";
import userStore from "stores/userStore";
import { observer } from "mobx-react-lite";
import CreateProjectForm from "components/createProjectForm";
import projectsStore from "stores/projectsStore";
import authStore from "stores/authStore";
import { Project } from "entities/project";
import DropDownContext from "components/dropDownContext";
import { Link } from "react-router-dom";

interface ProjectsPageProps {}

const ProjectsPage: FC<ProjectsPageProps> = observer(() => {
	const userId = userStore.userId;
	const [showCreateForm, setShowCreateForm] = useState(false);

	const [projects, setProjects] = useState<Project[]>([]);
	const [projectsMembership, setProjectsMembership] = useState<Project[]>([]);

	const [isLoading, setIsLoading] = useState<boolean>(false);

	const handleCreateProject = async (name: string) => {
		try {
			toggleCreateFormVisibility();
			const result = await projectsStore.createProject(userId, name);
			setProjects(prev => [...prev, result]);
		} catch (error) {}
	};

	const toggleCreateFormVisibility = () => {
		setShowCreateForm(prevState => !prevState);
		console.log(localStorage);
	};

	const loadData = async () => {
		setIsLoading(true);
		try {
			getProjects();
			getProjectsMembership();
		} catch (error) {
			console.error("Error loading data:", error);
		} finally {
			setIsLoading(false);
		}
	};

	const getProjects = async () => {
		setIsLoading(true);
		try {
			const newProjects = await projectsStore.getProjectsByUserId(userId);
			const sortedProjects = newProjects.sort((a, b) => {
				// Assuming createdAt is a Date object or a string that can be parsed to a Date
				return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime();
			});
			setProjects(sortedProjects);
		} catch (error: any) {
			console.error("Error loading projects:", error);
		} finally {
			setIsLoading(false);
		}
	};

	const getProjectsMembership = async () => {
		setIsLoading(true);
		try {
			const newProjectsMembership = await projectsStore.getProjectsByMember(userId);
			const sortedProjectsMembership = newProjectsMembership.sort((a, b) => {
				// Assuming createdAt is a Date object or a string that can be parsed to a Date
				return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime();
			});
			setProjectsMembership(sortedProjectsMembership);
		} catch (error: any) {
			console.error("Error loading projects membership:", error);
		} finally {
			setIsLoading(false);
		}
	};

	useEffect(() => {
		loadData();
	}, []);

	return (
		<div className="flex flex-col items-center w-full h-full">
			<div className="flex flex-row md:w-3/4 lg:w-3/4 w-full h-full">
				<div className="flex flex-col w-1/4 pr-1 gap-1 border-r border-gray-200 pt-10">
					<Link
						to="/projects"
						className="flex items-center px-2 py-1 rounded-lg hover:bg-gray-300 transition duration-300 bg-indigo-100">
						<i className="fa-regular fa-square-kanban mr-2"></i>
						<span>Projects</span>
					</Link>
					<Link
						to="/company"
						className="flex items-center px-2 py-1 rounded-lg hover:bg-gray-300 transition duration-300">
						<i className="fa-light fa-building mr-2"></i>
						<span>Company</span>
					</Link>
				</div>
				<div className="flex flex-col w-3/4 p-4 overflow-y-auto">
					<div className="flex flex-row items-center mb-10">
						<h1 className="text-2xl font-bold mr-5">Projects</h1>
						<div className="relative">
							<button className="p-2 bg-indigo-500 text-white rounded-lg" onClick={toggleCreateFormVisibility}>
								New
							</button>
							{showCreateForm && (
								<div className="absolute mt-2">
									<CreateProjectForm
										onCreate={handleCreateProject}
										isVisible={showCreateForm}
										closeComponent={toggleCreateFormVisibility}
									/>
								</div>
							)}
						</div>
					</div>
					<div className="w-full">
						<ProjectsList projects={projects} isLoading={isLoading} />
					</div>
					<div className="flex flex-row items-center mb-10">
						<h1 className="text-2xl font-bold mr-5">Projects Membership</h1>
					</div>
					<div className="w-full">
						<ProjectsList projects={projectsMembership} isLoading={isLoading} />
					</div>
				</div>
			</div>
		</div>
	);
});

export default ProjectsPage;
