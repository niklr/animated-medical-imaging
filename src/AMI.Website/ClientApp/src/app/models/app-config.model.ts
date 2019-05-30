export interface IAppConfig {
    isDevelopment: boolean;
    logging: {
        enableConsoleOutput: boolean;
    };
    api: {
        endpoint: string;
    };
}
