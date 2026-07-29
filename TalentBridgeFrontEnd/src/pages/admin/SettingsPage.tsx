import React, { useState } from 'react';
import { Card } from '../../components/ui/Card';
import { Button } from '../../components/ui/Button';

export const SettingsPage = () => {
  const [settings, setSettings] = useState({
    unitPrice: '15000',
    defaultWindowLength: '30',
    rateLimits: '100'
  });

  const handleSave = () => {
    alert('Settings saved successfully');
  };

  return (
    <div className="flex-col gap-4" style={{ maxWidth: '600px' }}>
      <h1 className="headline-md mb-4">System Settings</h1>
      <Card className="flex-col gap-6">
        <div>
          <label className="label-md mb-2" style={{ display: 'block' }}>Base Access Grant Unit Price (LKR)</label>
          <input 
            type="number" 
            className="input-field" 
            value={settings.unitPrice}
            onChange={e => setSettings({...settings, unitPrice: e.target.value})}
          />
        </div>
        <div>
          <label className="label-md mb-2" style={{ display: 'block' }}>Default Grant Window (Days)</label>
          <input 
            type="number" 
            className="input-field" 
            value={settings.defaultWindowLength}
            onChange={e => setSettings({...settings, defaultWindowLength: e.target.value})}
          />
        </div>
        <div>
          <label className="label-md mb-2" style={{ display: 'block' }}>API Rate Limits (req/min)</label>
          <input 
            type="number" 
            className="input-field" 
            value={settings.rateLimits}
            onChange={e => setSettings({...settings, rateLimits: e.target.value})}
          />
        </div>
        <div className="flex justify-end pt-4" style={{ borderTop: '1px solid var(--outline-variant)' }}>
          <Button onClick={handleSave}>Save Settings</Button>
        </div>
      </Card>
    </div>
  );
};
