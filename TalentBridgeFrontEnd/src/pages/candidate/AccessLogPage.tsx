import React from 'react';
import { Card } from '../../components/ui/Card';
import { DataTable } from '../../components/ui/DataTable';
import { Badge } from '../../components/ui/Badge';

export const AccessLogPage = () => {
  const mockLogs = [
    { id: '1', company: 'TechNova Solutions', type: 'preview', date: '2026-07-28 14:30' },
    { id: '2', company: 'Global Corp', type: 'full', date: '2026-07-27 10:15' },
    { id: '3', company: 'Unknown (Masked)', type: 'preview', date: '2026-07-25 09:00' }
  ];

  return (
    <div className="flex-col gap-4">
      <h1 className="headline-md mb-4">Who Viewed My Profile</h1>
      <Card>
        <DataTable
          data={mockLogs}
          rowKey={r => r.id}
          columns={[
            { header: 'Company', accessor: r => r.company },
            { header: 'Access Type', accessor: r => <Badge status={r.type as any}>{r.type}</Badge> },
            { header: 'Date', accessor: r => r.date }
          ]}
        />
      </Card>
    </div>
  );
};
