---
name: Web Application Development
description: Best practices and patterns for building full-stack web applications with React/Vue/Svelte and Express/NestJS
keywords:
  - web-development
  - react
  - vue
  - frontend
  - express
  - nestjs
  - rest-api
  - typescript
  - state-management
  - authentication
topics:
  - Frontend Architecture
  - Backend Services
  - API Integration
  - State Management
  - Authentication & Authorization
  - Full-Stack Patterns
applyTo:
  - /web/**
relatedPrompts:
  - 01-feature-implementation.md
  - 04-api-design.md
  - 05-testing-strategy.md
relatedSkills:
  - api-integration.md
  - error-handling.md
version: 1.0
---

# Web Application Development

Best practices for building professional full-stack web applications with frontend and backend components.

## Project Structure

### Frontend (React/Vue)

```typescript
src/
├── components/          # Reusable UI components
│   ├── common/         # Buttons, modals, etc.
│   ├── features/       # Feature-specific components
│   └── layouts/        # Layout components
├── pages/              # Route pages
├── hooks/              # Custom React hooks
├── services/           # API clients
├── store/              # State management (Zustand/Redux)
├── types/              # TypeScript interfaces
├── utils/              # Helper functions
├── styles/             # Global and component styles
└── index.tsx
```

### Backend (Express/NestJS)

```typescript
src/
├── api/
│   ├── routes/         # Express routes or NestJS controllers
│   ├── middleware/     # Express middleware
│   └── validators/     # Request validation
├── services/           # Business logic
├── models/             # Data models/entities
├── repositories/       # Data access layer
├── database/           # DB configuration
├── config/             # App configuration
├── types/              # Shared types
└── index.ts
```

## Frontend Best Practices

### 1. Component Organization

```typescript
// Good: Clear responsibility
interface IUserCardProps {
  userId: string;
  onEdit?: (id: string) => void;
}

export const UserCard: React.FC<IUserCardProps> = ({ userId, onEdit }) => {
  const { data: user, isLoading } = useUser(userId);

  if (isLoading) return <Skeleton />;
  if (!user) return <NotFound />;

  return (
    <Card>
      <Header>{user.name}</Header>
      <Body>{user.email}</Body>
      {onEdit && <Button onClick={() => onEdit(userId)}>Edit</Button>}
    </Card>
  );
};
```

### 2. State Management

```typescript
// Zustand pattern
import create from 'zustand';

interface IUserStore {
  users: IUser[];
  isLoading: boolean;
  fetchUsers: () => Promise<void>;
  addUser: (user: IUser) => void;
  removeUser: (id: string) => void;
}

export const useUserStore = create<IUserStore>((set) => ({
  users: [],
  isLoading: false,

  fetchUsers: async () => {
    set({ isLoading: true });
    try {
      const users = await userService.fetchAll();
      set({ users, isLoading: false });
    } catch (error) {
      set({ isLoading: false });
      throw error;
    }
  },

  addUser: (user) =>
    set((state) => ({
      users: [...state.users, user],
    })),

  removeUser: (id) =>
    set((state) => ({
      users: state.users.filter((u) => u.id !== id),
    })),
}));
```

### 3. Custom Hooks

```typescript
// Reusable logic extraction
export function useAsync<T>(asyncFunction: () => Promise<T>, immediate = true) {
  const [status, setStatus] = useState<'idle' | 'pending' | 'success' | 'error'>('idle');
  const [data, setData] = useState<T | null>(null);
  const [error, setError] = useState<Error | null>(null);

  const execute = useCallback(async () => {
    setStatus('pending');
    try {
      const response = await asyncFunction();
      setData(response);
      setStatus('success');
    } catch (err) {
      setError(err as Error);
      setStatus('error');
    }
  }, [asyncFunction]);

  useEffect(() => {
    if (immediate) execute();
  }, [execute, immediate]);

  return { status, data, error, execute };
}
```

### 4. API Integration

```typescript
// Centralized API client
class ApiClient {
  private baseURL: string;
  private token: string | null = null;

  constructor(baseURL: string) {
    this.baseURL = baseURL;
  }

  setToken(token: string): void {
    this.token = token;
  }

  private getHeaders(): HeadersInit {
    return {
      'Content-Type': 'application/json',
      ...(this.token && { Authorization: `Bearer ${this.token}` }),
    };
  }

  async get<T>(path: string): Promise<T> {
    const response = await fetch(`${this.baseURL}${path}`, {
      method: 'GET',
      headers: this.getHeaders(),
    });
    return this.handleResponse<T>(response);
  }

  async post<T>(path: string, body: unknown): Promise<T> {
    const response = await fetch(`${this.baseURL}${path}`, {
      method: 'POST',
      headers: this.getHeaders(),
      body: JSON.stringify(body),
    });
    return this.handleResponse<T>(response);
  }

  private async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      throw new HttpError(response.status, response.statusText);
    }
    return response.json() as Promise<T>;
  }
}

export const apiClient = new ApiClient(import.meta.env.VITE_API_URL);
```

## Backend Best Practices

### 1. Service Layer Pattern

```typescript
// Separate concerns: HTTP from business logic
export class UserService {
  constructor(private userRepository: IUserRepository) {}

  async getUserById(id: string): Promise<IUser> {
    const user = await this.userRepository.findById(id);
    if (!user) throw new NotFoundError('User', id);
    return user;
  }

  async createUser(input: IUserCreateInput): Promise<IUser> {
    // Validation
    if (!isValidEmail(input.email)) {
      throw new ValidationError('email', 'Invalid email format');
    }

    // Check uniqueness
    const existing = await this.userRepository.findByEmail(input.email);
    if (existing) {
      throw new ConflictError('User with this email already exists');
    }

    // Hash password
    const hashedPassword = await hashPassword(input.password);

    // Create and persist
    return this.userRepository.create({
      ...input,
      password: hashedPassword,
    });
  }
}
```

### 2. Express Route Handler

```typescript
// Express controller pattern
export class UserController {
  constructor(private userService: UserService) {}

  async getUser(req: Request, res: Response, next: NextFunction): Promise<void> {
    try {
      const { id } = req.params;
      const user = await this.userService.getUserById(id);
      res.json(user);
    } catch (error) {
      next(error);
    }
  }

  async createUser(req: Request, res: Response, next: NextFunction): Promise<void> {
    try {
      const user = await this.userService.createUser(req.body);
      res.status(201).json(user);
    } catch (error) {
      next(error);
    }
  }
}

// Route setup
router.get('/users/:id', (req, res, next) => controller.getUser(req, res, next));
router.post('/users', (req, res, next) => controller.createUser(req, res, next));
```

### 3. Middleware Pipeline

```typescript
// Error handling middleware
app.use((error: Error, req: Request, res: Response, next: NextFunction) => {
  if (error instanceof ValidationError) {
    return res.status(400).json({
      error: { code: 'VALIDATION_ERROR', message: error.message },
    });
  }

  if (error instanceof NotFoundError) {
    return res.status(404).json({
      error: { code: 'NOT_FOUND', message: error.message },
    });
  }

  logger.error('Unexpected error:', error);
  res.status(500).json({
    error: { code: 'INTERNAL_ERROR', message: 'Internal server error' },
  });
});

// Authentication middleware
export function authMiddleware(req: Request, res: Response, next: NextFunction) {
  const token = req.headers.authorization?.split(' ')[1];
  if (!token) return res.status(401).json({ error: 'No token' });

  try {
    const decoded = jwt.verify(token, process.env.JWT_SECRET!);
    (req as any).user = decoded;
    next();
  } catch {
    res.status(401).json({ error: 'Invalid token' });
  }
}
```

## Frontend-Backend Communication

### 1. Shared Types

```typescript
// shared/types/user.ts
export interface IUser {
  id: string;
  email: string;
  name: string;
  role: 'admin' | 'user';
  createdAt: Date;
}

export interface IUserCreateInput {
  email: string;
  name: string;
  password: string;
}

// Backend can import and validate
// Frontend knows exact shape of API responses
```

### 2. API Error Handling

```typescript
// Frontend
async function fetchUser(id: string) {
  try {
    return await apiClient.get<IUser>(`/users/${id}`);
  } catch (error) {
    if (error instanceof HttpError) {
      if (error.status === 404) {
        showNotification('User not found');
      } else if (error.status === 401) {
        redirectToLogin();
      }
    }
    throw error;
  }
}
```

## Authentication Flow

```typescript
// Backend: Generate token
app.post('/auth/login', async (req, res) => {
  const { email, password } = req.body;
  const user = await userService.authenticate(email, password);
  const token = generateToken({ userId: user.id, role: user.role });
  res.json({ token, user });
});

// Frontend: Store and use token
function useAuth() {
  const [token, setToken] = useState<string | null>(localStorage.getItem('auth_token'));

  const login = async (email: string, password: string) => {
    const { token, user } = await apiClient.post('/auth/login', {
      email,
      password,
    });
    localStorage.setItem('auth_token', token);
    apiClient.setToken(token);
    setToken(token);
    return user;
  };

  return { token, login };
}
```

## Testing Strategy

### Frontend Testing

```typescript
// Component test
describe('UserCard', () => {
  it('should render user data', () => {
    const { getByText } = render(
      <UserCard userId="123" />
    );
    expect(getByText('John Doe')).toBeInTheDocument();
  });

  it('should call onEdit when button clicked', () => {
    const handleEdit = jest.fn();
    const { getByText } = render(
      <UserCard userId="123" onEdit={handleEdit} />
    );
    fireEvent.click(getByText('Edit'));
    expect(handleEdit).toHaveBeenCalledWith('123');
  });
});

// Hook test
describe('useUser', () => {
  it('should fetch user on mount', async () => {
    const { result, waitForNextUpdate } = renderHook(() => useUser('123'));
    await waitForNextUpdate();
    expect(result.current.data).toEqual({ id: '123', name: 'John' });
  });
});
```

### Backend Testing

```typescript
// Integration test
describe('UserService', () => {
  it('should create user with hashed password', async () => {
    const user = await userService.createUser({
      email: 'test@example.com',
      name: 'Test',
      password: 'password123',
    });

    expect(user.id).toBeDefined();
    expect(user.email).toBe('test@example.com');
  });

  it('should throw ValidationError for invalid email', async () => {
    await expect(
      userService.createUser({
        email: 'invalid',
        name: 'Test',
        password: 'password123',
      })
    ).rejects.toThrow(ValidationError);
  });
});
```

## Common Patterns

### Form Handling

```typescript
// React Hook Form pattern
function UserForm() {
  const { register, handleSubmit, formState: { errors } } = useForm({
    resolver: zodResolver(userSchema)
  });

  const onSubmit = async (data: IUserCreateInput) => {
    try {
      await userService.create(data);
      showSuccess('User created');
    } catch (error) {
      showError(error.message);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input {...register('email')} />
      {errors.email && <span>{errors.email.message}</span>}
      <button type="submit">Create</button>
    </form>
  );
}
```

### Data Fetching

```typescript
// SWR or React Query pattern
function UserList() {
  const { data: users, isLoading, error } = useQuery(
    'users',
    () => userService.fetchAll()
  );

  if (isLoading) return <Loading />;
  if (error) return <Error message={error.message} />;

  return (
    <ul>
      {users?.map(user => (
        <li key={user.id}>{user.name}</li>
      ))}
    </ul>
  );
}
```

## Performance Optimization

### Code Splitting

```typescript
// Lazy load heavy components
const UserManagement = lazy(() => import('./pages/UserManagement'));

function App() {
  return (
    <Suspense fallback={<Loading />}>
      <Routes>
        <Route path="/users/*" element={<UserManagement />} />
      </Routes>
    </Suspense>
  );
}
```

### Memoization

```typescript
// Prevent unnecessary re-renders
export const UserCard = memo(function UserCard({ user, onEdit }: Props) {
  return (
    <div>
      <h3>{user.name}</h3>
      <button onClick={() => onEdit(user.id)}>Edit</button>
    </div>
  );
}, (prev, next) => prev.user.id === next.user.id);
```

## Key Takeaways

✅ **Separation of Concerns**: Services for business logic, controllers for HTTP
✅ **Reusable Components**: Small, focused components with clear props
✅ **Type Safety**: Use shared types between frontend and backend
✅ **Error Handling**: Consistent error handling across layers
✅ **Testing**: Test business logic independently
✅ **Performance**: Lazy load, memoize, and optimize queries
✅ **Security**: Validate on both frontend and backend

---

**Related Resources**:

- Prompts: [Feature Implementation](../prompts/01-feature-implementation.md), [API Design](../prompts/04-api-design.md)
- Skills: [Error Handling](error-handling.md), [API Integration](api-integration.md)
- Standards: See [copilot-instructions.md](../copilot-instructions.md)
