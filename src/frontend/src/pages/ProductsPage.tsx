import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'

interface Product {
  id: number
  name: string
  slug: string
  imageUrl: string
  price: number
}

export default function ProductsPage() {
  const [products, setProducts] = useState<Product[]>([])
  const [loading, setLoading]   = useState(true)

  useEffect(() => {
    fetch('/api/products')
      .then(r => r.json())
      .then(setProducts)
      .finally(() => setLoading(false))
  }, [])

  if (loading) return <p>Loading…</p>

  return (
    <div>
      <h1>Products</h1>
      <ul style={{ listStyle: 'none', padding: 0, display: 'grid', gridTemplateColumns: 'repeat(auto-fill,minmax(220px,1fr))', gap: '1rem' }}>
        {products.map(p => (
          <li key={p.id} style={{ border: '1px solid #e5e7eb', borderRadius: 8, padding: '1rem' }}>
            <Link to={`/products/${p.slug}`}>
              <strong>{p.name}</strong>
            </Link>
            <p>£{p.price.toFixed(2)}</p>
          </li>
        ))}
      </ul>
    </div>
  )
}
