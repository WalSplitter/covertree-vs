# Deployment Guide

This guide covers deploying the Base Project Template to production environments.

## Deployment Overview

```
Development → Staging → Production
     ↓            ↓          ↓
  Localhost   Heroku/AWS   CloudFlare/AWS
```

## Backend Deployment

### Option 1: Docker to AWS ECS

#### 1. Build Docker Image

```bash
docker build -t base-template:latest .
```

#### 2. Push to ECR

```bash
# Login to AWS ECR
aws ecr get-login-password --region us-east-1 | \
  docker login --username AWS --password-stdin <account>.dkr.ecr.us-east-1.amazonaws.com

# Tag image
docker tag base-template:latest <account>.dkr.ecr.us-east-1.amazonaws.com/base-template:latest

# Push image
docker push <account>.dkr.ecr.us-east-1.amazonaws.com/base-template:latest
```

#### 3. Deploy to ECS

```bash
# Update ECS service
aws ecs update-service --cluster base-template \
  --service backend \
  --force-new-deployment
```

### Option 2: Vercel (Express API)

#### 1. Setup Vercel

```bash
npm install -g vercel
vercel login
```

#### 2. Create `vercel.json`

```json
{
  "version": 2,
  "builds": [
    {
      "src": "apps/web-backend/dist/index.js",
      "use": "@vercel/node"
    }
  ],
  "routes": [
    {
      "src": "/(.*)",
      "dest": "apps/web-backend/dist/index.js"
    }
  ]
}
```

#### 3. Deploy

```bash
vercel --prod
```

### Option 3: Railway.app

1. Connect GitHub repository
2. Select `apps/web-backend` as root directory
3. Set environment variables
4. Deploy

### Option 4: Render.com

1. Connect GitHub repository
2. Create new Web Service
3. Set Start Command: `npm run build && npm start`
4. Set Environment Variables
5. Deploy

## Frontend Deployment

### Option 1: Vercel

#### 1. Connect Repository

```bash
vercel
```

Select `apps/web-frontend` as root directory.

#### 2. Configure `vercel.json`

```json
{
  "buildCommand": "npm run build",
  "outputDirectory": "dist"
}
```

#### 3. Deploy

```bash
vercel --prod
```

### Option 2: Netlify

#### 1. Create `netlify.toml`

```toml
[build]
command = "npm run build"
publish = "dist"

[[redirects]]
from = "/*"
to = "/index.html"
status = 200
```

#### 2. Deploy

```bash
npm install -g netlify-cli
netlify deploy --prod
```

### Option 3: AWS S3 + CloudFront

```bash
# Build
npm run build

# Deploy to S3
aws s3 sync apps/web-frontend/dist s3://my-bucket/

# Invalidate CloudFront
aws cloudfront create-invalidation \
  --distribution-id E123ABC \
  --paths "/*"
```

## Database Deployment

### PostgreSQL on AWS RDS

#### 1. Create RDS Instance

```bash
aws rds create-db-instance \
  --db-instance-identifier base-template-db \
  --db-instance-class db.t3.micro \
  --engine postgres \
  --master-username admin \
  --master-user-password <secure-password>
```

#### 2. Run Migrations

```bash
DATABASE_URL=postgresql://admin:pass@host:5432/db npm run migrate
```

### PostgreSQL on Railway

1. Add PostgreSQL plugin
2. Copy connection string
3. Run migrations

## Environment Variables

### Production `.env`

```env
NODE_ENV=production
PORT=3000

# Database
DATABASE_URL=postgresql://user:pass@host:5432/db
DATABASE_POOL_SIZE=20

# Redis
REDIS_URL=redis://host:6379

# JWT
JWT_SECRET=<long-random-string>
JWT_EXPIRY=24h

# Frontend
VITE_API_BASE_URL=https://api.example.com
VITE_APP_NAME=Production App

# Monitoring
SENTRY_DSN=https://xxxxx@sentry.io/xxxxx

# Security
CORS_ORIGIN=https://example.com
RATE_LIMIT_WINDOW_MS=900000
RATE_LIMIT_MAX_REQUESTS=100
```

