import { Link } from "react-router-dom";
import React from 'react';
import './sideMenu.scss';

interface SideMenuProps {
  isOpen: boolean;
}


const SideMenu: React.FC<SideMenuProps> = ({ isOpen }) => {
  return (
    <aside className={`sidebar bg-base-200 text-base-content ${isOpen ? 'open' : 'closed'}`}>
      <div className="menu p-4 overflow-y-auto">
        <ul className="menu-list">
          <li><Link to="/">Dashboard</Link></li>
          <li><Link to="/settings">Settings</Link></li>
          <li><Link to="/profile">Profile</Link></li>
          <li><Link to="/messages">Messages</Link></li>
          <li><Link to="/help">Help</Link></li>
        </ul>
      </div>
    </aside>
  );
}

export default SideMenu;
