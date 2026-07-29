import { apiClient } from './client';
import type { CandidateProfile, Company, Batch, Grant, Order, DashboardStats } from '../types';

export const adminApi = {
  getModerationQueue: async () => {
    return [] as CandidateProfile[];
  },
  getCandidateForModeration: async (id: string) => {
    return null;
  },
  approveCandidate: async (id: string) => {
    return true;
  },
  rejectCandidate: async (id: string, reason: string) => {
    return true;
  },
  getCompanies: async () => {
    return [] as Company[];
  },
  getBatches: async () => {
    return [] as Batch[];
  },
  getGrants: async () => {
    return [] as Grant[];
  },
  getOrders: async () => {
    return [] as Order[];
  },
  getDashboardStats: async () => {
    return {
      publishedCandidates: 1245,
      availableCandidates: 890,
      reservedCandidates: 355,
      activeGrants: 120,
      expiringGrants: 15,
      awaitingLkr: 5
    } as DashboardStats;
  }
};
