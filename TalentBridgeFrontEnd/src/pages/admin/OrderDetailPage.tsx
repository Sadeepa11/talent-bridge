import React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Card } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';
import { DataTable } from '../../components/ui/DataTable';
import { Button } from '../../components/ui/Button';

export const OrderDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const mockItems = [
    { id: '1', description: 'Full Access Grant for CAND-001', amount: '15,000' },
    { id: '2', description: 'Full Access Grant for CAND-002', amount: '15,000' },
    { id: '3', description: 'Preview Access Grant for CAND-003', amount: '15,000' }
  ];

  return (
    <div className="flex-col gap-4">
      <div className="flex items-center gap-4 mb-4">
        <span className="material-symbols-outlined cursor-pointer" onClick={() => navigate(-1)}>arrow_back</span>
        <h1 className="headline-md">Order {id || 'ORD-1002'}</h1>
      </div>
      
      <div className="flex gap-4" style={{ flexDirection: 'row' }}>
        <div className="flex-col gap-4" style={{ flex: 2 }}>
          <Card>
            <h3 className="headline-sm mb-4">Line Items</h3>
            <DataTable 
              data={mockItems}
              rowKey={r => r.id}
              columns={[
                { header: 'Description', accessor: r => r.description },
                { header: 'Amount (LKR)', accessor: r => r.amount }
              ]}
            />
            <div className="flex justify-end mt-4 pt-4 font-bold" style={{ borderTop: '1px solid var(--outline-variant)' }}>
              Total: 45,000 LKR
            </div>
          </Card>
        </div>
        
        <div className="flex-col gap-4" style={{ flex: 1 }}>
          <Card statusColor="amber">
            <h3 className="headline-sm mb-2">Order Status</h3>
            <Badge status="preview">Pending Payment</Badge>
            <p className="body-sm text-gray-500 mt-2">Awaiting bank transfer confirmation.</p>
          </Card>
          
          <Card>
            <h3 className="headline-sm mb-4">Record Payment</h3>
            <div className="flex-col gap-4">
              <input type="text" className="input-field" placeholder="Transaction Reference" />
              <input type="date" className="input-field" />
              <Button>Confirm Payment</Button>
            </div>
          </Card>
        </div>
      </div>
    </div>
  );
};
