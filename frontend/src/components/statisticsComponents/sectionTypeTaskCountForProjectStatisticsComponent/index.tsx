import { SectionType } from "entities/section";
import { SectionTypeTaskCountStatistics } from "entities/statistics/task/sectionTypeTaskCountStatistics";
import { FC, useEffect, useState } from "react";
import taskStatisticsStore from "stores/taskStatisticsStore";

interface SectionTypeTaskCountForProjectStatisticsComponentProps {
	projectId: string;
}

interface SectionTypeInfo {
	type: SectionType;
	color: string;
	text: string;
}

const SectionTypeTaskCountForProjectStatisticsComponent: FC<SectionTypeTaskCountForProjectStatisticsComponentProps> = ({
	projectId
}) => {
	const [sectionTypeCounts, setSectionTypeCounts] = useState<SectionTypeTaskCountStatistics[]>([]);

	useEffect(() => {
		loadData();
	}, []);

	const loadData = async () => {
		try {
			const sectionTypeCounts = await taskStatisticsStore.getSectionTypeTaskCountForProjectStatistics(projectId);
			if (sectionTypeCounts) {
				setSectionTypeCounts(sectionTypeCounts);
			}
		} catch (error) {
			console.log(error);
		}
	};

	// Manually create an array containing the desired section types
	const sectionTypes: SectionTypeInfo[] = [
		{ type: SectionType.ToDo, color: "bg-gray-300", text: "TO DO" },
		{ type: SectionType.Doing, color: "bg-blue-300", text: "DOING" },
		{ type: SectionType.Done, color: "bg-green-300", text: "DONE" }
	];

	return (
		<div className="text-sm font-medium">
			{sectionTypeCounts.map((section: SectionTypeTaskCountStatistics, index: number) => (
				<div key={section.sectionType} className="flex flex-row gap-x-5 justify-around items-center">
					<div className="flex justify-center items-center w-16 ">
						<span className="text-white py-[1px] w-full flex items-center justify-center">
							{sectionTypes.map(typeObj => {
								if (typeObj.type === section.sectionType) {
									return (
										<div
											className={`rounded-full py-1 px-2 w-full flex items-center justify-center ${typeObj.color}`}
											key={index}>
											{typeObj.text}
										</div>
									);
								}
								return null;
							})}
						</span>
					</div>
					<div className="flex justify-center items-center ">-</div>
					<div className="flex justify-center items-center ">{section.count}</div>
				</div>
			))}
		</div>
	);
};

export default SectionTypeTaskCountForProjectStatisticsComponent;
