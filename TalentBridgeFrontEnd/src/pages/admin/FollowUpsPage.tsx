import React from 'react';
import { Card } from '../../components/ui/Card';
import { DataTable } from '../../components/ui/DataTable';
import { Badge } from '../../components/ui/Badge';
import { Button } from '../../components/ui/Button';

export const FollowUpsPage = () => {
  const mockTasks = [
    { id: '1', grant: 'G-1029', company: 'TechNova', candidate: 'CAND-001', type: 'Outcome Check', dueDate: '2026-07-30', status: 'pending', assignedTo: 'Admin A' },
    { id: '2', grant: 'G-1030', company: 'Global Corp', candidate: 'CAND-005', type: 'Payment Follow-up', dueDate: '2026-07-28', status: 'overdue', assignedTo: 'Admin B' },
  ];

  return (
    <div className="flex-col gap-4">
      <h1 className="headline-md mb-4">Follow-ups Queue</h1>
      <Card>
        <DataTable
          data={mockTasks}
          rowKey={r => r.id}
          columns={[
            { header: 'Grant', accessor: r => r.grant },
            { header: 'Company', accessor: r => r.company },
            { header: 'Candidate', accessor: r => r.candidate },
            { header: 'Task Type', accessor: r => r.type },
            { header: 'Due Date', accessor: r => r.dueDate },
            { header: 'Assigned To', accessor: r => r.assignedTo },
            { header: 'Status', accessor: r => <Badge status={r.status === 'pending' ? 'preview' : 'expiring'}>{r.status}</Badge> },
            { header: 'Action', accessor: () => <Button variant="ghost" icon="check_circle" /> }
          ]}
        />
      </Card>
    </div>
  );
};
