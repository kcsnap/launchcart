# LaunchCart — E-Commerce Starter Application

LaunchCart is a modern e-commerce starter app built with React and ASP.NET Core. It includes a product catalogue with detailed product pages, a purchase enquiry system, and a responsive admin interface for managing products and customer enquiries. Perfect for rapid prototyping or as a foundation for custom e-commerce solutions.

## What is LaunchCart?

- **Product Catalogue** — Browse and search products with detailed information and high-quality images
- **Enquiry System** — Customers can submit purchase enquiries directly from product pages
- **Admin Dashboard** — Manage products, update stock, and track customer enquiries
- **Responsive Design** — Mobile-first UI that works seamlessly across devices
- **Modern Stack** — Built with industry-standard tools (React, TypeScript, ASP.NET Core, PostgreSQL)

## Tech Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| **Frontend** | React + TypeScript | React 18+ |
| **API** | ASP.NET Core | .NET 8.0 |
| **Database** | PostgreSQL | 14+ |
| **ORM** | Entity Framework Core | 8.0+ |
| **Package Manager** | npm | 9+ |

## Quick Start

### Prerequisites

- **Node.js** 18+ (LTS recommended) and npm 9+
- **.NET SDK** 8.0 or later
- **PostgreSQL** 14+ (running locally or via Docker)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/kcsnap/launchcart.git
   cd launchcart
   ```

2. **Install frontend dependencies:**
   ```bash
   cd src/frontend
   npm install
   cd ../..
   ```

3. **Install API dependencies:**
   ```bash
   cd src/api
   dotnet restore
   cd ../..
   ```

4. **Configure environment variables:**
   - Copy the example environment file (if available):
     ```bash
     cp src/api/.env.example src/api/.env
     ```
   - Update `src/api/.env` with your PostgreSQL connection string:
     ```env
     ConnectionStrings:PostgreSQL=Host=localhost;Port=5432;Database=launchcart;Username=postgres;Password=your_password
     ```

5. **Run database migrations:**
   ```bash
   cd src/api
   dotnet ef database update
   cd ../..
   ```

### Running Locally

**Frontend** (runs on `http://localhost:3000`):
```bash
cd src/frontend
npm start
```

**API** (runs on `http://localhost:5000`):
```bash
cd src/api
dotnet run
```

Both services will start in development mode. The frontend automatically proxies API requests to the backend.

## Project Structure

```
launchcart/
├── src/
│   ├── frontend/                 # React + TypeScript application
│   │   ├── src/
│   │   │   ├── pages/           # Page components (Home, Products, Admin, etc.)
│   │   │   ├── components/      # Reusable UI components
│   │   │   ├── hooks/           # Custom React hooks
│   │   │   └── App.tsx          # Main application component
│   │   ├── package.json         # Frontend dependencies
│   │   └── tsconfig.json        # TypeScript configuration
│   │
│   └── api/                      # ASP.NET Core REST API
│       ├── Endpoints/           # API route handlers
│       ├── Domain/              # Business logic and entities
│       ├── Data/                # Database context and migrations
│       ├── Program.cs           # API startup configuration
│       └── *.csproj             # Project file
│
├── infra/                        # Infrastructure as code (Terraform, Docker, etc.)
├── .github/                      # GitHub Actions CI/CD workflows
├── docs/                         # Documentation
└── README.md                     # This file
```

## Pages & API Routes

### Frontend Pages

| Route | Page | Description |
|-------|------|-------------|
| `/` | HomePage | Landing page with featured products and promotional content |
| `/products` | ProductsPage | Full product catalogue with search and filtering |
| `/products/:slug` | ProductDetailPage | Detailed product view with purchase enquiry form |
| `/admin/products` | AdminProductsPage | Manage products (create, update, deactivate) |
| `/admin/enquiries` | AdminEnquiriesPage | View and manage customer enquiries |

### API Endpoints

| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/products` | List all active products |
| GET | `/api/products/:slug` | Get product details by slug |
| POST | `/api/enquiries` | Submit a new purchase enquiry |
| GET | `/api/admin/products` | List all products (admin) |
| POST | `/api/admin/products` | Create a new product (admin) |
| PATCH | `/api/admin/products/:id/deactivate` | Deactivate a product (admin) |
| GET | `/api/admin/enquiries` | List all enquiries (admin) |
| PATCH | `/api/admin/enquiries/:id/status` | Update enquiry status (admin) |

For detailed API documentation, see the request/response examples in the [API](./src/api) directory or use the built-in Swagger UI available at `http://localhost:5000/swagger` during development.

## Contributing

To contribute to LaunchCart:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature-name`)
3. Commit your changes with clear, descriptive messages
4. Push to your fork and submit a pull request

For detailed contribution guidelines, branch naming conventions, and development workflow, see [CONTRIBUTING.md](./CONTRIBUTING.md) (coming soon).

## License

LaunchCart is licensed under the MIT License — see [LICENSE](./LICENSE) for details.

---

**Questions?** Open an issue on GitHub or check the documentation in the `docs/` directory for more guidance on development, deployment, and architecture.