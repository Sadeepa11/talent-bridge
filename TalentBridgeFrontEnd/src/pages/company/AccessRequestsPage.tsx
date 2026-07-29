import React from 'react';
import { Card } from '../../components/ui/Card';
import { DataTable } from '../../components/ui/DataTable';
import { Badge } from '../../components/ui/Badge';

export const AccessRequestsPage = () => {
  const mockRequests = [
    { id: '1', candidate: 'CAND-001', requestedAt: '2026-07-28', status: 'pending' },
    { id: '2', candidate: 'CAND-002', requestedAt: '2026-07-25', status: 'approved' },
  ];

  return (
    <div className="flex-col gap-4">
      <h1 className="headline-md mb-4">Access Requests</h1>
      <Card>
        <DataTable
          data={mockRequests}
          rowKey={r => r.id}
          columns={[
            { header: 'Request ID', accessor: r => r.id },
            { header: 'Candidate Reference', accessor: r => r.candidate },
            { header: 'Date Requested', accessor: r => r.requestedAt },
            { header: 'Status', accessor: r => <Badge status={r.status === 'approved' ? 'full' : 'preview'}>{r.status}</Badge> }
          ]}
        />
      </Card>
    </div>
  );
};
