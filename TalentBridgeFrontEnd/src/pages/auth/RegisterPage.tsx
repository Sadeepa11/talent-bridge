import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { Button } from '../../components/ui/Button';

export const RegisterPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [termsAccepted, setTermsAccepted] = useState(false);
  const [loading, setLoading] = useState(false);
  const [errorMsg, setErrorMsg] = useState('');
  const { register } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      setErrorMsg("Passwords don't match");
      return;
    }
    setLoading(true);
    setErrorMsg('');
    try {
      await register({ email, password, termsAccepted });
      navigate('/candidate/dashboard', { replace: true });
    } catch (err: any) {
      console.error(err);
      setErrorMsg(err?.message || 'Registration failed.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="flex-col gap-4">
      {errorMsg && (
        <div style={{ color: 'var(--error)', backgroundColor: 'var(--error-container)', padding: '8px 12px', borderRadius: '4px', fontSize: '14px' }}>
          {errorMsg}
        </div>
      )}
      <div>
        <label className="label-md mb-2" style={{ display: 'block' }}>Email</label>
        <input type="email" className="input-field" value={email} onChange={e => setEmail(e.target.value)} required />
      </div>
      <div>
        <label className="label-md mb-2" style={{ display: 'block' }}>Password</label>
        <input type="password" className="input-field" value={password} onChange={e => setPassword(e.target.value)} required />
      </div>
      <div>
        <label className="label-md mb-2" style={{ display: 'block' }}>Confirm Password</label>
        <input type="password" className="input-field" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} required />
      </div>
      <label className="flex items-center gap-2 mt-2">
        <input type="checkbox" checked={termsAccepted} onChange={e => setTermsAccepted(e.target.checked)} required />
        <span className="text-sm">I accept the Terms and Conditions</span>
      </label>
      <Button type="submit" loading={loading} className="w-full mt-4">Register</Button>
      <div className="text-center mt-4 text-sm">
        Already have an account? <a href="/login">Login here</a>
      </div>
    </form>
  );
};
