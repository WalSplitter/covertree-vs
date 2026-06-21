# Prompt: Feature Implementation - Base Project Template

Use this prompt template when implementing new features with GitHub Copilot.

## 📋 Before You Start

Review:

- [Code Standards](./.github/copilot-instructions.md#code-standards--quality) - Naming conventions, quality requirements
- [Monorepo Structure](./docs/adr/001-monorepo-structure.md) - How workspaces are organized
- Existing code in your workspace for patterns

## Template

```
Context:
I'm building a [PROJECT_TYPE] feature in the [WORKSPACE] workspace.
Monorepo structure: npm Workspaces (shared types, independent services)
Existing patterns: [REFERENCE_EXISTING_CODE] - See shared/types/ and similar implementations

Requirements:
- Feature: [FEATURE_NAME]
- Functionality: [WHAT_IT_SHOULD_DO]
- Input: [INPUT_SPECIFICATION]
- Output: [OUTPUT_SPECIFICATION]
- Success criteria: [SUCCESS_CRITERIA]

Constraints:
- Technology stack: [TECH_STACK] (See README Technology Stack section)
- Must follow: Naming (camelCase/PascalCase/UPPER_SNAKE_CASE), SOLID principles, strict TypeScript
- Use existing types: From shared/types/ for common interfaces
- Error handling: Comprehensive try-catch, custom error classes, logging
- Testing: Unit tests required, 80%+ coverage target
- Documentation: JSDoc with @param, @returns, @throws

Deliverables:
1. Implementation file in correct workspace with error handling
2. Unit tests (vitest) covering happy path and edge cases
3. JSDoc documentation with examples
4. Integration with existing services in workspace
5. Commit message using Conventional Commits format (feat/fix/docs)
```

## Example Usage

```
Context:
I'm building a user authentication feature in the apps/web-backend workspace.
Monorepo structure: npm Workspaces with shared types in shared/types/
Existing patterns: Express middleware, error handlers in src/middleware/, services in src/services/

Requirements:
- Feature: Create user registration service
- Functionality: Accept email/password, validate input, hash password, save to database
- Input: { email: string, password: string, name: string }
- Output: { userId: string, createdAt: Date }
- Success criteria: User saved with hashed password, validation errors caught

Constraints:
- Technology stack: TypeScript 5.0+, Express, PostgreSQL
- Must follow: camelCase (vars), PascalCase (types), Helmet security, CORS, input validation with Joi
- Use existing types: from shared/types/src/index.ts (IUser, IAuthPayload, IApiResponse)
- Error handling: ValidationError for invalid input, use Pino logger, structured error responses

Deliverables:
1. UserService class with registerUser() method
2. Unit tests covering valid registration and validation errors
3. JSDoc with @param, @returns, @throws
4. Integration with existing UserRepository
```

## 🎯 Tips for Better Results

1. **Specify the Workspace**: Indicate which /web, /apps/_, /desktop/_, /tools/_, or /shared/_ workspace
2. **Reference Shared Types**: Point to shared/types/src/ for common interfaces
3. **Follow Code Standards**: Use naming conventions from .github/copilot-instructions.md
4. **Provide Existing Examples**: Show similar implementations in the same workspace
5. **Include Testing**: Always ask for unit tests (vitest) with 80%+ coverage target
6. **Set Constraints**: Mention security (Helmet, CORS), validation (Joi), logging (Pino)

## Refinement Prompts

After initial implementation, use these refinement prompts:

- "Add comprehensive error handling for [SCENARIO]"
- "Optimize the performance by [STRATEGY]"
- "Add unit tests covering [EDGE_CASES]"
- "Refactor to follow [PATTERN] design pattern"
- "Add TypeScript strict mode compatibility"
