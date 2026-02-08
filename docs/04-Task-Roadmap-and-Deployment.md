# 🚀 Task Roadmap & Deployment Guide

## 📋 Project Execution Roadmap

### Phase Overview

```mermaid
gantt
    title WayPoint Implementation Timeline
    dateFormat  YYYY-MM-DD
    section Foundation
    Environment Setup           :p0_1, 2026-02-06, 2d
    Database Setup             :p0_2, after p0_1, 2d
    Backend Core               :p0_3, after p0_2, 5d
    
    section Core Features
    Location Intelligence      :p1_1, after p0_3, 4d
    Delivery Management       :p1_2, after p0_3, 4d
    Routing Integration       :p1_3, after p1_1, 5d
    
    section Frontend
    Web Admin Setup           :p1_4, after p0_3, 3d
    Map Integration          :p1_5, after p1_4, 4d
    Mobile App Core          :p1_6, after p1_4, 5d
    
    section Production
    Testing & QA             :p2_1, after p1_6, 5d
    Deployment               :p2_2, after p2_1, 3d
    Monitoring Setup         :p2_3, after p2_2, 2d
```

---

## 🎯 Task Management Table

### Phase 0: Foundation (Days 1-9) - CRITICAL PATH

| Task ID | Task Name | Priority | Estimated Hours | Dependencies | Assignee | Status |
|---------|-----------|----------|-----------------|--------------|----------|--------|
| **P0-001** | **Development Environment Setup** | P0 | 4h | None | DevOps | 🔴 TODO |
| P0-001a | Install .NET 8 SDK + PostgreSQL 16 + Redis | P0 | 1h | P0-001 | DevOps | 🔴 TODO |
| P0-001b | Configure Docker Compose for local dev | P0 | 2h | P0-001a | DevOps | 🔴 TODO |
| P0-001c | Setup IDE (Rider/VSCode) + Extensions | P0 | 1h | P0-001 | Dev | 🔴 TODO |
| **P0-002** | **PostgreSQL + PostGIS Setup** | P0 | 6h | P0-001 | Backend | 🔴 TODO |
| P0-002a | Execute PostGIS initialization script | P0 | 1h | P0-002 | Backend | 🔴 TODO |
| P0-002b | Create all tables + indexes | P0 | 2h | P0-002a | Backend | 🔴 TODO |
| P0-002c | Setup EF Core migrations | P0 | 2h | P0-002b | Backend | 🔴 TODO |
| P0-002d | Seed development data | P0 | 1h | P0-002c | Backend | 🔴 TODO |
| **P0-003** | **Backend Project Scaffolding** | P0 | 8h | P0-001 | Backend | 🔴 TODO |
| P0-003a | Create .NET solution + projects structure | P0 | 2h | P0-003 | Backend | 🔴 TODO |
| P0-003b | Setup Clean Architecture layers | P0 | 3h | P0-003a | Backend | 🔴 TODO |
| P0-003c | Configure MediatR + FluentValidation | P0 | 2h | P0-003b | Backend | 🔴 TODO |
| P0-003d | Setup Global Exception Handler | P0 | 1h | P0-003c | Backend | 🔴 TODO |
| **P0-004** | **Domain Layer Implementation** | P0 | 12h | P0-003 | Backend | 🔴 TODO |
| P0-004a | Create BaseEntity + ValueObject base classes | P0 | 2h | P0-004 | Backend | 🔴 TODO |
| P0-004b | Implement LatLng + Address value objects | P0 | 3h | P0-004a | Backend | 🔴 TODO |
| P0-004c | Create Location, Delivery, Rider entities | P0 | 4h | P0-004b | Backend | 🔴 TODO |
| P0-004d | Implement domain events | P0 | 3h | P0-004c | Backend | 🔴 TODO |

### Phase 1: Core Features (Days 10-23) - ESSENTIAL

