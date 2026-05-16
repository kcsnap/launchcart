# LaunchCart

A modern, full-stack e-commerce starter application built with React, TypeScript, and ASP.NET Core. Perfect for launching product catalogues with an integrated purchase enquiry system.

## 🚀 Features

- **Product Catalogue** — Browse and search a dynamic product catalogue
- **Product Details** — Detailed product pages with images, descriptions, and specifications
- **Purchase Enquiries** — Customers can submit purchase enquiries directly from product pages
- **Admin Dashboard** — Manage products and track customer enquiries
- **Responsive Design** — Works seamlessly on desktop, tablet, and mobile
- **PostgreSQL Backend** — Robust, scalable database with Entity Framework Core ORM

## 📋 Table of Contents

- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Development](#development)
- [API Documentation](#api-documentation)
- [Database](#database)
- [Deployment](#deployment)
- [Contributing](#contributing)

## 🛠 Tech Stack

### Frontend
- **React** — UI library
- **TypeScript** — Type-safe JavaScript
- **React Router** — Client-side routing
- **Vite/Webpack** — Module bundler

### Backend
- **ASP.NET Core** — Web framework
- **Entity Framework Core** — ORM
- **PostgreSQL** — Database

### Infrastructure
- **Docker** — Containerization
- **Terraform** — Infrastructure as Code
- **GitHub Actions** — CI/CD pipeline

## 📁 Project Structure

```
launchcart/
├── src/
│   ├── frontend/                 # React TypeScript application
│   │   ├── public/
│   │   ├── src/
│   │   │   ├── components/       # Reusable UI components
│   │   │   ├── pages/            # Page components
│   │   │   ├── hooks/            # Custom React hooks
│   │   │   ├── services/         # API client services
│   │   │   ├── styles/           # Global styles
│   │   │   └── App.tsx
│   │   ├── package.json
│   │   └── tsconfig.json
│   │
│   └── api/                      # ASP.NET Core backend
│       ├── Data/
│       │   ├── ApplicationDbContext.cs
│       │   └── Migrations/       # EF Core migrations
│       ├── Domain/               # Entity models
│       │   ├── Product.cs
│       │   └── Enquiry.cs
│       ├── Endpoints/            # API route handlers
│       ├── Services/             # Business logic
│       ├── Program.cs
│       └── appsettings.json
│
├── infra/
│   └── terraform/                # Infrastructure definitions
│
├── .github/
│   └── workflows/                # CI/CD pipelines
│
└── docs/                         # Documentation
```

## 🚀 Getting Started

### Prerequisites

- **Node.js** 16+ (for frontend development)
- **.NET 7+** (for API development)
- **PostgreSQL** 12+ (for database)
- **Docker & Docker Compose** (optional, for containerized setup)

### Setup

#### 1. Clone the repository
```bash
git clone https://github.com/kcsnap/launchcart.git
cd launchcart
```

#### 2. Backend Setup

```bash
cd src/api

# Restore dependencies
dotnet restore

# Update database connection string in appsettings.json
# Update appsettings.json with your PostgreSQL connection:
# "ConnectionStrings": { "DefaultConnection": "Host=localhost;Database=launchcart;Username=postgres;Password=..." }

# Apply migrations
dotnet ef database update

# Run the API server
dotnet run
```

API will be available at `https://localhost:5001`

#### 3. Frontend Setup

```bash
cd src/frontend

# Install dependencies
npm install

# Create .env.local file with API endpoint
echo "REACT_APP_API_URL=https://localhost:5001" > .env.local

# Start development server
npm run dev
```

Frontend will be available at `http://localhost:3000`

#### 4. Docker Setup (Alternative)

```bash
docker-compose up -d
```

This will start:
- PostgreSQL on port 5432
- API on port 5001
- Frontend on port 3000

## 💻 Development

### Frontend Development

```bash
cd src/frontend

# Development server with hot reload
npm run dev

# Build for production
npm run build

# Run tests
npm test

# Lint and format code
npm run lint
npm run format
```

### Backend Development

```bash
cd src/api

# Development server
dotnet run

# Run tests
dotnet test

# Create new migration
dotnet ef migrations add <MigrationName>

# Apply pending migrations
dotnet ef database update
```

## 📚 API Documentation

### Authentication
Current version supports unauthenticated access. Admin endpoints should be protected in production.

### Products

#### List Products
```
GET /api/products
```

Query Parameters:
- `search` — Filter by name or description
- `page` — Pagination (default: 1)
- `pageSize` — Items per page (default: 20)

Response:
```json
{
  "data": [
    {
      "id": "uuid",
      "name": "Product Name",
      "slug": "product-name",
      "description": "Product description",
      "price": 99.99,
      "isActive": true,
      "createdAt": "2024-01-01T00:00:00Z"
    }
  ],
  "totalCount": 100
}
```

#### Get Product Detail
```
GET /api/products/:slug
```

Response:
```json
{
  "id": "uuid",
  "name": "Product Name",
  "slug": "product-name",
  "description": "Product description",
  "price": 99.99,
  "specification": "Technical details",
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### Enquiries

#### Submit Enquiry
```
POST /api/enquiries
```

Request:
```json
{
  "productId": "uuid",
  "customerName": "John Doe",
  "customerEmail": "john@example.com",
  "customerPhone": "+1234567890",
  "message": "I'm interested in this product"
}
```

Response:
```json
{
  "id": "uuid",
  "status": "new",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### Admin Endpoints

#### List Admin Products
```
GET /api/admin/products
```

#### Create Product
```
POST /api/admin/products
```

Request:
```json
{
  "name": "Product Name",
  "slug": "product-name",
  "description": "Description",
  "price": 99.99,
  "specification": "Technical details"
}
```

#### Deactivate Product
```
PATCH /api/admin/products/:id/deactivate
```

#### List Enquiries
```
GET /api/admin/enquiries
```

#### Update Enquiry Status
```
PATCH /api/admin/enquiries/:id/status
```

Request:
```json
{
  "status": "contacted|resolved|spam"
}
```

## 🗄 Database

### Schema

#### Products Table
```sql
CREATE TABLE products (
  id UUID PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  slug VARCHAR(255) UNIQUE NOT NULL,
  description TEXT,
  specification TEXT,
  price DECIMAL(10, 2),
  is_active BOOLEAN DEFAULT true,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

#### Enquiries Table
```sql
CREATE TABLE enquiries (
  id UUID PRIMARY KEY,
  product_id UUID REFERENCES products(id),
  customer_name VARCHAR(255) NOT NULL,
  customer_email VARCHAR(255) NOT NULL,
  customer_phone VARCHAR(20),
  message TEXT,
  status VARCHAR(50) DEFAULT 'new',
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### Migrations

Migrations are managed via Entity Framework Core:

```bash
# View pending migrations
dotnet ef migrations list

# Create migration
dotnet ef migrations add <MigrationName>

# Apply migrations
dotnet ef database update

# Revert last migration
dotnet ef database update <PreviousMigrationName>
```

## 🚢 Deployment

### Prerequisites
- AWS account or preferred cloud provider
- Terraform installed
- Docker Hub account (for image registry)

### Deploy with Terraform

```bash
cd infra/terraform

# Initialize Terraform
terraform init

# Plan deployment
terraform plan

# Apply deployment
terraform apply
```

### Environment Variables

Create `.env` files for each environment:

**Backend** (`src/api/appsettings.Production.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=$DB_HOST;Database=$DB_NAME;Username=$DB_USER;Password=$DB_PASSWORD"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

**Frontend** (`.env.production`):
```
REACT_APP_API_URL=https://api.yourdomain.com
```

### CI/CD Pipeline

GitHub Actions workflows automatically:
1. Run tests on every push
2. Build Docker images
3. Push to registry
4. Deploy to staging/production

View workflows in `.github/workflows/`

## 📖 Pages

### Frontend Routes

| Route | Component | Description |
|-------|-----------|-------------|
| `/` | HomePage | Landing page with featured products |
| `/products` | ProductsPage | Full product catalogue with search and filters |
| `/products/:slug` | ProductDetailPage | Product details with enquiry form |
| `/admin/products` | AdminProductsPage | Manage products (create, edit, deactivate) |
| `/admin/enquiries` | AdminEnquiriesPage | View and manage customer enquiries |

## 🔒 Security Considerations

- **Admin Endpoints**: Require authentication (not implemented in starter, add OAuth/JWT)
- **Input Validation**: All endpoints validate input server-side
- **CORS**: Configure appropriately for production
- **Database**: Use environment variables for connection strings
- **HTTPS**: Required for production deployments

## 📝 Environment Variables

### Backend
```
ASPNETCORE_ENVIRONMENT=Development|Production
ConnectionString__DefaultConnection=<postgresql-connection-string>
```

### Frontend
```
REACT_APP_API_URL=<api-base-url>
REACT_APP_ENV=development|production
```

## 🧪 Testing

### Backend
```bash
cd src/api
dotnet test
```

### Frontend
```bash
cd src/frontend
npm test
npm run test:coverage
```

## 🐛 Troubleshooting

### Database Connection Issues
- Verify PostgreSQL is running
- Check connection string in `appsettings.json`
- Ensure database user has proper permissions

### Migration Failures
```bash
# Reset database (development only)
dotnet ef database drop --force
dotnet ef database update
```

### CORS Errors
- Check API CORS configuration in `Program.cs`
- Verify frontend URL is whitelisted

## 📚 Documentation

Additional documentation available in `/docs`:
- Architecture decisions
- API specifications
- Deployment guides
- Contributing guidelines

## 🤝 Contributing

1. Create a feature branch (`git checkout -b feature/amazing-feature`)
2. Commit changes (`git commit -m 'Add amazing feature'`)
3. Push to branch (`git push origin feature/amazing-feature`)
4. Open a Pull Request

## 📄 License

This project is licensed under the MIT License — see LICENSE file for details.

## 💬 Support

For issues and questions:
- GitHub Issues: [Report a bug](https://github.com/kcsnap/launchcart/issues)
- Discussions: [Ask a question](https://github.com/kcsnap/launchcart/discussions)

## 🙏 Acknowledgments

Built with modern web technologies to provide a solid foundation for e-commerce applications.

---

**Last Updated:** January 2024  
**Version:** 1.0.0  
**Maintainer:** LaunchCart Team