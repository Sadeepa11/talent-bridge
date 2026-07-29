import React, { useState } from 'react';
import { Card } from '../../components/ui/Card';
import { ConsentCheckbox } from '../../components/domain/ConsentCheckbox';
import { Button } from '../../components/ui/Button';

export const ConsentPage = () => {
  const [granted, setGranted] = useState(false);

  return (
    <div className="flex-col gap-6">
      <h1 className="headline-md">Manage Consent</h1>
      <p className="body-md text-gray-600">Review how companies see your profile before and after you grant access.</p>
      
      <div className="flex gap-6" style={{ flexDirection: 'row' }}>
        <div style={{ flex: 1 }}>
          <Card statusColor="amber" className="flex-col gap-4">
            <div className="flex justify-between items-center">
              <h3 className="headline-sm">Limited View</h3>
              <span className="material-symbols-outlined" style={{ color: 'var(--access-limited)' }}>lock</span>
            </div>
            <div className="text-sm text-gray-500">What companies see initially</div>
            <ul style={{ paddingLeft: '20px', fontSize: '14px' }}>
              <li>Masked Name (e.g. Candidate 1024)</li>
              <li>Hidden Contact Info</li>
              <li>Masked Employers</li>
              <li>Skills & Job Title visible</li>
            </ul>
          </Card>
        </div>
        
        <div style={{ flex: 1 }}>
          <Card statusColor="green" className="flex-col gap-4">
            <div className="flex justify-between items-center">
              <h3 className="headline-sm">Full View</h3>
              <span className="material-symbols-outlined" style={{ color: 'var(--access-full)' }}>lock_open</span>
            </div>
            <div className="text-sm text-gray-500">What companies see after consent</div>
            <ul style={{ paddingLeft: '20px', fontSize: '14px' }}>
              <li>Full Name & Photo</li>
              <li>Email & Phone Number</li>
              <li>Full Work History</li>
              <li>Downloadable Resume</li>
            </ul>
          </Card>
        </div>
      </div>

      <Card>
        <ConsentCheckbox 
          label="I consent to sharing my full profile data with verified companies when requested." 
          checked={granted}
          onChange={e => setGranted(e.target.checked)}
        />
        <div className="mt-4 flex gap-4">
          <Button onClick={() => alert('Consent Updated')}>Save Preferences</Button>
          <Button variant="danger" onClick={() => alert('Withdrawal Process Started')}>Withdraw from Platform</Button>
        </div>
      </Card>
    </div>
  );
};
