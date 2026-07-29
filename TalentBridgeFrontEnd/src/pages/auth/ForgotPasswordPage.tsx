import React, { useState } from 'react';
import { Button } from '../../components/ui/Button';

export const ForgotPasswordPage = () => {
  const [email, setEmail] = useState('');
  const [submitted, setSubmitted] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitted(true);
  };

  if (submitted) {
    return (
      <div className="text-center">
        <h2 className="headline-sm mb-4">Check your email</h2>
        <p className="body-md mb-4">We've sent password reset instructions to {email}</p>
        <a href="/login">Back to Login</a>
      </div>
    );
  }

  return (
    <form onSubmit={handleSubmit} className="flex-col gap-4">
      <div>
        <label className="label-md mb-2" style={{ display: 'block' }}>Email</label>
        <input type="email" className="input-field" value={email} onChange={e => setEmail(e.target.value)} required />
      </div>
      <Button type="submit" className="w-full mt-4">Reset Password</Button>
      <div className="text-center mt-4 text-sm">
        <a href="/login">Back to Login</a>
      </div>
    </form>
  );
};
