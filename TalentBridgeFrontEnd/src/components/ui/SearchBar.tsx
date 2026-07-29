import React from 'react';

interface SearchBarProps extends React.InputHTMLAttributes<HTMLInputElement> {}

export const SearchBar: React.FC<SearchBarProps> = (props) => {
  return (
    <div style={{ position: 'relative', width: '100%' }}>
      <span className="material-symbols-outlined" style={{ position: 'absolute', left: '12px', top: '8px', color: 'var(--on-surface-variant)' }}>search</span>
      <input type="text" className="input-field" placeholder="Search..." style={{ paddingLeft: '40px' }} {...props} />
    </div>
  );
};
