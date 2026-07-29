import React from 'react';
import { Card } from '../../components/ui/Card';
import { DataTable } from '../../components/ui/DataTable';
import { Badge } from '../../components/ui/Badge';
import { Button } from '../../components/ui/Button';

export const GrantsPage = () => {
  const mockGrants = [
    { id: '1', scope: 'full', status: 'active', company: 'TechNova', candidate: 'CAND-001', validFrom: '2026-07-01', validTo: '2026-07-31' },
    { id: '2', scope: 'preview', status: 'expired', company: 'Global Corp', candidate: 'CAND-002', validFrom: '2026-06-01', validTo: '2026-06-30' }
  ];

  return (
    <div className="flex-col gap-4">
      <h1 className="headline-md mb-4">Access Grants</h1>
      <Card>
        <DataTable
          data={mockGrants}
          rowKey={r => r.id}
          columns={[
            { header: 'Company', accessor: r => r.company },
            { header: 'Candidate', accessor: r => r.candidate },
            { header: 'Scope', accessor: r => <Badge status={r.scope as any}>{r.scope}</Badge> },
            { header: 'Status', accessor: r => <Badge status={r.status as any}>{r.status}</Badge> },
            { header: 'Valid From', accessor: r => r.validFrom },
            { header: 'Valid To', accessor: r => r.validTo },
            { header: 'Actions', accessor: () => (
              <div className="flex gap-2">
                <Button variant="secondary">Extend</Button>
                <Button variant="danger">Revoke</Button>
              </div>
            )}
          ]}
        />
      </Card>
    </div>
  );
};
