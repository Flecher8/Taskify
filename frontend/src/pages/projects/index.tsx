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
		<div className="projectsPage flex flex-col w-full m-10">
			<div className="flex flex-row justify-normal mb-10 items-center">
				<h1 className="projectsPage-header mr-5">Projects</h1>
				<div className="dropdown">
					<div
						tabIndex={0}
						role="button"
						className="buttonToOpenCreateProject p-2"
						onClick={toggleCreateFormVisibility}>
						New
					</div>
					<CreateProjectForm
						onCreate={handleCreateProject}
						isVisible={showCreateForm}
						closeComponent={toggleCreateFormVisibility}
					/>
				</div>
			</div>
			<div className="w-full">
				<ProjectsList projects={projects} isLoading={isLoading} />
			</div>
			<div className="flex flex-row justify-normal mb-10 items-center">
				<h1 className="projectsPage-header mr-5">Projects membership</h1>
			</div>
			<div className="w-full">
				<ProjectsList projects={projectsMembership} isLoading={isLoading} />
			</div>
		</div>
	);
});

export default ProjectsPage;
