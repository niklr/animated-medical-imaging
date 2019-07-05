## AMI.Website

### Docker build/run

In a command prompt, make sure you are in the root directory containing the solution file AMI.sln

* docker build --tag=ami-website:v0.0.0 -f AMI.Website\Dockerfile .
* docker run --name ami-website -e "ClientOptions:ApiEndpoint=http://localhost:23000" -p 23600:80 ami-website:v0.0.0

Now the website should be accessible on http://localhost:23600/ in your browser.

### Initialization

* dotnet new angular -o AMI.Website

https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/angular?view=aspnetcore-2.2&tabs=visual-studio

### Install npm packages

To install third-party npm packages, use a command prompt in the ClientApp subdirectory. For example:

* cd ClientApp
* npm install --save <package_name>

### Publish and deploy

In development, the app runs in a mode optimized for developer convenience. 
For example, JavaScript bundles include source maps (so that when debugging, you can see your original TypeScript code). 
The app watches for TypeScript, HTML, and CSS file changes on disk and automatically recompiles and reloads when it sees those files change.

In production, serve a version of your app that's optimized for performance. 
This is configured to happen automatically. 
When you publish, the build configuration emits a minified, ahead-of-time (AoT) compiled build of your client-side code. 
Unlike the development build, the production build doesn't require Node.js to be installed on the server (unless you have enabled server-side rendering (SSR)).

You can use standard [ASP.NET Core hosting and deployment methods](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/index?view=aspnetcore-2.2).

### Run "ng serve" independently

The project is configured to start its own instance of the Angular CLI server in the background when the ASP.NET Core app starts in development mode. 
This is convenient because you don't have to run a separate server manually.

There's a drawback to this default setup. Each time you modify your C# code and your ASP.NET Core app needs to restart, the Angular CLI server restarts. 
Around 10 seconds is required to start back up. 
If you're making frequent C# code edits and don't want to wait for Angular CLI to restart, run the Angular CLI server externally, independently of the ASP.NET Core process. 
To do so:

1. In a command prompt, switch to the ClientApp subdirectory, and launch the Angular CLI development server:
  * cd ClientApp
  * npm start

> **Important**
> 
> Use npm start to launch the Angular CLI development server, not ng serve, so that the configuration in package.json is respected. 
> To pass additional parameters to the Angular CLI server, add them to the relevant scripts line in your package.json file.

2. Modify your ASP.NET Core app to use the external Angular CLI instance instead of launching one of its own. 
In your Startup class, replace the spa.UseAngularCliServer invocation with the following:

C#
```
spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
```

When you start your ASP.NET Core app, it won't launch an Angular CLI server. 
The instance you started manually is used instead. 
This enables it to start and restart faster. 
It's no longer waiting for Angular CLI to rebuild your client app each time.

