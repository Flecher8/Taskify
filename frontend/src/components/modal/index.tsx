import React, { ReactNode } from "react";

interface ModalProps {
	id: string;
	openButtonText: ReactNode;
	openButtonStyle: string;
	modalBoxStyle?: string;
	children: ReactNode;
}

const Modal: React.FC<ModalProps> = ({ id, openButtonText, openButtonStyle, modalBoxStyle = "", children }) => {
	const openModal = () => {
		const modal = document.getElementById(id) as HTMLDialogElement;
		modal.showModal();
	};

	const closeModal = () => {
		const modal = document.getElementById(id) as HTMLDialogElement;
		modal.close();
	};

	return (
		<>
			<button className={openButtonStyle} onClick={openModal}>
				{openButtonText}
			</button>
			<dialog id={id} className="modal" onClick={e => e.stopPropagation()}>
				<div className={`modal-box ${modalBoxStyle}`}>
					<form method="dialog">
						<button className="btn btn-sm btn-circle btn-ghost absolute right-2 top-2" onClick={closeModal}>
							✕
						</button>
					</form>
					{React.cloneElement(children as React.ReactElement<any>)}
				</div>
			</dialog>
		</>
	);
};

export default Modal;
