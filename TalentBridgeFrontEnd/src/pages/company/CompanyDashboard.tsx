import React from 'react';
import { Card } from '../../components/ui/Card';
import { StatCard } from '../../components/ui/StatCard';
import { Badge } from '../../components/ui/Badge';

export const CompanyDashboard = () => {
  return (
    <div className="flex-col gap-6">
      <h1 className="headline-md mb-2">Company Dashboard</h1>
      
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '24px' }}>
        <StatCard label="Active Full Grants" value="5" statusColor="green" />
        <StatCard label="Active Previews" value="12" statusColor="amber" />
        <StatCard label="Pending Outcomes" value="3" statusColor="primary" />
      </div>

      <div className="flex gap-6" style={{ flexDirection: 'row' }}>
        <div style={{ flex: 1 }}>
          <Card statusColor="red">
            <h3 className="headline-sm mb-4 flex items-center gap-2">
              <span className="material-symbols-outlined text-error">warning</span>
              Expiring Soon
            </h3>
            <div className="flex-col gap-4">
              {[1, 2].map(i => (
                <div key={i} className="flex justify-between items-center pb-2 border-b border-outline-variant">
                  <div>
                    <div className="font-medium">Candidate CAND-00{i}</div>
                    <div className="text-sm text-gray-500">Full Access</div>
                  </div>
                  <Badge status="expiring">2 Days Left</Badge>
                </div>
              ))}
            </div>
          </Card>
        </div>
        
        <div style={{ flex: 1 }}>
          <Card statusColor="primary">
            <h3 className="headline-sm mb-4">Pending Outcomes</h3>
            <div className="flex-col gap-4">
              {[3, 4].map(i => (
                <div key={i} className="flex justify-between items-center pb-2 border-b border-outline-variant">
                  <div>
                    <div className="font-medium">Candidate CAND-00{i}</div>
                    <div className="text-sm text-gray-500">Interviewed on Jul 20</div>
                  </div>
                  <a href={`/company/candidates/CAND-00${i}`} className="text-primary text-sm font-medium">Report</a>
                </div>
              ))}
            </div>
          </Card>
        </div>
      </div>
    </div>
  );
};
