import React from 'react';
import { Card } from '../ui/Card';
import { Badge } from '../ui/Badge';
import type { CandidateProfile, FullProfile } from '../../types';

interface CandidateCardProps {
  profile: CandidateProfile;
  accessType: 'limited' | 'full';
  onActionClick?: () => void;
  actionLabel?: string;
}

export const CandidateCard: React.FC<CandidateCardProps> = ({ profile, accessType, onActionClick, actionLabel }) => {
  const isFull = accessType === 'full';
  const borderCol = isFull ? 'green' : 'amber';
  
  return (
    <Card statusColor={borderCol} className="flex-col gap-4">
      <div className="flex justify-between items-start">
        <div className="flex items-center gap-4">
          {isFull && (profile as FullProfile).pii?.photoBase64 ? (
            <img src={(profile as FullProfile).pii.photoBase64} alt="Profile" style={{ width: 48, height: 48, borderRadius: '50%' }} />
          ) : (
            <div style={{ width: 48, height: 48, borderRadius: '50%', backgroundColor: 'var(--surface-container-high)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
              <span className="material-symbols-outlined" style={{ color: 'var(--outline)' }}>person</span>
            </div>
          )}
          <div>
            <h3 className="headline-sm">{isFull ? (profile as FullProfile).pii?.name : `Candidate ${profile.referenceCode}`}</h3>
            <div className="body-sm text-gray-500">{profile.position}</div>
          </div>
        </div>
        <span className="material-symbols-outlined" style={{ color: isFull ? 'var(--access-full)' : 'var(--access-limited)' }}>
          {isFull ? 'lock_open' : 'lock'}
        </span>
      </div>
      
      <div className="flex-col gap-2">
        <div className="flex gap-2 items-center text-sm">
          <span className="material-symbols-outlined" style={{ fontSize: 16 }}>work</span>
          {profile.experiences?.length || 0} roles
        </div>
        <div className="flex gap-2 items-center text-sm">
          <span className="material-symbols-outlined" style={{ fontSize: 16 }}>location_on</span>
          {profile.city}
        </div>
      </div>
      
      <div className="flex gap-2 flex-wrap mt-2">
        {profile.skills?.slice(0, 3).map(skill => (
          <Badge key={skill} status="published">{skill}</Badge>
        ))}
        {(profile.skills?.length || 0) > 3 && <span className="text-xs text-gray-500">+{profile.skills.length - 3} more</span>}
      </div>
      
      {actionLabel && (
        <div className="mt-4 pt-4" style={{ borderTop: '1px solid var(--outline-variant)' }}>
          <button className="btn btn-secondary w-full" onClick={onActionClick}>{actionLabel}</button>
        </div>
      )}
    </Card>
  );
};
