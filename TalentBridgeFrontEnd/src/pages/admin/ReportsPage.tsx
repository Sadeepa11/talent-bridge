import React, { useState } from 'react';
import { Card } from '../../components/ui/Card';
import { StatCard } from '../../components/ui/StatCard';

export const ReportsPage = () => {
  const [activeTab, setActiveTab] = useState('conversion');

  const tabs = ['conversion', 'placements', 'revenue', 'inventory'];

  return (
    <div className="flex-col gap-4">
      <h1 className="headline-md mb-4">Reports</h1>
      <div className="flex gap-4 mb-4 border-b border-outline-variant">
        {tabs.map(t => (
          <button 
            key={t}
            onClick={() => setActiveTab(t)}
            style={{
              padding: '8px 16px',
              border: 'none',
              backgroundColor: 'transparent',
              borderBottom: activeTab === t ? '2px solid var(--primary)' : '2px solid transparent',
              color: activeTab === t ? 'var(--primary)' : 'var(--on-surface-variant)',
              fontWeight: activeTab === t ? 600 : 400,
              cursor: 'pointer',
              textTransform: 'capitalize'
            }}
          >
            {t}
          </button>
        ))}
      </div>
      
      {activeTab === 'conversion' && (
        <div className="flex-col gap-4">
          <div className="flex gap-4">
            <StatCard label="Previews Granted" value="250" statusColor="amber" />
            <StatCard label="Full Access Requested" value="120" statusColor="primary" />
            <StatCard label="Conversion Rate" value="48%" statusColor="green" />
          </div>
          <Card>
            <h3 className="headline-sm mb-4">Conversion Funnel</h3>
            <div style={{ height: '200px', backgroundColor: 'var(--surface-container)', display: 'flex', alignItems: 'center', justifyContent: 'center', color: 'var(--on-surface-variant)' }}>
              Chart Placeholder
            </div>
          </Card>
        </div>
      )}
      
      {activeTab !== 'conversion' && (
        <Card>
          <h3 className="headline-sm mb-4" style={{ textTransform: 'capitalize' }}>{activeTab} Report</h3>
          <p className="text-gray-500">Select another tab to see different data sets.</p>
        </Card>
      )}
    </div>
  );
};
