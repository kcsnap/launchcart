import { useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'

export default function EnquiryPage() {
  const { productId } = useParams<{ productId: string }>()
  const navigate      = useNavigate()
  const [form, setForm] = useState({ name: '', email: '', message: '' })
  const [submitted, setSubmitted] = useState(false)
  const [error, setError]         = useState('')

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    const res = await fetch('/api/enquiries', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ productId: Number(productId), ...form })
    })
    if (res.ok) {
      setSubmitted(true)
    } else {
      setError('Something went wrong. Please try again.')
    }
  }

  if (submitted) return (
    <div>
      <h1>Enquiry sent</h1>
      <p>Thanks for your enquiry. We'll be in touch shortly.</p>
      <button onClick={() => navigate('/products')}>Back to products</button>
    </div>
  )

  return (
    <div>
      <h1>Make an enquiry</h1>
      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem', maxWidth: 400 }}>
        <input required placeholder="Your name"  value={form.name}    onChange={e => setForm(f => ({ ...f, name: e.target.value }))} />
        <input required type="email" placeholder="Email address" value={form.email}   onChange={e => setForm(f => ({ ...f, email: e.target.value }))} />
        <textarea required placeholder="Your message" rows={4} value={form.message} onChange={e => setForm(f => ({ ...f, message: e.target.value }))} />
        {error && <p style={{ color: 'red' }}>{error}</p>}
        <button type="submit">Send enquiry</button>
      </form>
    </div>
  )
}
