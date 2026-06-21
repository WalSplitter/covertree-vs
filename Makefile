.PHONY: help install dev build test lint format clean docker-up docker-down

help:
	@echo "Available commands:"
	@echo "  make install        Install dependencies"
	@echo "  make dev           Start development servers"
	@echo "  make build         Build all projects"
	@echo "  make test          Run all tests"
	@echo "  make test-watch    Run tests in watch mode"
	@echo "  make lint          Lint all code"
	@echo "  make lint-fix      Fix lint issues"
	@echo "  make format        Format code with Prettier"
	@echo "  make format-check  Check formatting"
	@echo "  make type-check    Type check with TypeScript"
	@echo "  make clean         Clean all artifacts"
	@echo "  make docker-up     Start Docker containers"
	@echo "  make docker-down   Stop Docker containers"
	@echo "  make docker-logs   Show Docker logs"

install:
	npm install

dev:
	npm run dev

build:
	npm run build

test:
	npm run test

test-watch:
	npm run test:watch

test-coverage:
	npm run test:coverage

lint:
	npm run lint

lint-fix:
	npm run lint:fix

format:
	npm run format

format-check:
	npm run format:check

type-check:
	npm run type-check

clean:
	npm run clean
	rm -rf node_modules package-lock.json

docker-build:
	docker build -t base-template:latest .

docker-up:
	docker-compose up -d

docker-down:
	docker-compose down

docker-logs:
	docker-compose logs -f

docker-clean:
	docker-compose down -v
	docker rmi base-template:latest

setup: install
	cp .env.example .env
	docker-compose up -d
	npm run dev

reset: docker-clean clean install
	cp .env.example .env
	docker-compose up -d
	npm run dev
