import { NextFunction, Request, Response } from 'express';

import { logger } from '../utils/logger';

export const errorHandler = (
  error: Error,
  _req: Request,
  res: Response,
  _next: NextFunction
): void => {
  logger.error(error);

  res.status(500).json({
    error: 'Internal Server Error',
    message: process.env.NODE_ENV === 'production' ? 'An error occurred' : error.message,
  });
};
