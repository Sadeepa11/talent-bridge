import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { normalizeRole } from '../../App';
import { Button } from '../../components/ui/Button';

export const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [errorMsg, setErrorMsg] = useState('');
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setErrorMsg('');
    try {
      await login({ email, password });
      const storedUserStr = localStorage.getItem('user');
      if (storedUserStr) {
        const user = JSON.parse(storedUserStr);
        const role = normalizeRole(user.role);
        navigate(`/${role}/dashboard`, { replace: true });
      } else {
        navigate('/candidate/dashboard', { replace: true });
      }
    } catch (err: any) {
      console.error(err);
      setErrorMsg(err?.message || 'Login failed. Check credentials.');
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
        <input 
          type="email" 
          className="input-field" 
          value={email}
          onChange={e => setEmail(e.target.value)}
          required
        />
      </div>
      <div>
        <label className="label-md mb-2" style={{ display: 'block' }}>Password</label>
        <input 
          type="password" 
          className="input-field" 
          value={password}
          onChange={e => setPassword(e.target.value)}
          required
        />
      </div>
      <Button type="submit" loading={loading} className="w-full mt-4">Login</Button>
      <div className="text-center mt-4 text-sm">
        Don't have an account? <a href="/register">Register here</a>
      </div>
    </form>
  );
};
