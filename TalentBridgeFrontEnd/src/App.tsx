import React from 'react';
import { BrowserRouter, Routes, Route, Navigate, useNavigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { AuthLayout } from './components/layout/AuthLayout';
import { MainLayout } from './components/layout/MainLayout';

// Auth
import { LoginPage } from './pages/auth/LoginPage';
import { RegisterPage } from './pages/auth/RegisterPage';
import { ForgotPasswordPage } from './pages/auth/ForgotPasswordPage';

// Admin
import { AdminDashboard } from './pages/admin/AdminDashboard';
import { ModerationQueue } from './pages/admin/ModerationQueue';
import { ModerationDetail } from './pages/admin/ModerationDetail';
import { CompaniesPage } from './pages/admin/CompaniesPage';
import { CompanyDetailPage } from './pages/admin/CompanyDetailPage';
import { BatchCurationPage } from './pages/admin/BatchCurationPage';
import { GrantsPage } from './pages/admin/GrantsPage';
import { OrdersPage } from './pages/admin/OrdersPage';
import { OrderDetailPage } from './pages/admin/OrderDetailPage';
import { FollowUpsPage } from './pages/admin/FollowUpsPage';
import { ReportsPage } from './pages/admin/ReportsPage';
import { SettingsPage } from './pages/admin/SettingsPage';

// Company
import { CompanyDashboard } from './pages/company/CompanyDashboard';
import { CandidateGridPage } from './pages/company/CandidateGridPage';
import { CandidateProfilePage } from './pages/company/CandidateProfilePage';
import { AccessRequestsPage } from './pages/company/AccessRequestsPage';

// Candidate
import { CandidateDashboard } from './pages/candidate/CandidateDashboard';
import { ProfileWizardPage } from './pages/candidate/ProfileWizardPage';
import { AccessLogPage } from './pages/candidate/AccessLogPage';
import { ConsentPage } from './pages/candidate/ConsentPage';
import { DataRightsPage } from './pages/candidate/DataRightsPage';

export const normalizeRole = (role?: string) => {
  if (!role) return '';
  const r = role.toLowerCase();
  if (r === 'superadmin' || r === 'opsadmin' || r === 'admin') return 'admin';
  if (r === 'companyuser' || r === 'company_user') return 'company_user';
  if (r === 'candidate') return 'candidate';
  return r;
};

const ProtectedRoute = ({ children, allowedRoles }: { children: React.ReactNode, allowedRoles?: string[] }) => {
  const { user, isAuthenticated, isLoading } = useAuth();
  
  if (isLoading) return <div className="flex items-center justify-center min-h-screen">Loading...</div>;
  if (!isAuthenticated) return <Navigate to="/login" replace />;
  
  const userRole = normalizeRole(user?.role);
  if (allowedRoles && user && !allowedRoles.includes(userRole)) {
    return <Navigate to={`/${userRole}/dashboard`} replace />;
  }
  
  return <>{children}</>;
};

const RootRedirect = () => {
  const { user, isAuthenticated, isLoading } = useAuth();
  if (isLoading) return <div className="flex items-center justify-center min-h-screen">Loading...</div>;
  if (isAuthenticated && user) {
    const userRole = normalizeRole(user.role);
    return <Navigate to={`/${userRole}/dashboard`} replace />;
  }
  return <Navigate to="/login" replace />;
};

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<RootRedirect />} />
      
      <Route element={<AuthLayout />}>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/forgot-password" element={<ForgotPasswordPage />} />
      </Route>
      
      <Route path="/admin" element={<ProtectedRoute allowedRoles={['admin']}><MainLayout /></ProtectedRoute>}>
        <Route path="dashboard" element={<AdminDashboard />} />
        <Route path="moderation" element={<ModerationQueue />} />
        <Route path="moderation/:id" element={<ModerationDetail />} />
        <Route path="companies" element={<CompaniesPage />} />
        <Route path="companies/:id" element={<CompanyDetailPage />} />
        <Route path="batches/new" element={<BatchCurationPage />} />
        <Route path="batches" element={<BatchCurationPage />} />
        <Route path="grants" element={<GrantsPage />} />
        <Route path="orders" element={<OrdersPage />} />
        <Route path="orders/:id" element={<OrderDetailPage />} />
        <Route path="follow-ups" element={<FollowUpsPage />} />
        <Route path="reports" element={<ReportsPage />} />
        <Route path="settings" element={<SettingsPage />} />
      </Route>

      <Route path="/company" element={<ProtectedRoute allowedRoles={['company_user']}><MainLayout /></ProtectedRoute>}>
        <Route path="dashboard" element={<CompanyDashboard />} />
        <Route path="candidates" element={<CandidateGridPage />} />
        <Route path="candidates/:refCode" element={<CandidateProfilePage />} />
        <Route path="access-requests" element={<AccessRequestsPage />} />
        <Route path="outcomes" element={<CompanyDashboard />} />
      </Route>
      
      <Route path="/candidate" element={<ProtectedRoute allowedRoles={['candidate']}><MainLayout /></ProtectedRoute>}>
        <Route path="dashboard" element={<CandidateDashboard />} />
        <Route path="profile" element={<ProfileWizardPage />} />
        <Route path="profile/edit" element={<ProfileWizardPage />} />
        <Route path="access-log" element={<AccessLogPage />} />
        <Route path="consent" element={<ConsentPage />} />
        <Route path="data-rights" element={<DataRightsPage />} />
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
};

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <AppRoutes />
      </BrowserRouter>
    </AuthProvider>
  );
}
