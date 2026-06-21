import cors from 'cors';
import helmet from 'helmet';
import express, { Express } from 'express';

import { logger } from './utils/logger';
import { errorHandler } from './middleware/error-handler';
import { notFound } from './middleware/not-found';
import { healthRouter } from './routes/health';

const app: Express = express();
const port = process.env.PORT ?? 3000;

// Middleware
app.use(helmet());
app.use(cors());
app.use(express.json());

// Routes
app.use('/api/health', healthRouter);

// 404 handler
app.use(notFound);

// Error handler (must be last)
app.use(errorHandler);

// Start server
app.listen(port, (): void => {
  logger.info(`Server running on port ${port}`);
});

export default app;
