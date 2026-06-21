import { jsx as _jsx, jsxs as _jsxs } from 'react/jsx-runtime';
import './styles/App.css';
function App() {
  return _jsxs('div', {
    className: 'app',
    children: [
      _jsxs('header', {
        className: 'app-header',
        children: [
          _jsx('h1', { children: 'Welcome to Base Project Template' }),
          _jsx('p', {
            children: 'A professional, production-ready template for modern applications',
          }),
        ],
      }),
      _jsx('main', {
        className: 'app-main',
        children: _jsxs('section', {
          children: [
            _jsx('h2', { children: '\u2728 Key Features' }),
            _jsxs('ul', {
              children: [
                _jsx('li', { children: 'Full-stack web application structure' }),
                _jsx('li', { children: 'TypeScript configuration' }),
                _jsx('li', { children: 'ESLint and Prettier setup' }),
                _jsx('li', { children: 'GitHub Actions CI/CD' }),
                _jsx('li', { children: 'Testing framework configured' }),
                _jsx('li', { children: 'Professional development guidelines' }),
              ],
            }),
          ],
        }),
      }),
    ],
  });
}
export default App;
//# sourceMappingURL=App.js.map
