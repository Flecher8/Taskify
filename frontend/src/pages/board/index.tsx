import { Project } from "api/services/projectsService";
import { FC } from "react";

interface BoardProps {
	project: Project;
}

const Board: FC<BoardProps> = ({ project }) => {
	return (
		<div className="board">
			<div>Board</div>
		</div>
	);
};

export default Board;
