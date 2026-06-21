import { NextFunction, Request, Response } from 'express';

export const notFound = (_req: Request, res: Response, _next: NextFunction): void => {
  res.status(404).json({
    error: 'Not Found',
    message: 'The requested resource was not found',
  });
};
