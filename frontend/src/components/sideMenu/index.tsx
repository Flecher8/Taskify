import { Link } from "react-router-dom";
import React, { MouseEventHandler } from 'react';
import './sideMenu.scss';
import { SIDEBAR_WIDTH } from "../../constants"; 

interface SideMenuProps {
  isOpen: boolean;
}


const SideMenu: React.FC<SideMenuProps> = ({ isOpen}) => {
  return (
    <aside className={`sidebar bg-base-200 text-base-content ${isOpen ? 'open' : 'closed'}`} style={{minWidth: SIDEBAR_WIDTH}}>
      {/* Menu section */}
      <div className="menu p-4 h-full">
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
