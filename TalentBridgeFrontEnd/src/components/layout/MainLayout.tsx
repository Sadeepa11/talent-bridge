import React from 'react';
import { Outlet } from 'react-router-dom';
import { Sidebar } from './Sidebar';
import { Header } from './Header';

export const MainLayout = () => {
  return (
    <div style={{ minHeight: '100vh', backgroundColor: 'var(--background)' }}>
      <Sidebar />
      <Header />
      <main className="main-content">
        <div style={{ maxWidth: 'var(--container-max)', margin: '0 auto' }}>
          <Outlet />
        </div>
      </main>
    </div>
  );
};
