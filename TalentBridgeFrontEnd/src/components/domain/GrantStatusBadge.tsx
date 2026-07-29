import React from 'react';
import { Badge } from '../ui/Badge';
import { ProgressBar } from '../ui/ProgressBar';

interface GrantStatusBadgeProps {
  scope: 'preview' | 'full';
  daysRemaining: number;
}

export const GrantStatusBadge: React.FC<GrantStatusBadgeProps> = ({ scope, daysRemaining }) => {
  const isExpiring = daysRemaining <= 7;
  
  return (
    <div className="flex-col gap-1 w-full" style={{ maxWidth: '120px' }}>
      <Badge status={scope === 'full' ? 'full' : 'preview'}>
        {scope}
      </Badge>
      {daysRemaining > 0 ? (
        <>
          <ProgressBar progress={(daysRemaining / 30) * 100} color={isExpiring ? 'var(--error)' : 'var(--primary)'} height="4px" />
          <div className="text-xs text-center mt-1" style={{ color: isExpiring ? 'var(--error)' : 'var(--on-surface-variant)' }}>
            {daysRemaining} days left
          </div>
        </>
      ) : (
        <Badge status="expired">Expired</Badge>
      )}
    </div>
  );
};
