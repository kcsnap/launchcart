import { Routes, Route } from 'react-router-dom'
import Nav from './components/Nav'
import HomePage from './pages/HomePage'
import ProductsPage from './pages/ProductsPage'
import ProductDetailPage from './pages/ProductDetailPage'
import EnquiryPage from './pages/EnquiryPage'
import AdminProductsPage from './pages/AdminProductsPage'
import AdminEnquiriesPage from './pages/AdminEnquiriesPage'

export default function App() {
  return (
    <>
      <Nav />
      <main style={{ padding: '1rem 2rem' }}>
        <Routes>
          <Route path="/"                   element={<HomePage />} />
          <Route path="/products"           element={<ProductsPage />} />
          <Route path="/products/:slug"     element={<ProductDetailPage />} />
          <Route path="/enquiry/:productId" element={<EnquiryPage />} />
          <Route path="/admin/products"     element={<AdminProductsPage />} />
          <Route path="/admin/enquiries"    element={<AdminEnquiriesPage />} />
        </Routes>
      </main>
    </>
  )
}