## CI/CD Pipeline

### GitHub Actions Production Deployment

Create `.github/workflows/deploy.yml`:

```yaml
name: Deploy to Production

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Build
        run: npm install && npm run build

      - name: Deploy Backend
        run: npm run deploy:backend
        env:
          DEPLOY_KEY: ${{ secrets.DEPLOY_KEY }}

      - name: Deploy Frontend
        run: npm run deploy:frontend
        env:
          VERCEL_TOKEN: ${{ secrets.VERCEL_TOKEN }}
```

## Monitoring & Logging

### Application Monitoring

```javascript
// Backend monitoring
import Sentry from '@sentry/node';

Sentry.init({
  dsn: process.env.SENTRY_DSN,
  environment: process.env.NODE_ENV,
  tracesSampleRate: 0.1,
});
```

### Health Checks

```bash
curl https://api.example.com/api/health
```

Response:

```json
{
  "status": "ok",
  "timestamp": "2026-06-01T12:00:00Z"
}
```

### Log Aggregation

Set up with:

- **CloudWatch** (AWS)
- **New Relic**
- **Datadog**
- **LogRocket**

## Scaling Strategies

### Horizontal Scaling (Multiple Instances)

```bash
# AWS ECS auto-scaling
aws autoscaling create-auto-scaling-group \
  --auto-scaling-group-name backend-asg \
  --min-size 2 \
  --max-size 10 \
  --desired-capacity 3
```

### Vertical Scaling (Larger Instance)

Increase instance size when CPU/Memory usage is consistently high.

### Database Optimization

- Enable read replicas
- Implement caching (Redis)
- Use connection pooling
- Optimize queries

## Disaster Recovery

### Backup Strategy

```bash
# Daily automated backups
aws rds create-db-snapshot \
  --db-instance-identifier base-template-db \
  --db-snapshot-identifier base-template-backup-$(date +%Y%m%d)
```

### Recovery Procedure

1. Identify issue
2. Restore from backup
3. Verify data integrity
4. Resume operations

### Rollback Plan

```bash
# Revert to previous Docker image
aws ecs update-service \
  --cluster base-template \
  --service backend \
  --force-new-deployment
```

## Security Checklist

- [ ] Enable HTTPS/TLS
- [ ] Set secure environment variables
- [ ] Enable WAF (Web Application Firewall)
- [ ] Setup rate limiting
- [ ] Enable CORS properly
- [ ] Use security headers
- [ ] Enable database encryption
- [ ] Setup backup encryption
- [ ] Enable VPC security groups
- [ ] Setup DDoS protection

## Performance Optimization

- Enable gzip compression
- Use CDN for static assets
- Implement caching headers
- Optimize database queries
- Use connection pooling
- Enable query result caching
- Implement pagination
- Compress images

## Post-Deployment

### 1. Verify Deployment

```bash
curl https://api.example.com/api/health
curl https://example.com
```

### 2. Monitor Logs

```bash
# Docker logs
docker logs base-template-backend

# AWS CloudWatch
aws logs tail /ecs/base-template --follow
```

### 3. Run Smoke Tests

```bash
npm run test:smoke
```

### 4. Notify Team

- Post deployment message
- Document changes
- Update status page

## Troubleshooting

### 503 Service Unavailable

- Check database connection
- Verify environment variables
- Check server logs
- Restart service

### High Latency

- Check database queries
- Check network connectivity
- Monitor CPU/Memory
- Check error logs

### Memory Leaks

- Profile application
- Check for circular references
- Monitor heap dumps
- Use memory leak detection tools

## Resources

- [AWS Deployment Guide](https://aws.amazon.com/getting-started/)
- [Vercel Documentation](https://vercel.com/docs)
- [Netlify Documentation](https://docs.netlify.com/)
- [Railway Documentation](https://docs.railway.app/)

---

Last Updated: 2026-06-01
