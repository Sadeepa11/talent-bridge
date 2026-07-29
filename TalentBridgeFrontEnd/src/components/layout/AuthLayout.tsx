import React from 'react';
import { Outlet } from 'react-router-dom';
import { Card } from '../ui/Card';

export const AuthLayout = () => {
  return (
    <div className="flex items-center justify-center" style={{ minHeight: '100vh', backgroundColor: 'var(--surface-container-low)' }}>
      <Card style={{ width: '100%', maxWidth: '440px', padding: '32px' }}>
        <div className="text-center mb-4">
          <h1 className="headline-md mb-2" style={{ color: 'var(--primary)' }}>TalentBridge</h1>
          <p className="body-md" style={{ color: 'var(--on-surface-variant)' }}>Professional hiring platform</p>
        </div>
        <Outlet />
      </Card>
    </div>
  );
};
