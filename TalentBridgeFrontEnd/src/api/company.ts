import { apiClient } from './client';
import type { Batch, CandidateProfile, AccessRequest } from '../types';

export const companyApi = {
  getBatches: async () => {
    return [] as Batch[];
  },
  getCandidates: async () => {
    return [] as CandidateProfile[];
  },
  getCandidate: async (refCode: string) => {
    return null;
  },
  createAccessRequest: async (candidateId: string) => {
    return {} as AccessRequest;
  },
  getAccessRequests: async () => {
    return [] as AccessRequest[];
  },
  reportOutcome: async (data: any) => {
    return true;
  },
  getDashboard: async () => {
    return {};
  }
};
