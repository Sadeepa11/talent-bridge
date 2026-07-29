import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Card } from '../../components/ui/Card';
import { DataTable } from '../../components/ui/DataTable';
import { Badge } from '../../components/ui/Badge';
import { Button } from '../../components/ui/Button';

export const OrdersPage = () => {
  const navigate = useNavigate();
  const mockOrders = [
    { id: 'ORD-1001', company: 'TechNova', status: 'paid', total: '45,000', date: '2026-07-15' },
    { id: 'ORD-1002', company: 'Global Corp', status: 'pending', total: '15,000', date: '2026-07-28' },
  ];

  return (
    <div className="flex-col gap-4">
      <div className="flex justify-between items-center mb-4">
        <h1 className="headline-md">Orders & Billing</h1>
        <Button>Create Order</Button>
      </div>
      <Card>
        <DataTable
          data={mockOrders}
          rowKey={r => r.id}
          onRowClick={r => navigate(`/admin/orders/${r.id}`)}
          columns={[
            { header: 'Order Code', accessor: r => r.id },
            { header: 'Company', accessor: r => r.company },
            { header: 'Date', accessor: r => r.date },
            { header: 'Total (LKR)', accessor: r => r.total },
            { header: 'Status', accessor: r => <Badge status={r.status === 'paid' ? 'full' : 'preview'}>{r.status}</Badge> },
          ]}
        />
      </Card>
    </div>
  );
};
