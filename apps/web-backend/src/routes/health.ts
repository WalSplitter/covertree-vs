import { Router } from 'express';

export const healthRouter = Router();

healthRouter.get('/', (_req, res): void => {
  res.json({
    status: 'ok',
    timestamp: new Date().toISOString(),
  });
});
