import React from 'react';
import { StatCard } from '../../components/ui/StatCard';
import { Card } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';

export const AdminDashboard = () => {
  return (
    <div className="flex-col gap-4">
      <h1 className="headline-md mb-4">Admin Dashboard</h1>
      
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '24px' }}>
        <StatCard label="Published Candidates" value="1,245" statusColor="secondary" />
        <StatCard label="Available" value="890" statusColor="green" />
        <StatCard label="Reserved" value="355" statusColor="grey" />
        <StatCard label="Active Grants" value="120" statusColor="blue" />
        <StatCard label="Expiring <7D" value="15" statusColor="red" />
        <StatCard label="Awaiting LKR" value="5" statusColor="amber" />
      </div>

      <div className="flex gap-4 mt-4" style={{ flexDirection: 'row' }}>
        <div style={{ flex: 1 }}>
          <Card>
            <h3 className="headline-sm mb-4">Talent Pool by Category</h3>
            {/* Mock Chart */}
            <div className="flex-col gap-4">
              <div>
                <div className="flex justify-between mb-2">
                  <span className="body-md font-medium">Engineering</span>
                  <span className="text-sm text-gray-500">450 / 600</span>
                </div>
                <div style={{ width: '100%', height: '8px', backgroundColor: 'var(--surface-container-high)', borderRadius: '4px', overflow: 'hidden', display: 'flex' }}>
                  <div style={{ width: '75%', backgroundColor: 'var(--access-full)' }}></div>
                  <div style={{ width: '25%', backgroundColor: 'var(--access-reserved)' }}></div>
                </div>
              </div>
              <div>
                <div className="flex justify-between mb-2">
                  <span className="body-md font-medium">Sales</span>
                  <span className="text-sm text-gray-500">200 / 300</span>
                </div>
                <div style={{ width: '100%', height: '8px', backgroundColor: 'var(--surface-container-high)', borderRadius: '4px', overflow: 'hidden', display: 'flex' }}>
                  <div style={{ width: '66%', backgroundColor: 'var(--access-full)' }}></div>
                  <div style={{ width: '34%', backgroundColor: 'var(--access-reserved)' }}></div>
                </div>
              </div>
            </div>
          </Card>
        </div>
        
        <div style={{ flex: 2 }}>
          <Card>
            <h3 className="headline-sm mb-4">Grants Expiring Soon</h3>
            <div className="table-container">
              <table>
                <thead>
                  <tr>
                    <th>Company</th>
                    <th>Scope</th>
                    <th>Time Remaining</th>
                    <th>Status</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td>TechNova Solutions</td>
                    <td><Badge status="full">Full</Badge></td>
                    <td>2 days</td>
                    <td><Badge status="expiring">Expiring</Badge></td>
                  </tr>
                  <tr>
                    <td>Global Corp</td>
                    <td><Badge status="preview">Preview</Badge></td>
                    <td>5 days</td>
                    <td><Badge status="expiring">Expiring</Badge></td>
                  </tr>
                </tbody>
              </table>
            </div>
          </Card>
        </div>
      </div>
    </div>
  );
};
