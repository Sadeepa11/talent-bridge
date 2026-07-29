import React from 'react';
import { Card } from '../../components/ui/Card';
import { ProgressBar } from '../../components/ui/ProgressBar';
import { Badge } from '../../components/ui/Badge';
import { Button } from '../../components/ui/Button';
import { useNavigate } from 'react-router-dom';

export const CandidateDashboard = () => {
  const navigate = useNavigate();

  return (
    <div className="flex-col gap-6">
      <h1 className="headline-md mb-2">My Dashboard</h1>
      
      <div className="flex gap-6" style={{ flexDirection: 'row' }}>
        <div style={{ flex: 1 }}>
          <Card statusColor="green" className="flex-col gap-4 h-full">
            <h3 className="headline-sm">Profile Status</h3>
            <Badge status="published">Published & Available</Badge>
            <div>
              <div className="flex justify-between mb-1">
                <span className="body-sm font-medium">Profile Completeness</span>
                <span className="body-sm">90%</span>
              </div>
              <ProgressBar progress={90} color="var(--access-full)" />
            </div>
            <Button onClick={() => navigate('/candidate/profile')} className="mt-4">Update Profile</Button>
          </Card>
        </div>
        
        <div style={{ flex: 2 }}>
          <Card className="h-full">
            <h3 className="headline-sm mb-4">Recent Activity</h3>
            <div className="flex-col gap-4">
              <div className="flex gap-4 items-start">
                <span className="material-symbols-outlined text-primary" style={{ padding: '8px', backgroundColor: 'var(--primary-container)', borderRadius: '50%' }}>visibility</span>
                <div>
                  <div className="font-medium">Profile Viewed</div>
                  <div className="text-sm text-gray-500">A company viewed your masked profile today.</div>
                </div>
              </div>
              <div className="flex gap-4 items-start">
                <span className="material-symbols-outlined text-primary" style={{ padding: '8px', backgroundColor: 'var(--primary-container)', borderRadius: '50%' }}>check_circle</span>
                <div>
                  <div className="font-medium">Profile Approved</div>
                  <div className="text-sm text-gray-500">Your profile was approved and published 3 days ago.</div>
                </div>
              </div>
            </div>
          </Card>
        </div>
      </div>
    </div>
  );
};
