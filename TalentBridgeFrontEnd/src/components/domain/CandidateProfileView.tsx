import React from 'react';
import type { FullProfile } from '../../types';
import { Card } from '../ui/Card';
import { Badge } from '../ui/Badge';

export const CandidateProfileView: React.FC<{ profile: FullProfile }> = ({ profile }) => {
  return (
    <div className="flex-col gap-6 w-full">
      <Card statusColor="green">
        <div className="flex gap-6 items-start">
          <div style={{ width: 100, height: 100, borderRadius: '50%', backgroundColor: 'var(--surface-container)', overflow: 'hidden' }}>
            {profile.pii.photoBase64 ? <img src={profile.pii.photoBase64} alt={profile.pii.name} style={{ width: '100%', height: '100%', objectFit: 'cover' }} /> : <span className="material-symbols-outlined p-4" style={{ fontSize: 60 }}>person</span>}
          </div>
          <div className="flex-col gap-2">
            <h2 className="headline-md">{profile.pii.name}</h2>
            <div className="flex gap-4 text-sm text-gray-600">
              <span className="flex items-center gap-1"><span className="material-symbols-outlined" style={{ fontSize: 16 }}>email</span> {profile.pii.email}</span>
              <span className="flex items-center gap-1"><span className="material-symbols-outlined" style={{ fontSize: 16 }}>phone</span> {profile.pii.phone}</span>
              <span className="flex items-center gap-1"><span className="material-symbols-outlined" style={{ fontSize: 16 }}>location_on</span> {profile.city}</span>
            </div>
            <div className="flex gap-2 flex-wrap mt-2">
              {profile.skills.map(s => <Badge key={s} status="published">{s}</Badge>)}
            </div>
          </div>
        </div>
      </Card>
      
      <div className="flex gap-6" style={{ flexDirection: 'row' }}>
        <div className="flex-col gap-6" style={{ flex: 2 }}>
          <Card>
            <h3 className="headline-sm mb-4">Experience</h3>
            <div className="flex-col gap-4">
              {profile.experiences.map((exp, i) => (
                <div key={i} className="flex-col gap-1 pb-4" style={{ borderBottom: i < profile.experiences.length - 1 ? '1px solid var(--outline-variant)' : 'none' }}>
                  <div className="font-medium">{exp.title}</div>
                  <div className="text-sm text-gray-600">{exp.company} • {exp.startDate} - {exp.current ? 'Present' : exp.endDate}</div>
                  <p className="body-sm mt-2">{exp.description}</p>
                </div>
              ))}
            </div>
          </Card>
        </div>
        <div className="flex-col gap-6" style={{ flex: 1 }}>
          <Card>
            <h3 className="headline-sm mb-4">Qualifications</h3>
            <div className="flex-col gap-4">
              {profile.qualifications.map((q, i) => (
                <div key={i} className="flex-col gap-1">
                  <div className="font-medium">{q.degree}</div>
                  <div className="text-sm text-gray-600">{q.institution} • {q.year}</div>
                </div>
              ))}
            </div>
          </Card>
        </div>
      </div>
    </div>
  );
};