| Task ID | Task Name | Priority | Estimated Hours | Dependencies | Assignee | Status |
|---------|-----------|----------|-----------------|--------------|----------|--------|
| **P1-001** | **Location Intelligence System** | P1 | 16h | P0-004 | Backend | 🔴 TODO |
| P1-001a | Implement LocationRepository with PostGIS queries | P1 | 4h | P1-001 | Backend | 🔴 TODO |
| P1-001b | Create SpatialDeduplicationService | P1 | 4h | P1-001a | Backend | 🔴 TODO |
| P1-001c | Implement LocationLearningService | P1 | 5h | P1-001b | Backend | 🔴 TODO |
| P1-001d | Build ConfidenceScoreCalculator logic | P1 | 3h | P1-001c | Backend | 🔴 TODO |
| **P1-002** | **Delivery Management** | P1 | 14h | P0-004 | Backend | 🔴 TODO |
| P1-002a | CreateDelivery command + handler | P1 | 3h | P1-002 | Backend | 🔴 TODO |
| P1-002b | AssignRider command + handler | P1 | 3h | P1-002a | Backend | 🔴 TODO |
| P1-002c | CompleteDelivery command (triggers learning) | P1 | 5h | P1-002b | Backend | 🔴 TODO |
| P1-002d | Delivery queries (GetById, GetRiderDeliveries) | P1 | 3h | P1-002c | Backend | 🔴 TODO |
| **P1-003** | **Routing Integration** | P1 | 20h | P1-001 | Backend | 🔴 TODO |
| P1-003a | Create OpenRouteService client wrapper | P1 | 4h | P1-003 | Backend | 🔴 TODO |
| P1-003b | Implement OSRM fallback service | P1 | 4h | P1-003a | Backend | 🔴 TODO |
| P1-003c | Setup Polly resilience policies | P1 | 3h | P1-003b | Backend | 🔴 TODO |
| P1-003d | Build RoutingServiceFactory (strategy pattern) | P1 | 3h | P1-003c | Backend | 🔴 TODO |
| P1-003e | Implement OptimizeRoute command (TSP solver) | P1 | 6h | P1-003d | Backend | 🔴 TODO |
| **P1-004** | **Caching Strategy** | P1 | 10h | P1-003 | Backend | 🔴 TODO |
| P1-004a | Setup Redis connection + configuration | P1 | 2h | P1-004 | Backend | 🔴 TODO |
| P1-004b | Implement RedisCacheService | P1 | 3h | P1-004a | Backend | 🔴 TODO |
| P1-004c | Create caching behavior for MediatR | P1 | 3h | P1-004b | Backend | 🔴 TODO |
| P1-004d | Cache route calculations | P1 | 2h | P1-004c | Backend | 🔴 TODO |
| **P1-005** | **Web Admin Foundation** | P1 | 12h | P0-003 | Frontend | 🔴 TODO |
| P1-005a | Setup Next.js 14 project with TypeScript | P1 | 2h | P1-005 | Frontend | 🔴 TODO |
| P1-005b | Configure Tailwind + Shadcn UI | P1 | 2h | P1-005a | Frontend | 🔴 TODO |
| P1-005c | Setup TanStack Query + Zustand | P1 | 2h | P1-005b | Frontend | 🔴 TODO |
| P1-005d | Create API client wrapper | P1 | 3h | P1-005c | Frontend | �4 TODO |
| P1-005e | Build authentication layout | P1 | 3h | P1-005d | Frontend | 🔴 TODO |
| **P1-006** | **Map Integration (Web)** | P1 | 16h | P1-005 | Frontend | 🔴 TODO |
| P1-006a | Setup Mapbox GL JS | P1 | 3h | P1-006 | Frontend | 🔴 TODO |
| P1-006b | Create MapContainer component | P1 | 3h | P1-006a | Frontend | 🔴 TODO |
| P1-006c | Implement marker components (Delivery, Rider) | P1 | 4h | P1-006b | Frontend | 🔴 TODO |
| P1-006d | Build route visualization (polylines) | P1 | 4h | P1-006c | Frontend | 🔴 TODO |
| P1-006e | Real-time rider tracking (WebSocket) | P1 | 2h | P1-006d | Frontend | 🔴 TODO |
| **P1-007** | **Mobile App Core** | P1 | 20h | P1-005 | Mobile | 🔴 TODO |
| P1-007a | Setup Expo project + React Native Maps | P1 | 3h | P1-007 | Mobile | 🔴 TODO |
| P1-007b | Configure expo-location for GPS tracking | P1 | 3h | P1-007a | Mobile | 🔴 TODO |
| P1-007c | Setup WatermelonDB for offline storage | P1 | 4h | P1-007b | Mobile | 🔴 TODO |
| P1-007d | Implement delivery list + detail screens | P1 | 5h | P1-007c | Mobile | 🔴 TODO |
| P1-007e | Build "Complete Delivery" flow with GPS capture | P1 | 5h | P1-007d | Mobile | 🔴 TODO |

