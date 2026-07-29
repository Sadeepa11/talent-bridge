import React from 'react';

interface EmptyStateProps {
  icon: string;
  message: string;
  description?: string;
  action?: React.ReactNode;
}

export const EmptyState: React.FC<EmptyStateProps> = ({ icon, message, description, action }) => {
  return (
    <div className="flex-col items-center justify-center text-center p-8 w-full" style={{ color: 'var(--on-surface-variant)' }}>
      <span className="material-symbols-outlined mb-4" style={{ fontSize: '48px', color: 'var(--outline-variant)' }}>{icon}</span>
      <h3 className="headline-sm mb-2" style={{ color: 'var(--on-surface)' }}>{message}</h3>
      {description && <p className="body-md mb-4">{description}</p>}
      {action && <div>{action}</div>}
    </div>
  );
};
