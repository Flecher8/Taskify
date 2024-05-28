import { Project } from "entities/project";
import { ProjectMember } from "entities/projectMember";
import { FC, useEffect, useState } from "react";
import ProjectsStore from "stores/projectsStore";
import ProjectMembersStore from "stores/projectMembersStore";
import TimeStatisticsStore from "stores/timeStatisticsStore";
import { User } from "entities/user";
import ProjectWorkloadDate from "components/projectWorkloadDate";
import { UserTimeSpendOnDateStatistic } from "entities/statistics/time/userTimeSpendOnDateStatistic";
import { UserTimeSpendOnDateRequest } from "entities/statistics/time/userTimeSpendOnDateRequest";
import { getCurrentWeekDates } from "utilities/getCurrentWeekDates";
import TimeSpentIndicator from "components/timeSpentIndicator";
import Loading from "components/loading";

interface ProjectWorkloadDashboardProps {
	projectId: string;
}

const ProjectWorkloadDashboard: FC<ProjectWorkloadDashboardProps> = ({ projectId }) => {
	const [project, setProject] = useState<Project | null>(null);
	const [members, setMembers] = useState<User[]>([]);
	const [timeStatistics, setTimeStatistics] = useState<{ [key: string]: { [date: string]: number } }>({});
	const [selectedDate, setSelectedDate] = useState(new Date());
	const [loading, setLoading] = useState<boolean>(true);

	const weekDates = getCurrentWeekDates(selectedDate);

	useEffect(() => {
		loadData();
	}, [selectedDate]);

	const loadData = async () => {
		setLoading(true);
		try {
			const [projectData, projectMembersData] = await Promise.all([
				ProjectsStore.getProjectById(projectId),
				ProjectMembersStore.getProjectMembersByProjectId(projectId)
			]);

			const allUsers: User[] = [];
			if (projectData && projectData.user) {
				allUsers.push(projectData.user);
			}
			projectMembersData.forEach(member => {
				if (member.user) {
					allUsers.push(member.user);
				}
			});

			setProject(projectData);
			setMembers(allUsers);

			// Fetch time statistics for each member for the week
			const statistics: { [key: string]: { [date: string]: number } } = {};

			await Promise.all(
				allUsers.map(async user => {
					const userStats: { [date: string]: number } = {};

					await Promise.all(
						weekDates.map(async date => {
							const requestDto: UserTimeSpendOnDateRequest = {
								userId: user.id,
								projectId: projectId,
								date: date
							};
							const stat = await TimeStatisticsStore.getUserProjectTimeStatistics(requestDto);
							userStats[date.toISOString()] = stat ? stat.secondsSpend : 0;
						})
					);

					statistics[user.id] = userStats;
				})
			);

			setTimeStatistics(statistics);
		} catch (error) {
			console.error("Error loading project data:", error);
		}
		setLoading(false);
	};

	const handleDateChange = (direction: "prev" | "next") => {
		const newDate = new Date(selectedDate);
		newDate.setDate(selectedDate.getDate() + (direction === "prev" ? -7 : 7));
		setSelectedDate(newDate);
	};

	const handleTodayClick = () => {
		if (selectedDate.toDateString() === new Date().toDateString()) return;

		setSelectedDate(new Date());
	};

	const isWeekend = (date: Date) => {
		const dayOfWeek = date.getDay();
		return dayOfWeek === 0 || dayOfWeek === 6; // Sunday (0) or Saturday (6)
	};

	return (
		<div className="flex flex-col w-full h-full overflow-x-auto">
			<div className="flex flex-row gap-5 w-full h-16 items-center text-gray-500 pl-10 border-b border-gray-200">
				<div
					className="flex justify-center items-center rounded-full bg-white px-2 border border-gray-300 cursor-pointer"
					onClick={handleTodayClick}>
					Today
				</div>
				<div className="flex flex-row justify-center items-center gap-3">
					<div
						className="flex justify-center items-center rounded-full hover:cursor-pointer"
						onClick={() => handleDateChange("prev")}>{`<`}</div>
					<div
						className="flex justify-center items-center rounded-full hover:cursor-pointer"
						onClick={() => handleDateChange("next")}>{`>`}</div>
				</div>
			</div>
			<div className="grid grid-cols-9 w-full">
				<div className="col-span-2 border-b border-r border-gray-200 p-2">Members</div>
				{weekDates.map((date, index) => (
					<div
						key={index}
						className={`col-span-1 border-b border-gray-200 p-2 text-center ${
							index < weekDates.length - 1 && date.getMonth() !== weekDates[index + 1].getMonth()
								? "border-r"
								: ""
						}`}>
						<ProjectWorkloadDate
							date={date}
							showMonth={index === 0 || date.getDate() === 1 || date.getDay() === 1}
							selected={date.toDateString() === new Date().toDateString()}
						/>
					</div>
				))}
			</div>
			{loading ? (
				<div className="flex justify-center items-center w-full h-full">
					<Loading />
				</div>
			) : (
				<>
					{members.map((member, index) => (
						<div key={index} className="grid grid-cols-9 w-full">
							<div className="col-span-2 border-b border-r border-gray-200 p-2">{`${member.firstName} ${member.lastName}`}</div>
							{weekDates.map((date, dateIndex) => (
								<div
									key={dateIndex}
									className="col-span-1 border-b border-r border-gray-200 p-2 text-center h-20">
									<TimeSpentIndicator
										secondsSpent={timeStatistics[member.id]?.[date.toISOString()] || 0}
										normalWorkingHoursPerDay={project?.normalWorkingHoursPerDay || 8}
										isActive={!isWeekend(date)} // Set isActive based on whether the date is a weekend or not
									/>
								</div>
							))}
						</div>
					))}
					<div className="grid grid-cols-9 w-full">
						<div className="col-span-2 border-b border-r border-gray-200 p-2 font-semibold"></div>
						{weekDates.map((date, index) => (
							<div
								key={index}
								className="col-span-1 border-b border-r border-gray-200 p-2 text-center text-gray-500">
								{project?.normalWorkingHoursPerDay} h
							</div>
						))}
					</div>
				</>
			)}
		</div>
	);
};
export default ProjectWorkloadDashboard;