### Phase 2: Polish & Production (Days 24-33) - NICE TO HAVE

| Task ID | Task Name | Priority | Estimated Hours | Dependencies | Assignee | Status |
|---------|-----------|----------|-----------------|--------------|----------|--------|
| **P2-001** | **Testing Suite** | P2 | 24h | P1-007 | QA | 🔴 TODO |
| P2-001a | Unit tests for domain layer | P2 | 6h | P2-001 | Backend | 🔴 TODO |
| P2-001b | Integration tests for repositories | P2 | 8h | P2-001a | Backend | 🔴 TODO |
| P2-001c | API endpoint tests | P2 | 6h | P2-001b | Backend | 🔴 TODO |
| P2-001d | Frontend component tests | P2 | 4h | P2-001c | Frontend | 🔴 TODO |
| **P2-002** | **Performance Optimization** | P2 | 12h | P2-001 | Backend | 🔴 TODO |
| P2-002a | Add response compression | P2 | 2h | P2-002 | Backend | 🔴 TODO |
| P2-002b | Optimize PostGIS query indexes | P2 | 4h | P2-002a | Backend | 🔴 TODO |
| P2-002c | Implement rate limiting | P2 | 3h | P2-002b | Backend | 🔴 TODO |
| P2-002d | Add database connection pooling | P2 | 3h | P2-002c | Backend | 🔴 TODO |
| **P2-003** | **Security Hardening** | P2 | 10h | P2-001 | DevOps | 🔴 TODO |
| P2-003a | Implement JWT authentication | P2 | 4h | P2-003 | Backend | 🔴 TODO |
| P2-003b | Add refresh token mechanism | P2 | 3h | P2-003a | Backend | 🔴 TODO |
| P2-003c | Setup CORS policies | P2 | 1h | P2-003b | Backend | 🔴 TODO |
| P2-003d | API key masking in frontend | P2 | 2h | P2-003c | Frontend | 🔴 TODO |
| **P2-004** | **Deployment** | P2 | 16h | P2-003 | DevOps | 🔴 TODO |
| P2-004a | Setup Fly.io for API deployment | P2 | 4h | P2-004 | DevOps | 🔴 TODO |
| P2-004b | Configure Supabase PostgreSQL | P2 | 3h | P2-004a | DevOps | 🔴 TODO |
| P2-004c | Deploy Next.js to Vercel | P2 | 2h | P2-004b | DevOps | 🔴 TODO |
| P2-004d | Setup Upstash Redis | P2 | 2h | P2-004c | DevOps | 🔴 TODO |
| P2-004e | Configure CI/CD pipeline | P2 | 5h | P2-004d | DevOps | 🔴 TODO |
| **P2-005** | **Monitoring & Observability** | P2 | 12h | P2-004 | DevOps | 🔴 TODO |
| P2-005a | Setup OpenTelemetry tracing | P2 | 4h | P2-005 | Backend | 🔴 TODO |
| P2-005b | Configure application insights | P2 | 3h | P2-005a | DevOps | 🔴 TODO |
| P2-005c | Setup error tracking (Sentry) | P2 | 3h | P2-005b | DevOps | 🔴 TODO |
| P2-005d | Create dashboards for metrics | P2 | 2h | P2-005c | DevOps | 🔴 TODO |

---

## 🔧 Environment Configuration

### Complete `.env.example` Template

