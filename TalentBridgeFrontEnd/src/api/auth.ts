import { apiClient } from './client';
import type { AuthResponse, LoginCredentials, RegisterCredentials, User } from '../types';

export const authApi = {
  login: async (credentials: LoginCredentials) => {
    // const response = await apiClient.post<AuthResponse>('/auth/login', credentials);
    // return response.data;
    
    // MOCK DATA for now
    if (credentials.email === 'admin@talentbridge.com') {
      return { user: { id: '1', email: 'admin@tb.com', role: 'admin', name: 'Admin' }, token: 'mock-token' };
    }
    if (credentials.email === 'company@test.com') {
      return { user: { id: '2', email: 'company@test.com', role: 'company_user', name: 'Company User', companyId: 'c1' }, token: 'mock-token' };
    }
    return { user: { id: '3', email: credentials.email, role: 'candidate', name: 'Candidate' }, token: 'mock-token' };
  },
  register: async (credentials: RegisterCredentials) => {
    // const response = await apiClient.post<AuthResponse>('/auth/register', credentials);
    // return response.data;
    return { user: { id: '3', email: credentials.email, role: 'candidate', name: 'Candidate' }, token: 'mock-token' };
  },
  logout: async () => {
    // await apiClient.post('/auth/logout');
  },
  getMe: async () => {
    // const response = await apiClient.get<User>('/auth/me');
    // return response.data;
    const userStr = localStorage.getItem('user');
    if (userStr) return JSON.parse(userStr);
    throw new Error('Not authenticated');
  }
};
