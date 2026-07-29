import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Card } from '../../components/ui/Card';
import { DataTable } from '../../components/ui/DataTable';
import { Badge } from '../../components/ui/Badge';
import type { CandidateProfile } from '../../types';

export const ModerationQueue = () => {
  const navigate = useNavigate();
  const mockData: CandidateProfile[] = [
    { id: '1', referenceCode: 'CAND-001', category: 'Engineering', position: 'Senior Frontend', city: 'Colombo', skills: ['React', 'TS'], salaryRange: '', availability: '', status: 'submitted', experiences: [], qualifications: [] },
    { id: '2', referenceCode: 'CAND-002', category: 'Sales', position: 'Account Executive', city: 'Kandy', skills: ['B2B', 'Salesforce'], salaryRange: '', availability: '', status: 'submitted', experiences: [], qualifications: [] }
  ];

  return (
    <div className="flex-col gap-4">
      <h1 className="headline-md mb-4">Moderation Queue</h1>
      <Card>
        <DataTable
          data={mockData}
          rowKey={(row) => row.id}
          onRowClick={(row) => navigate(`/admin/moderation/${row.id}`)}
          columns={[
            { header: 'Reference', accessor: (row) => row.referenceCode },
            { header: 'Position', accessor: (row) => row.position },
            { header: 'Date', accessor: () => '2026-07-29' },
            { header: 'Status', accessor: (row) => <Badge status={row.status}>{row.status}</Badge> }
          ]}
        />
      </Card>
    </div>
  );
};
