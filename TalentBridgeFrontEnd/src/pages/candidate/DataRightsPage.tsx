import React from 'react';
import { Card } from '../../components/ui/Card';
import { Button } from '../../components/ui/Button';

export const DataRightsPage = () => (
  <div className="flex-col gap-4">
    <h1 className="headline-md mb-4">Data Rights</h1>
    <Card className="flex-col gap-4">
      <h3 className="headline-sm">Export Data</h3>
      <p className="body-sm text-gray-600">Download a copy of all data TalentBridge holds about you.</p>
      <Button variant="secondary" icon="download" style={{ width: 'fit-content' }}>Request Data Export</Button>
    </Card>
    <Card className="flex-col gap-4 border-red">
      <h3 className="headline-sm text-error">Account Deletion</h3>
      <p className="body-sm text-gray-600">Permanently delete your account and all associated data. This action cannot be undone.</p>
      <Button variant="danger" icon="delete_forever" style={{ width: 'fit-content' }}>Request Deletion</Button>
    </Card>
  </div>
);
