import React, { useState } from 'react';
import { Card } from '../../components/ui/Card';
import { Button } from '../../components/ui/Button';

export const ProfileWizardPage = () => {
  const [step, setStep] = useState(1);

  return (
    <div className="flex-col gap-6">
      <h1 className="headline-md">Complete Your Profile</h1>
      <div className="flex justify-between mb-4">
        {[1, 2, 3, 4, 5].map(s => (
          <div key={s} style={{ flex: 1, textAlign: 'center', borderBottom: s <= step ? '4px solid var(--primary)' : '4px solid var(--surface-container-high)', paddingBottom: '8px', color: s <= step ? 'var(--primary)' : 'var(--on-surface-variant)' }}>
            Step {s}
          </div>
        ))}
      </div>

      <Card>
        {step === 1 && <div><h2 className="headline-sm mb-4">Personal Details</h2><p>Form fields...</p></div>}
        {step === 2 && <div><h2 className="headline-sm mb-4">Professional Details</h2><p>Form fields...</p></div>}
        {step === 3 && <div><h2 className="headline-sm mb-4">Qualifications & Experience</h2><p>Form fields...</p></div>}
        {step === 4 && <div><h2 className="headline-sm mb-4">Skills & Documents</h2><p>Form fields...</p></div>}
        {step === 5 && <div><h2 className="headline-sm mb-4">Preview & Consent</h2><p>Masked preview...</p></div>}

        <div className="flex justify-between mt-6 pt-4" style={{ borderTop: '1px solid var(--outline-variant)' }}>
          <Button variant="secondary" onClick={() => setStep(s => Math.max(1, s - 1))} disabled={step === 1}>Back</Button>
          <Button onClick={() => step < 5 ? setStep(s => s + 1) : alert('Submitted!')}>{step === 5 ? 'Submit Profile' : 'Next'}</Button>
        </div>
      </Card>
    </div>
  );
};
