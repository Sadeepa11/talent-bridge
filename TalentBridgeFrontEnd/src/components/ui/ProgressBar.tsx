import React from 'react';

interface ProgressBarProps {
  progress: number;
  color?: string;
  height?: string;
}

export const ProgressBar: React.FC<ProgressBarProps> = ({ progress, color = 'var(--primary)', height = '8px' }) => {
  return (
    <div style={{ width: '100%', height, backgroundColor: 'var(--surface-container-high)', borderRadius: '4px', overflow: 'hidden' }}>
      <div style={{ width: `${Math.min(100, Math.max(0, progress))}%`, height: '100%', backgroundColor: color, transition: 'width 0.3s ease' }}></div>
    </div>
  );
};
