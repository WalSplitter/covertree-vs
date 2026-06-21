import js from '@eslint/js';
import tseslint from 'typescript-eslint';
import prettierConfig from 'eslint-config-prettier';
import importPlugin from 'eslint-plugin-import';
import globals from 'globals';

export default tseslint.config(
  {
    files: ['**/*.{ts,tsx,js,jsx}'],
  },

  js.configs.recommended,
  ...tseslint.configs.recommended,
  prettierConfig,

  // Browser globals for frontend files
  {
    files: ['apps/web-frontend/**/*.{ts,tsx,js,jsx}'],
    languageOptions: {
      globals: {
        ...globals.browser,
      },
    },
  },

  // Node globals for backend/tools files
  {
    files: ['apps/web-backend/**/*.{ts,js}', 'apps/api/**/*.{ts,js}', 'tools/**/*.{ts,js}'],
    languageOptions: {
      globals: {
        ...globals.node,
      },
    },
  },

  {
    plugins: {
      import: importPlugin,
    },

    rules: {
      'no-console': ['warn', { allow: ['warn', 'error'] }],
      'no-debugger': 'warn',
      'prefer-const': 'error',
      'no-var': 'error',
      eqeqeq: ['error', 'always'],
      curly: ['error', 'all'],
      '@typescript-eslint/no-unused-vars': [
        'error',
        {
          argsIgnorePattern: '^_',
          varsIgnorePattern: '^_',
        },
      ],
    },
  },

  {
    ignores: ['**/node_modules/**', '**/dist/**', '**/build/**', '**/coverage/**'],
  }
);
