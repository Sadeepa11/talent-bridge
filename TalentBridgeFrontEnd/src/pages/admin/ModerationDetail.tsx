import React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ModerationSplitView } from '../../components/domain/ModerationSplitView';
import type { FullProfile, PreviewProfile } from '../../types';

export const ModerationDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const mockFull: FullProfile = {
    id: id || '1',
    referenceCode: 'CAND-001',
    category: 'Engineering',
    position: 'Senior Frontend',
    city: 'Colombo',
    skills: ['React', 'TS'],
    salaryRange: '200k-300k',
    availability: 'Immediate',
    status: 'submitted',
    experiences: [{ id: '1', title: 'Senior Dev', company: 'Google', startDate: '2020', current: true, description: 'React dev' }],
    qualifications: [],
    pii: { name: 'John Doe', email: 'john@test.com', phone: '0771234567', address: '123 Main St', nic: '951234567V', dob: '1995-01-01' },
    documents: []
  };

  const mockPreview: PreviewProfile = {
    ...mockFull,
  };

  return (
    <div className="flex-col gap-4">
      <div className="flex items-center gap-4 mb-4">
        <span className="material-symbols-outlined cursor-pointer" onClick={() => navigate(-1)}>arrow_back</span>
        <h1 className="headline-md">Review Candidate {mockFull.referenceCode}</h1>
      </div>
      <ModerationSplitView 
        fullData={mockFull} 
        previewData={mockPreview} 
        onApprove={() => navigate('/admin/moderation')} 
        onReject={() => navigate('/admin/moderation')} 
      />
    </div>
  );
};
