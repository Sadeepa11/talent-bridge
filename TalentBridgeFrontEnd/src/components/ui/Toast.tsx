import React, { useState, useEffect } from 'react';

interface ToastProps {
  message: string;
  type?: 'success' | 'error' | 'info';
  duration?: number;
  onClose?: () => void;
}

export const Toast: React.FC<ToastProps> = ({ message, type = 'info', duration = 3000, onClose }) => {
  const [visible, setVisible] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => {
      setVisible(false);
      if (onClose) onClose();
    }, duration);
    return () => clearTimeout(timer);
  }, [duration, onClose]);

  if (!visible) return null;

  const bgColors = {
    success: 'var(--access-full)',
    error: 'var(--error)',
    info: 'var(--primary)'
  };

  return (
    <div style={{
      position: 'fixed',
      bottom: '24px',
      right: '24px',
      backgroundColor: bgColors[type],
      color: 'white',
      padding: '12px 24px',
      borderRadius: 'var(--radius-md)',
      boxShadow: 'var(--shadow-card)',
      zIndex: 1000,
      display: 'flex',
      alignItems: 'center',
      gap: '8px'
    }}>
      <span className="material-symbols-outlined">{type === 'success' ? 'check_circle' : type === 'error' ? 'error' : 'info'}</span>
      <span className="body-md">{message}</span>
    </div>
  );
};
