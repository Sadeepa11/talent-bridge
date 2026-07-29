import React from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Card } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';

export const CompanyDetailPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  return (
    <div className="flex-col gap-4">
      <div className="flex items-center gap-4 mb-4">
        <span className="material-symbols-outlined cursor-pointer" onClick={() => navigate(-1)}>arrow_back</span>
        <h1 className="headline-md">Company Detail</h1>
      </div>
      <Card statusColor="primary">
        <h2 className="headline-sm mb-2">TechNova Solutions</h2>
        <div className="text-sm text-gray-600 mb-4">Industry: IT | Contact: hr@technova.com</div>
        <Badge status="full">Active</Badge>
      </Card>
      
      <div className="flex gap-4 mt-4" style={{ flexDirection: 'row' }}>
        <div style={{ flex: 1 }}>
          <Card>
            <h3 className="headline-sm mb-4">Users</h3>
            <ul style={{ listStyle: 'none', padding: 0 }}>
              <li className="mb-2">Admin User (admin@technova.com)</li>
              <li>HR User (hr@technova.com)</li>
            </ul>
          </Card>
        </div>
        <div style={{ flex: 2 }}>
          <Card>
            <h3 className="headline-sm mb-4">Batches & Grants History</h3>
            <p className="text-sm text-gray-500">History will appear here.</p>
          </Card>
        </div>
      </div>
    </div>
  );
};
