export interface User {
  id: string;
  email: string;
  role: 'admin' | 'company_user' | 'candidate';
  name: string;
  companyId?: string;
  avatar?: string;
}

export interface AuthResponse {
  user: User;
  token: string;
}

export interface LoginCredentials {
  email: string;
  password?: string;
}

export interface RegisterCredentials {
  email: string;
  password?: string;
  termsAccepted: boolean;
}

export interface CandidatePii {
  name: string;
  email: string;
  phone: string;
  address: string;
  nic: string;
  dob: string;
  photoBase64?: string;
}

export interface Experience {
  id: string;
  title: string;
  company: string;
  startDate: string;
  endDate?: string;
  current: boolean;
  description: string;
}

export interface Qualification {
  id: string;
  institution: string;
  degree: string;
  year: number;
}

export interface Document {
  id: string;
  name: string;
  type: string;
  url: string; // or base64
}

export interface BaseProfile {
  id: string;
  referenceCode: string;
  category: string;
  position: string;
  city: string;
  skills: string[];
  salaryRange: string;
  availability: string;
  status: 'draft' | 'submitted' | 'approved' | 'rejected' | 'placed' | 'withdrawn';
  experiences: Experience[];
  qualifications: Qualification[];
}

export interface PreviewProfile extends BaseProfile {
  // PII is omitted
}

export interface FullProfile extends BaseProfile {
  pii: CandidatePii;
  documents: Document[];
}

export type CandidateProfile = PreviewProfile | FullProfile;

export interface Company {
  id: string;
  name: string;
  industry: string;
  status: 'active' | 'pending' | 'inactive';
  contactEmail: string;
  onboardingDate: string;
}

export interface CompanyUser {
  id: string;
  companyId: string;
  name: string;
  email: string;
  role: string;
}

export interface Batch {
  id: string;
  companyId: string;
  createdAt: string;
  candidates: string[]; // reference codes
  status: 'active' | 'closed';
}

export interface Grant {
  id: string;
  batchId: string;
  companyId: string;
  candidateId: string;
  scope: 'preview' | 'full';
  status: 'active' | 'expiring' | 'expired' | 'revoked';
  expiresAt: string;
}

export interface GrantResolution {
  grantId: string;
  action: 'upgrade' | 'extend' | 'revoke';
}

export interface OrderItem {
  id: string;
  description: string;
  amount: number;
}

export interface Order {
  id: string;
  companyId: string;
  status: 'pending' | 'paid' | 'cancelled';
  total: number;
  items: OrderItem[];
  createdAt: string;
}

export interface AccessEvent {
  id: string;
  candidateId: string;
  companyId: string;
  companyName: string;
  type: 'preview' | 'full';
  accessedAt: string;
}

export interface AccessRequest {
  id: string;
  candidateId: string;
  companyId: string;
  status: 'pending' | 'approved' | 'rejected';
  requestedAt: string;
}

export interface Consent {
  id: string;
  candidateId: string;
  companyId: string;
  scope: 'limited' | 'full';
  grantedAt: string;
  status: 'active' | 'withdrawn';
}

export interface Outcome {
  id: string;
  candidateId: string;
  companyId: string;
  status: 'interviewing' | 'hired' | 'rejected';
  notes?: string;
  reportedAt: string;
}

export interface FollowUpTask {
  id: string;
  title: string;
  dueDate: string;
  status: 'pending' | 'completed';
}

export interface Notification {
  id: string;
  userId: string;
  message: string;
  read: boolean;
  createdAt: string;
}

export interface Setting {
  key: string;
  value: string;
}

export interface DashboardStats {
  publishedCandidates: number;
  availableCandidates: number;
  reservedCandidates: number;
  activeGrants: number;
  expiringGrants: number;
  awaitingLkr: number;
}

export interface CategoryBreakdown {
  category: string;
  available: number;
  reserved: number;
}
