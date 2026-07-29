import React from 'react';
import { Card } from '../../components/ui/Card';
import { DataTable } from '../../components/ui/DataTable';
import { Button } from '../../components/ui/Button';
import { Select } from '../../components/ui/Select';
import { Badge } from '../../components/ui/Badge';

export const BatchCurationPage = () => {
  return (
    <div className="flex gap-6" style={{ flexDirection: 'row' }}>
      <div style={{ width: '260px' }} className="flex-col gap-4">
        <Card statusColor="grey">
          <h3 className="headline-sm mb-2">Pool Status</h3>
          <div className="body-md">Available: 890</div>
        </Card>
        
        <Card>
          <h4 className="font-medium mb-4">Filters</h4>
          <div className="flex-col gap-4">
            <Select options={[{value: 'eng', label: 'Engineering'}, {value: 'sales', label: 'Sales'}]} />
            <div>
              <div className="font-medium mb-2 text-sm">Experience</div>
              <label className="flex items-center gap-2"><input type="checkbox" /> 0-2 Years</label>
              <label className="flex items-center gap-2"><input type="checkbox" /> 3-5 Years</label>
            </div>
          </div>
        </Card>
      </div>

      <div style={{ flex: 1 }} className="flex-col gap-4">
        <Card className="flex justify-between items-center bg-surface-container-low">
          <div>
            <div className="text-sm text-gray-500">Target Company</div>
            <div className="font-medium">TechNova Solutions</div>
          </div>
          <div>
            <div className="text-sm text-gray-500">Selection</div>
            <Badge status="full">5 Candidates</Badge>
          </div>
          <Button icon="send">Issue Batch</Button>
        </Card>

        <Card>
          <DataTable 
            data={[
              { id: '1', ref: 'CAND-001', skills: 'React, Node', exp: '4 Years', status: 'available' },
              { id: '2', ref: 'CAND-002', skills: 'Vue, PHP', exp: '2 Years', status: 'available' }
            ]}
            rowKey={r => r.id}
            checkboxSelection
            columns={[
              { header: 'Ref Code', accessor: r => r.ref },
              { header: 'Skills', accessor: r => r.skills },
              { header: 'Experience', accessor: r => r.exp },
              { header: 'Status', accessor: r => <Badge status={r.status === 'available' ? 'full' : 'reserved'}>{r.status}</Badge> }
            ]}
          />
        </Card>
      </div>
    </div>
  );
};
