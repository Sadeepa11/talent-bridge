import React from 'react';
import { Card } from '../../components/ui/Card';
import { CandidateCard } from '../../components/domain/CandidateCard';
import type { CandidateProfile } from '../../types';
import { useNavigate } from 'react-router-dom';

export const CandidateGridPage = () => {
  const navigate = useNavigate();
  const mockCandidates: CandidateProfile[] = [
    {
      id: '1', referenceCode: 'CAND-001', category: 'Engineering', position: 'Senior Frontend Engineer',
      city: 'Colombo', skills: ['React', 'TS'], salaryRange: '', availability: '', status: 'published',
      experiences: [{ id: '1', title: 'Senior Dev', company: 'Google', startDate: '2020', current: true, description: '' }],
      qualifications: []
    },
    {
      id: '2', referenceCode: 'CAND-002', category: 'Design', position: 'UI/UX Designer',
      city: 'Kandy', skills: ['Figma', 'Sketch'], salaryRange: '', availability: '', status: 'published',
      experiences: [], qualifications: [],
      pii: { name: 'Alice Smith', email: 'alice@test.com', phone: '', address: '', nic: '', dob: '' },
      documents: []
    } as any
  ];

  return (
    <div className="flex-col gap-6">
      <Card statusColor="secondary" className="flex justify-between items-center bg-surface-container-low">
        <div>
          <h2 className="headline-sm text-primary">Active Batch #B-2026-07</h2>
          <div className="text-sm text-gray-500">12 Candidates available</div>
        </div>
        <div className="text-right">
          <div className="text-sm text-gray-500 mb-1">Time Remaining</div>
          <div className="headline-sm" style={{ color: 'var(--error)' }}>2 days 14 hours</div>
        </div>
      </Card>
      
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '24px' }}>
        <CandidateCard 
          profile={mockCandidates[0]} 
          accessType="limited" 
          actionLabel="Request Full Access" 
          onActionClick={() => alert('Access Requested')} 
        />
        <CandidateCard 
          profile={mockCandidates[1]} 
          accessType="full" 
          actionLabel="View Full Profile" 
          onActionClick={() => navigate(`/company/candidates/${mockCandidates[1].referenceCode}`)} 
        />
      </div>
    </div>
  );
};
