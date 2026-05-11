import { Link } from 'react-router-dom'

export default function Nav() {
  return (
    <nav style={{ padding: '1rem 2rem', borderBottom: '1px solid #e5e7eb', display: 'flex', gap: '1.5rem', alignItems: 'center' }}>
      <Link to="/" style={{ fontWeight: 700, fontSize: '1.1rem' }}>LaunchCart</Link>
      <Link to="/products">Products</Link>
      <span style={{ flex: 1 }} />
      <Link to="/admin/products">Admin: Products</Link>
      <Link to="/admin/enquiries">Admin: Enquiries</Link>
    </nav>
  )
}
