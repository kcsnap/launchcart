import { useEffect, useState } from 'react'
import { useParams, Link } from 'react-router-dom'

interface Product {
  id: number
  name: string
  slug: string
  description: string
  imageUrl: string
  price: number
}

export default function ProductDetailPage() {
  const { slug }                    = useParams<{ slug: string }>()
  const [product, setProduct]       = useState<Product | null>(null)
  const [loading, setLoading]       = useState(true)
  const [notFound, setNotFound]     = useState(false)

  useEffect(() => {
    fetch(`/api/products/${slug}`)
      .then(r => {
        if (r.status === 404) { setNotFound(true); return null }
        return r.json()
      })
      .then(data => data && setProduct(data))
      .finally(() => setLoading(false))
  }, [slug])

  if (loading)  return <p>Loading…</p>
  if (notFound) return <p>Product not found. <Link to="/products">Back to products</Link></p>
  if (!product) return null

  return (
    <div>
      <Link to="/products">← Back to products</Link>
      <h1>{product.name}</h1>
      <p>{product.description}</p>
      <p><strong>£{product.price.toFixed(2)}</strong></p>
      <Link to={`/enquiry/${product.id}`}>
        <button>Make an enquiry</button>
      </Link>
    </div>
  )
}
