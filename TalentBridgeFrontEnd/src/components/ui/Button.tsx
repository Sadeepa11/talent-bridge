import React from 'react';

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'danger' | 'ghost';
  icon?: string;
  loading?: boolean;
}

export const Button: React.FC<ButtonProps> = ({ 
  children, 
  variant = 'primary', 
  icon, 
  loading, 
  className = '', 
  ...props 
}) => {
  return (
    <button 
      className={`btn btn-${variant} ${className}`} 
      disabled={loading || props.disabled}
      {...props}
    >
      {icon && !loading && <span className="material-symbols-outlined">{icon}</span>}
      {loading && <span className="material-symbols-outlined" style={{ animation: 'spin 1s linear infinite' }}>autorenew</span>}
      {children}
    </button>
  );
};
