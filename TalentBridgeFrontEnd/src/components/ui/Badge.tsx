import React from 'react';

interface BadgeProps {
  status: 'preview' | 'full' | 'active' | 'expiring' | 'reserved' | 'published' | 'submitted' | 'approved' | 'rejected' | 'placed' | 'withdrawn' | 'draft' | 'expired';
  children?: React.ReactNode;
}

export const Badge: React.FC<BadgeProps> = ({ status, children }) => {
  let mappedStatus = status;
  // Map some business statuses to UI colors
  if (['published', 'approved', 'placed'].includes(status)) mappedStatus = 'full';
  if (['submitted', 'draft'].includes(status)) mappedStatus = 'reserved';
  if (['rejected', 'withdrawn', 'expired'].includes(status)) mappedStatus = 'expiring';
  
  return (
    <span className={`badge badge-${mappedStatus}`}>
      {children || status}
    </span>
  );
};