```bash
# ============================================
# WAYPOINT SYSTEM CONFIGURATION
# Copy this file to .env and fill in values
# ============================================

# --------------------------------------------
# APPLICATION
# --------------------------------------------
NODE_ENV=development
APP_NAME=WayPoint
APP_VERSION=1.0.0
APP_URL=http://localhost:3000
API_URL=http://localhost:5000

# --------------------------------------------
# DATABASE (PostgreSQL + PostGIS)
# --------------------------------------------
# Local Development
DATABASE_HOST=localhost
DATABASE_PORT=5432
DATABASE_NAME=waypoint
DATABASE_USER=postgres
DATABASE_PASSWORD=your_secure_password_here
DATABASE_SSL_MODE=Prefer

# Connection String (Alternative)
DATABASE_URL=postgresql://postgres:password@localhost:5432/waypoint?sslmode=prefer

# Supabase (Production)
SUPABASE_DB_URL=postgresql://postgres:[PASSWORD]@db.[PROJECT_REF].supabase.co:5432/postgres
SUPABASE_API_URL=https://[PROJECT_REF].supabase.co
SUPABASE_ANON_KEY=your_supabase_anon_key

# --------------------------------------------
# REDIS (Caching)
# --------------------------------------------
# Local Development
REDIS_HOST=localhost
REDIS_PORT=6379
REDIS_PASSWORD=

# Upstash (Production)
UPSTASH_REDIS_URL=rediss://default:[PASSWORD]@[ENDPOINT].upstash.io:6379

# Cache Settings
CACHE_TTL_SECONDS=3600
CACHE_TTL_ROUTES=7200

# --------------------------------------------
# AUTHENTICATION & SECURITY
# --------------------------------------------
JWT_SECRET=your_super_secret_jwt_key_min_32_chars_long_random_string
JWT_ISSUER=waypoint-api
JWT_AUDIENCE=waypoint-clients
JWT_EXPIRATION_MINUTES=60
REFRESH_TOKEN_EXPIRATION_DAYS=7

# API Keys (for internal services)
API_KEY_ADMIN=admin_key_replace_with_random_string
API_KEY_MOBILE=mobile_key_replace_with_random_string

# --------------------------------------------
# GIS & ROUTING SERVICES
# --------------------------------------------
# OpenRouteService (Primary)
OPENROUTESERVICE_API_KEY=your_ors_api_key_here
OPENROUTESERVICE_BASE_URL=https://api.openrouteservice.org
OPENROUTESERVICE_RATE_LIMIT=40  # per minute

# OSRM (Fallback)
OSRM_BASE_URL=https://router.project-osrm.org
# Or self-hosted: http://your-osrm-server:5000

# Nominatim Geocoding
NOMINATIM_BASE_URL=https://nominatim.openstreetmap.org
NOMINATIM_USER_AGENT=WayPoint/1.0 (your-email@example.com)
NOMINATIM_RATE_LIMIT=1  # per second

# --------------------------------------------
# MAPPING SERVICES
# --------------------------------------------
# Mapbox (Web Admin)
NEXT_PUBLIC_MAPBOX_TOKEN=pk.eyJ1IjoieW91cnVzZXJuYW1lIiwiYSI6InlvdXJ0b2tlbiJ9.example

# Google Maps (Mobile - Optional)
GOOGLE_MAPS_API_KEY_IOS=your_ios_api_key
GOOGLE_MAPS_API_KEY_ANDROID=your_android_api_key

# --------------------------------------------
# FEATURE FLAGS
# --------------------------------------------
FEATURE_LOCATION_LEARNING=true
FEATURE_ROUTE_OPTIMIZATION=true
FEATURE_REAL_TIME_TRACKING=true
FEATURE_OFFLINE_MODE=true

# --------------------------------------------
# PERFORMANCE & LIMITS
# --------------------------------------------
# API Rate Limiting
RATE_LIMIT_REQUESTS=100
RATE_LIMIT_WINDOW_MINUTES=15

# Spatial Query Limits
MAX_NEARBY_LOCATIONS=100
MAX_ROUTE_WAYPOINTS=50
LOCATION_DEDUP_RADIUS_METERS=50

# File Upload
MAX_FILE_SIZE_MB=10

# --------------------------------------------
# LOGGING & MONITORING
# --------------------------------------------
LOG_LEVEL=Information  # Trace, Debug, Information, Warning, Error, Critical
SERILOG_SEQ_URL=http://localhost:5341  # Optional: Seq logging

# OpenTelemetry
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317
OTEL_SERVICE_NAME=waypoint-api

# Sentry (Error Tracking)
SENTRY_DSN=https://[KEY]@[ORG].ingest.sentry.io/[PROJECT]
SENTRY_ENVIRONMENT=development
SENTRY_TRACES_SAMPLE_RATE=0.1

# --------------------------------------------
# NOTIFICATIONS (Future)
# --------------------------------------------
TWILIO_ACCOUNT_SID=your_twilio_account_sid
TWILIO_AUTH_TOKEN=your_twilio_auth_token
TWILIO_PHONE_NUMBER=+1234567890

# --------------------------------------------
# STORAGE (Future)
# --------------------------------------------
AWS_S3_BUCKET=waypoint-files
AWS_ACCESS_KEY_ID=your_aws_access_key
AWS_SECRET_ACCESS_KEY=your_aws_secret_key
AWS_REGION=ap-southeast-1

# --------------------------------------------
# DEVELOPMENT ONLY
# --------------------------------------------
ASPNETCORE_ENVIRONMENT=Development
DOTNET_WATCH_RESTART_ON_RUDE_EDIT=true
```

