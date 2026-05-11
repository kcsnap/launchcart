import { Link } from 'react-router-dom'

export default function HomePage() {
  return (
    <div>
      <h1>Welcome to LaunchCart</h1>
      <p>Handcrafted goods, thoughtfully made.</p>
      <Link to="/products">
        <button>Browse products</button>
      </Link>
    </div>
  )
}
