import React from 'react';
import { NavLink } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';

export const Sidebar = () => {
  const { user } = useAuth();
  
  const getNavItems = () => {
    switch (user?.role) {
      case 'admin':
        return [
          { label: 'Dashboard', path: '/admin/dashboard', icon: 'dashboard' },
          { label: 'Candidates', path: '/admin/moderation', icon: 'group' },
          { label: 'Batches', path: '/admin/batches', icon: 'folder_shared' },
          { label: 'Access Grants', path: '/admin/grants', icon: 'vpn_key' },
          { label: 'Companies', path: '/admin/companies', icon: 'business' },
          { label: 'Orders', path: '/admin/orders', icon: 'receipt_long' },
          { label: 'Reports', path: '/admin/reports', icon: 'analytics' },
          { label: 'Settings', path: '/admin/settings', icon: 'settings' },
        ];
      case 'company_user':
        return [
          { label: 'Dashboard', path: '/company/dashboard', icon: 'dashboard' },
          { label: 'Candidates', path: '/company/candidates', icon: 'group' },
          { label: 'Access Requests', path: '/company/access-requests', icon: 'vpn_key' },
          { label: 'Outcomes', path: '/company/outcomes', icon: 'check_circle' },
        ];
      case 'candidate':
        return [
          { label: 'Dashboard', path: '/candidate/dashboard', icon: 'dashboard' },
          { label: 'My Profile', path: '/candidate/profile', icon: 'person' },
          { label: 'Who Viewed Me', path: '/candidate/access-log', icon: 'visibility' },
          { label: 'Consent', path: '/candidate/consent', icon: 'verified_user' },
          { label: 'Data Rights', path: '/candidate/data-rights', icon: 'gavel' },
        ];
      default:
        return [];
    }
  };

  const navItems = getNavItems();

  return (
    <aside className="sidebar flex-col">
      <div style={{ padding: '24px', fontSize: '1.5rem', fontWeight: 700, borderBottom: '1px solid rgba(255,255,255,0.1)' }}>
        TalentBridge
      </div>
      <nav style={{ padding: '16px 0', display: 'flex', flexDirection: 'column', gap: '8px' }}>
        {navItems.map(item => (
          <NavLink
            key={item.path}
            to={item.path}
            style={({ isActive }) => ({
              display: 'flex',
              alignItems: 'center',
              padding: '12px 24px',
              gap: '16px',
              color: 'var(--on-primary)',
              textDecoration: 'none',
              backgroundColor: isActive ? 'rgba(255,255,255,0.1)' : 'transparent',
              borderLeft: isActive ? '4px solid var(--secondary-container)' : '4px solid transparent',
            })}
          >
            <span className="material-symbols-outlined">{item.icon}</span>
            <span className="body-md font-medium">{item.label}</span>
          </NavLink>
        ))}
      </nav>
    </aside>
  );
};