---

## 🐳 Docker Compose Setup

### `docker-compose.yml` - Complete Local Development Stack

```yaml
version: '3.9'

services:
  # PostgreSQL with PostGIS
  postgres:
    image: postgis/postgis:16-3.4
    container_name: waypoint-postgres
    environment:
      POSTGRES_DB: waypoint
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres_dev_password
      PGDATA: /var/lib/postgresql/data/pgdata
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
      - ./infrastructure/scripts/init-postgis.sql:/docker-entrypoint-initdb.d/01-init.sql
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 10s
      timeout: 5s
      retries: 5
    networks:
      - waypoint-network

  # Redis Cache
  redis:
    image: redis:7-alpine
    container_name: waypoint-redis
    command: redis-server --appendonly yes --requirepass redis_dev_password
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    healthcheck:
      test: ["CMD", "redis-cli", "--raw", "incr", "ping"]
      interval: 10s
      timeout: 3s
      retries: 5
    networks:
      - waypoint-network

  # .NET API
  waypoint-api:
    build:
      context: ./backend
      dockerfile: ../infrastructure/docker/Dockerfile.api
    container_name: waypoint-api
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:5000
      - ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=waypoint;Username=postgres;Password=postgres_dev_password
      - Redis__Host=redis
      - Redis__Port=6379
      - Redis__Password=redis_dev_password
    ports:
      - "5000:5000"
    depends_on:
      postgres:
        condition: service_healthy
      redis:
        condition: service_healthy
    volumes:
      - ./backend:/app
      - /app/bin
      - /app/obj
    networks:
      - waypoint-network
    restart: unless-stopped

  # Next.js Web Admin (Optional - can run standalone)
  # waypoint-web:
  #   build:
  #     context: ./web-admin
  #     dockerfile: ../infrastructure/docker/Dockerfile.web
  #   container_name: waypoint-web
  #   environment:
  #     - NODE_ENV=development
  #     - NEXT_PUBLIC_API_URL=http://localhost:5000
  #   ports:
  #     - "3000:3000"
  #   volumes:
  #     - ./web-admin:/app
  #     - /app/node_modules
  #     - /app/.next
  #   depends_on:
  #     - waypoint-api
  #   networks:
  #     - waypoint-network

volumes:
  postgres_data:
    driver: local
  redis_data:
    driver: local

networks:
  waypoint-network:
    driver: bridge
```

### `Dockerfile.api` - Multi-stage .NET Build

