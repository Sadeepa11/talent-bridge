import React from 'react';
import type { FullProfile, PreviewProfile } from '../../types';
import { Card } from '../ui/Card';
import { CandidateProfileView } from './CandidateProfileView';

interface ModerationSplitViewProps {
  fullData: FullProfile;
  previewData: PreviewProfile;
  onApprove: (notes: string) => void;
  onReject: (notes: string) => void;
}

export const ModerationSplitView: React.FC<ModerationSplitViewProps> = ({ fullData, previewData, onApprove, onReject }) => {
  const [notes, setNotes] = React.useState('');

  return (
    <div className="flex-col gap-6">
      <div className="flex gap-6" style={{ flexDirection: 'row', minHeight: '600px' }}>
        <div style={{ flex: 1, overflowY: 'auto', paddingRight: '16px', borderRight: '2px solid var(--surface-container-high)' }}>
          <h3 className="headline-sm mb-4">Full Submitted Data</h3>
          <CandidateProfileView profile={fullData} />
        </div>
        <div style={{ flex: 1, overflowY: 'auto', paddingLeft: '16px' }}>
          <h3 className="headline-sm mb-4">Company Preview (Masked)</h3>
          <Card statusColor="amber">
            <div className="flex-col gap-4">
              <div className="flex justify-between items-center">
                <h4 className="headline-sm">Candidate {previewData.referenceCode}</h4>
                <span className="material-symbols-outlined" style={{ color: 'var(--access-limited)' }}>lock</span>
              </div>
              <div className="text-sm text-gray-500 mb-4">{previewData.position} • {previewData.city}</div>
              
              <div className="flex-col gap-4">
                <div className="font-medium">Experience</div>
                {previewData.experiences.map((exp, i) => (
                  <div key={i} className="text-sm">
                    {exp.title} at [Masked Company]
                  </div>
                ))}
              </div>
            </div>
          </Card>
        </div>
      </div>
      
      <Card className="flex-col gap-4">
        <label className="label-md">Moderation Notes</label>
        <textarea 
          className="input-field" 
          style={{ height: '80px', paddingTop: '8px' }} 
          value={notes} 
          onChange={e => setNotes(e.target.value)}
          placeholder="Add notes for internal review..."
        />
        <div className="flex justify-end gap-4 mt-2">
          <button className="btn btn-danger" onClick={() => onReject(notes)}>Reject</button>
          <button className="btn btn-primary" onClick={() => onApprove(notes)}>Approve & Publish</button>
        </div>
      </Card>
    </div>
  );
};
