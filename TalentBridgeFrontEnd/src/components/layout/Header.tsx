import React from 'react';
import { useAuth } from '../../contexts/AuthContext';
import { Button } from '../ui/Button';

export const Header = () => {
  const { user, logout } = useAuth();
  
  return (
    <header className="topbar justify-between">
      <div className="flex items-center gap-4 w-full" style={{ maxWidth: '400px' }}>
        <div style={{ position: 'relative', width: '100%' }}>
          <span className="material-symbols-outlined" style={{ position: 'absolute', left: '12px', top: '8px', color: 'var(--on-surface-variant)' }}>search</span>
          <input type="text" className="input-field" placeholder="Search..." style={{ paddingLeft: '40px' }} />
        </div>
      </div>
      <div className="flex items-center gap-4">
        <div style={{ position: 'relative' }}>
          <span className="material-symbols-outlined">notifications</span>
          <span style={{ position: 'absolute', top: 0, right: 0, width: '8px', height: '8px', backgroundColor: 'var(--error)', borderRadius: '50%' }}></span>
        </div>
        <div className="flex items-center gap-2">
          {user?.avatar ? (
            <img src={user.avatar} alt="Avatar" style={{ width: '32px', height: '32px', borderRadius: '50%' }} />
          ) : (
            <div style={{ width: '32px', height: '32px', borderRadius: '50%', backgroundColor: 'var(--primary-container)', display: 'flex', alignItems: 'center', justifyContent: 'center', color: 'var(--on-primary-container)' }}>
              {user?.name?.charAt(0) || 'U'}
            </div>
          )}
          <div className="flex-col">
            <span className="label-md">{user?.name}</span>
            <span className="text-xs" style={{ color: 'var(--on-surface-variant)' }}>{user?.role}</span>
          </div>
        </div>
        <Button variant="ghost" icon="logout" onClick={logout} />
      </div>
    </header>
  );
};
