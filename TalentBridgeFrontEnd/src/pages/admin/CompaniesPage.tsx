import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card } from '../../components/ui/Card';
import { DataTable } from '../../components/ui/DataTable';
import { Badge } from '../../components/ui/Badge';
import { Button } from '../../components/ui/Button';
import { Modal } from '../../components/ui/Modal';
import type { Company } from '../../types';

export const CompaniesPage = () => {
  const navigate = useNavigate();
  const [modalOpen, setModalOpen] = useState(false);

  const mockCompanies: Company[] = [
    { id: '1', name: 'TechNova Solutions', industry: 'IT', status: 'active', contactEmail: 'hr@technova.com', onboardingDate: '2026-01-10' },
    { id: '2', name: 'Global Corp', industry: 'Finance', status: 'pending', contactEmail: 'recruitment@global.com', onboardingDate: '2026-07-28' }
  ];

  return (
    <div className="flex-col gap-4">
      <div className="flex justify-between items-center mb-4">
        <h1 className="headline-md">Companies</h1>
        <Button onClick={() => setModalOpen(true)}>Create Company</Button>
      </div>
      <Card>
        <DataTable
          data={mockCompanies}
          rowKey={r => r.id}
          onRowClick={r => navigate(`/admin/companies/${r.id}`)}
          columns={[
            { header: 'Name', accessor: r => r.name },
            { header: 'Industry', accessor: r => r.industry },
            { header: 'Contact', accessor: r => r.contactEmail },
            { header: 'Onboarding Date', accessor: r => r.onboardingDate },
            { header: 'Status', accessor: r => <Badge status={r.status === 'active' ? 'full' : 'preview'}>{r.status}</Badge> }
          ]}
        />
      </Card>

      <Modal isOpen={modalOpen} onClose={() => setModalOpen(false)} title="Create Company" actions={<Button onClick={() => setModalOpen(false)}>Save</Button>}>
        <div className="flex-col gap-4">
          <input className="input-field" placeholder="Company Name" />
          <input className="input-field" placeholder="Industry" />
          <input className="input-field" placeholder="Contact Email" />
        </div>
      </Modal>
    </div>
  );
};
