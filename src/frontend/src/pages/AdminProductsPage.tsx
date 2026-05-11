import { useEffect, useState } from 'react'

interface Product {
  id: number
  name: string
  slug: string
  price: number
  isActive: boolean
}

export default function AdminProductsPage() {
  const [products, setProducts] = useState<Product[]>([])

  const load = () =>
    fetch('/api/admin/products').then(r => r.json()).then(setProducts)

  useEffect(() => { load() }, [])

  const deactivate = async (id: number) => {
    await fetch(`/api/admin/products/${id}/deactivate`, { method: 'PATCH' })
    load()
  }

  return (
    <div>
      <h1>Admin — Products</h1>
      <table style={{ width: '100%', borderCollapse: 'collapse' }}>
        <thead>
          <tr>
            <th style={{ textAlign: 'left' }}>Name</th>
            <th>Price</th>
            <th>Active</th>
            <th />
          </tr>
        </thead>
        <tbody>
          {products.map(p => (
            <tr key={p.id} style={{ borderTop: '1px solid #e5e7eb' }}>
              <td>{p.name}</td>
              <td>£{p.price.toFixed(2)}</td>
              <td>{p.isActive ? '✓' : '✗'}</td>
              <td>
                {p.isActive && (
                  <button onClick={() => deactivate(p.id)}>Deactivate</button>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
