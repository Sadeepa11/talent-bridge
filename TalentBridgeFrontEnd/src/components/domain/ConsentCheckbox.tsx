import React from 'react';

interface ConsentCheckboxProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label: string;
}

export const ConsentCheckbox: React.FC<ConsentCheckboxProps> = ({ label, ...props }) => {
  return (
    <label className="flex items-start gap-2 cursor-pointer p-4 rounded-md" style={{ backgroundColor: 'var(--surface-container-lowest)', border: '1px solid var(--outline-variant)' }}>
      <input type="checkbox" style={{ marginTop: '4px', width: '16px', height: '16px' }} {...props} />
      <span className="body-sm">{label}</span>
    </label>
  );
};
