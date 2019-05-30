export interface IAppConfig {
  isDevelopment: boolean;
  version: string;
  logging: {
    enableConsoleOutput: boolean;
  };
  api: {
    endpoint: string;
  };
}
