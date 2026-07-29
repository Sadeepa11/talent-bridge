import React from 'react';
import { useParams } from 'react-router-dom';
import { CandidateProfileView } from '../../components/domain/CandidateProfileView';
import { OutcomeSelector } from '../../components/domain/OutcomeSelector';
import { Card } from '../../components/ui/Card';
import type { FullProfile } from '../../types';

export const CandidateProfilePage = () => {
  const { refCode } = useParams();

  const mockFull: FullProfile = {
    id: '2',
    referenceCode: refCode || 'CAND-002',
    category: 'Design',
    position: 'UI/UX Designer',
    city: 'Kandy',
    skills: ['Figma', 'Sketch'],
    salaryRange: '150k-200k',
    availability: '1 month notice',
    status: 'published',
    experiences: [],
    qualifications: [],
    pii: { name: 'Alice Smith', email: 'alice@test.com', phone: '0779876543', address: '456 Hill Rd', nic: '961234567V', dob: '1996-01-01' },
    documents: [{ id: '1', name: 'Resume.pdf', type: 'resume', url: '#' }]
  };

  return (
    <div className="flex-col gap-6">
      <div style={{ backgroundColor: '#dcfce7', color: '#15803d', padding: '16px', borderRadius: 'var(--radius-md)', display: 'flex', alignItems: 'center', gap: '8px' }}>
        <span className="material-symbols-outlined">check_circle</span>
        <span className="font-medium">You have full access to this candidate's profile.</span>
      </div>

      <CandidateProfileView profile={mockFull} />

      <Card statusColor="primary">
        <h3 className="headline-sm mb-4">Report Outcome</h3>
        <p className="body-md mb-4 text-gray-600">Please update the status of your engagement with this candidate.</p>
        <OutcomeSelector onSubmit={(outcome, notes) => alert(`Submitted: ${outcome}`)} />
      </Card>
    </div>
  );
};