```dockerfile
# ============================================
# Stage 1: Build
# ============================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["WayPoint.sln", "./"]
COPY ["src/WayPoint.Domain/WayPoint.Domain.csproj", "src/WayPoint.Domain/"]
COPY ["src/WayPoint.Application/WayPoint.Application.csproj", "src/WayPoint.Application/"]
COPY ["src/WayPoint.Infrastructure/WayPoint.Infrastructure.csproj", "src/WayPoint.Infrastructure/"]
COPY ["src/WayPoint.WebApi/WayPoint.WebApi.csproj", "src/WayPoint.WebApi/"]

# Restore dependencies
RUN dotnet restore "WayPoint.sln"

# Copy source code
COPY . .

# Build
WORKDIR "/src/src/WayPoint.WebApi"
RUN dotnet build "WayPoint.WebApi.csproj" -c Release -o /app/build

# ============================================
# Stage 2: Publish
# ============================================
FROM build AS publish
RUN dotnet publish "WayPoint.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ============================================
# Stage 3: Runtime
# ============================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for healthchecks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=publish /app/publish .

# Create non-root user
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

EXPOSE 5000

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:5000/health || exit 1

ENTRYPOINT ["dotnet", "WayPoint.WebApi.dll"]
```

---

## 🚀 Deployment Guides

### Fly.io Deployment (API)

#### Step 1: Install Fly CLI
```bash
curl -L https://fly.io/install.sh | sh
fly auth login
```

#### Step 2: Initialize Fly App
```bash
cd backend
fly launch --name waypoint-api --region sin --no-deploy

# Answer prompts:
# - Would you like to copy its configuration? Yes
# - Would you like to set up a PostgreSQL database? No (using Supabase)
# - Would you like to set up an Upstash Redis database? Yes
```

#### Step 3: Configure `fly.toml`
```toml
app = "waypoint-api"
primary_region = "sin"  # Singapore

[build]
  dockerfile = "../infrastructure/docker/Dockerfile.api"

[env]
  ASPNETCORE_ENVIRONMENT = "Production"
  ASPNETCORE_URLS = "http://+:8080"

[http_service]
  internal_port = 8080
  force_https = true
  auto_stop_machines = true
  auto_start_machines = true
  min_machines_running = 1
  processes = ["app"]

[[services.ports]]
  port = 80
  handlers = ["http"]
  force_https = true

[[services.ports]]
  port = 443
  handlers = ["tls", "http"]

[services.concurrency]
  type = "connections"
  hard_limit = 100
  soft_limit = 80

[[vm]]
  cpu_kind = "shared"
  cpus = 1
  memory_mb = 512  # Increase for production: 1024-2048
```

#### Step 4: Set Secrets
```bash
fly secrets set \
  DATABASE_URL="postgresql://..." \
  REDIS_URL="rediss://..." \
  JWT_SECRET="..." \
  OPENROUTESERVICE_API_KEY="..." \
  SENTRY_DSN="..."
```

#### Step 5: Deploy
```bash
fly deploy
fly status
fly logs
```

### Supabase Setup (Database)

