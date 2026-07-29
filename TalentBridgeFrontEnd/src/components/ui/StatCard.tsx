import React from 'react';
import { Card } from './Card';

interface StatCardProps {
  label: string;
  value: string | number;
  statusColor?: 'amber' | 'green' | 'blue' | 'red' | 'grey' | 'primary' | 'secondary';
}

export const StatCard: React.FC<StatCardProps> = ({ label, value, statusColor }) => {
  return (
    <Card statusColor={statusColor} className="flex-col justify-center">
      <div className="label-md mb-2">{label}</div>
      <div className="headline-lg" style={{ color: 'var(--primary)' }}>{value}</div>
    </Card>
  );
};
