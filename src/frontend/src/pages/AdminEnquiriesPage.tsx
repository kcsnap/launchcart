import { useEffect, useState } from 'react'

interface Enquiry {
  id: number
  name: string
  email: string
  message: string
  status: string
  createdAtUtc: string
  product: { name: string }
}

export default function AdminEnquiriesPage() {
  const [enquiries, setEnquiries] = useState<Enquiry[]>([])

  const load = () =>
    fetch('/api/admin/enquiries').then(r => r.json()).then(setEnquiries)

  useEffect(() => { load() }, [])

  const updateStatus = async (id: number, status: string) => {
    await fetch(`/api/admin/enquiries/${id}/status`, {
      method: 'PATCH',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ status })
    })
    load()
  }

  return (
    <div>
      <h1>Admin — Enquiries</h1>
      {enquiries.length === 0 && <p>No enquiries yet.</p>}
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr>
            <th style={{ textAlign: 'left' }}>Product</th>
            <th style={{ textAlign: 'left' }}>From</th>
            <th style={{ textAlign: 'left' }}>Message</th>
            <th>Status</th>
            <th />
          </tr>
        </thead>
        <tbody>
          {enquiries.map(q => (
            <tr key={q.id} style={{ borderTop: '1px solid #e5e7eb', verticalAlign: 'top' }}>
              <td>{q.product?.name}</td>
              <td>{q.name}<br /><small>{q.email}</small></td>
              <td>{q.message}</td>
              <td>{q.status}</td>
              <td>
                {q.status === 'New' && (
                  <button onClick={() => updateStatus(q.id, 'Read')}>Mark read</button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