#### Step 1: Create Project
1. Go to [supabase.com](https://supabase.com)
2. Create new project → Select region (Singapore)
3. Save database password

#### Step 2: Enable PostGIS
```sql
-- Run in SQL Editor
CREATE EXTENSION IF NOT EXISTS postgis;
SELECT PostGIS_Full_Version();
```

#### Step 3: Run Migrations
```bash
# From your local machine
psql "postgresql://postgres:[PASSWORD]@db.[PROJECT].supabase.co:5432/postgres" \
  < infrastructure/scripts/init-postgis.sql
```

#### Step 4: Connection String
```bash
DATABASE_URL="postgresql://postgres:[PASSWORD]@db.[PROJECT_REF].supabase.co:5432/postgres?sslmode=require"
```

### Vercel Deployment (Web Admin)

#### Step 1: Connect GitHub Repo
1. Go to [vercel.com](https://vercel.com)
2. Import Git Repository
3. Select `web-admin` as root directory

#### Step 2: Configure Build Settings
```bash
Build Command: npm run build
Output Directory: .next
Install Command: npm install
```

#### Step 3: Environment Variables
```bash
NEXT_PUBLIC_API_URL=https://waypoint-api.fly.dev
NEXT_PUBLIC_MAPBOX_TOKEN=pk.eyJ1...
```

#### Step 4: Deploy
```bash
# Or deploy via CLI
cd web-admin
vercel --prod
```

---

## 🔄 CI/CD Pipeline

### `.github/workflows/backend-ci.yml`

```yaml
name: Backend CI/CD

on:
  push:
    branches: [main, develop]
    paths:
      - 'backend/**'
  pull_request:
    branches: [main]
    paths:
      - 'backend/**'

jobs:
  test:
    runs-on: ubuntu-latest
    
    services:
      postgres:
        image: postgis/postgis:16-3.4
        env:
          POSTGRES_PASSWORD: postgres
          POSTGRES_DB: waypoint_test
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5
        ports:
          - 5432:5432

    steps:
      - uses: actions/checkout@v4
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      
      - name: Restore dependencies
        working-directory: ./backend
        run: dotnet restore
      
      - name: Build
        working-directory: ./backend
        run: dotnet build --no-restore --configuration Release
      
      - name: Test
        working-directory: ./backend
        run: dotnet test --no-build --configuration Release --verbosity normal
        env:
          DATABASE_URL: "Host=localhost;Port=5432;Database=waypoint_test;Username=postgres;Password=postgres"

  deploy:
    needs: test
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup Fly CLI
        uses: superfly/flyctl-actions/setup-flyctl@master
      
      - name: Deploy to Fly.io
        working-directory: ./backend
        run: flyctl deploy --remote-only
        env:
          FLY_API_TOKEN: ${{ secrets.FLY_API_TOKEN }}
```

---

## ✅ Pre-Launch Checklist

### Security
- [ ] All API keys stored in secrets (not committed)
- [ ] JWT secret is cryptographically strong (>32 chars)
- [ ] CORS policies configured correctly
- [ ] Rate limiting enabled on API
- [ ] HTTPS enforced on all services
- [ ] Database connections use SSL
- [ ] Input validation on all endpoints

### Performance
- [ ] PostGIS indexes created and verified
- [ ] Redis caching working for routes
- [ ] Response compression enabled
- [ ] Database connection pooling configured
- [ ] API response times < 200ms (P95)

### Monitoring
- [ ] Health check endpoints responding
- [ ] OpenTelemetry traces visible
- [ ] Error tracking (Sentry) configured
- [ ] Log aggregation working
- [ ] Uptime monitoring setup

### Documentation
- [ ] API documentation (Swagger) accessible
- [ ] README.md updated with setup instructions
- [ ] Architecture diagrams current
- [ ] Environment variables documented

---

## 📚 Useful Commands Reference

### Development Commands
```bash
# Backend
dotnet ef migrations add InitialCreate --project WayPoint.Infrastructure
dotnet ef database update --project WayPoint.Infrastructure
dotnet watch run --project WayPoint.WebApi

# Frontend (Web)
npm run dev
npm run build
npm run lint

# Mobile
npx expo start
npx expo start --ios
npx expo start --android

# Database
psql -h localhost -U postgres -d waypoint
pg_dump waypoint > backup.sql
psql waypoint < backup.sql

# Docker
docker-compose up -d
docker-compose logs -f waypoint-api
docker-compose down -v  # Caution: removes volumes
```

### Production Commands
```bash
# Fly.io
fly deploy
fly logs
fly ssh console
fly postgres connect -a waypoint-db

# Supabase
supabase db dump > backup.sql
supabase db push

# Vercel
vercel --prod
vercel logs
```

---

## 🎉 Success Metrics

### Technical KPIs
- API uptime: > 99.9%
- P95 response time: < 200ms
- Cache hit rate: > 80%
- Test coverage: > 80%
- Zero critical security vulnerabilities

### Business KPIs
- Location confidence improvement: +15% per week
- Route optimization savings: 20% distance reduction
- Rider productivity: +25% deliveries/day
- Customer satisfaction: 4.5+ stars

---

**Document Version**: 1.0  
**Last Updated**: February 2026  
**Estimated Total Effort**: 230 hours (~29 days with 1 full-time developer)  
**Status**: Ready for Execution 🚀
