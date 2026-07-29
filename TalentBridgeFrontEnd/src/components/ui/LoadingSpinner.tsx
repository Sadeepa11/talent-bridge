import React from 'react';

export const LoadingSpinner: React.FC = () => {
  return (
    <div className="flex justify-center items-center p-8">
      <span className="material-symbols-outlined" style={{ animation: 'spin 1s linear infinite', fontSize: '2rem', color: 'var(--primary)' }}>autorenew</span>
    </div>
  );
};
