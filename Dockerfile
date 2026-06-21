# Build stage
FROM node:20-alpine AS builder

WORKDIR /app

# Copy package files
COPY package*.json ./
COPY web/package.json web/

# Install dependencies
RUN npm ci

# Copy source code
COPY web/backend backend/

# Build
RUN npm run build -w backend

# Production stage
FROM node:20-alpine

WORKDIR /app

ENV NODE_ENV=production

# Copy package files
COPY package*.json ./
COPY web/package.json web/

# Install production dependencies only
RUN npm ci --production

# Copy built application
COPY --from=builder /app/web/backend/dist web/backend/dist

EXPOSE 3000

CMD ["node", "web/backend/dist/index.js"]
