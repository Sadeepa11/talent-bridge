import React from 'react';

interface CardProps extends React.HTMLAttributes<HTMLDivElement> {
  statusColor?: 'amber' | 'green' | 'blue' | 'red' | 'grey' | 'primary' | 'secondary';
  children: React.ReactNode;
}

export const Card: React.FC<CardProps> = ({ children, statusColor, className = '', ...props }) => {
  const borderClass = statusColor ? `card-left-border border-${statusColor}` : '';
  
  return (
    <div className={`card ${borderClass} ${className}`} {...props}>
      {children}
    </div>
  );
};
