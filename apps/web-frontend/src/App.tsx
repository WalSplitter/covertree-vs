import './styles/App.css';

function App(): JSX.Element {
  return (
    <div className="app">
      <header className="app-header">
        <h1>Welcome to Base Project Template</h1>
        <p>A professional, production-ready template for modern applications</p>
      </header>
      <main className="app-main">
        <section>
          <h2>✨ Key Features</h2>
          <ul>
            <li>Full-stack web application structure</li>
            <li>TypeScript configuration</li>
            <li>ESLint and Prettier setup</li>
            <li>GitHub Actions CI/CD</li>
            <li>Testing framework configured</li>
            <li>Professional development guidelines</li>
          </ul>
        </section>
      </main>
    </div>
  );
}

export default App;
