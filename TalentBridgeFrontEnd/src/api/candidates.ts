import { apiClient } from './client';
import type { CandidateProfile, AccessEvent, Consent } from '../types';

export const candidatesApi = {
  getProfile: async () => {
    // const response = await apiClient.get<CandidateProfile>('/candidates/me');
    // return response.data;
    return {} as CandidateProfile;
  },
  updateProfile: async (data: Partial<CandidateProfile>) => {
    return {} as CandidateProfile;
  },
  submitProfile: async () => {
    return true;
  },
  getAccessLog: async () => {
    return [] as AccessEvent[];
  },
  getConsents: async () => {
    return [] as Consent[];
  },
  submitConsent: async (data: Partial<Consent>) => {
    return true;
  },
  withdraw: async () => {
    return true;
  },
  requestDataExport: async () => {
    return true;
  },
  requestDeletion: async () => {
    return true;
  }
};
