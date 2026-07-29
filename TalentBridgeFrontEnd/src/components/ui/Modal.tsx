import React from 'react';
import { Card } from './Card';
import { Button } from './Button';

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title: string;
  children: React.ReactNode;
  actions?: React.ReactNode;
}

export const Modal: React.FC<ModalProps> = ({ isOpen, onClose, title, children, actions }) => {
  if (!isOpen) return null;
  
  return (
    <div style={{ position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, backgroundColor: 'rgba(0,0,0,0.5)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 100 }}>
      <Card style={{ width: '100%', maxWidth: '600px', maxHeight: '90vh', overflowY: 'auto' }}>
        <div className="flex justify-between items-center mb-4">
          <h2 className="headline-sm">{title}</h2>
          <Button variant="ghost" icon="close" onClick={onClose} />
        </div>
        <div className="mb-4">
          {children}
        </div>
        {actions && (
          <div className="flex justify-end gap-2 mt-4 pt-4" style={{ borderTop: '1px solid var(--outline-variant)' }}>
            {actions}
          </div>
        )}
      </Card>
    </div>
  );
};
