import React, { useState } from 'react';
import { Select } from '../ui/Select';
import { Button } from '../ui/Button';

export const OutcomeSelector: React.FC<{ onSubmit: (outcome: string, notes: string) => void }> = ({ onSubmit }) => {
  const [outcome, setOutcome] = useState('interviewing');
  const [notes, setNotes] = useState('');

  return (
    <div className="flex-col gap-4">
      <Select 
        options={[
          { value: 'interviewing', label: 'Interviewing' },
          { value: 'hired', label: 'Hired' },
          { value: 'rejected', label: 'Rejected' }
        ]}
        value={outcome}
        onChange={e => setOutcome(e.target.value)}
      />
      <textarea 
        className="input-field" 
        style={{ height: '80px', paddingTop: '8px' }} 
        value={notes}
        onChange={e => setNotes(e.target.value)}
        placeholder="Add outcome notes..."
      />
      <Button onClick={() => onSubmit(outcome, notes)}>Submit Outcome</Button>
    </div>
  );
};
