## AMI.API

### Docker build/run

In a command prompt, make sure you are in the root directory containing the solution file AMI.sln

* docker build --tag=ami-api:v0.0.0 -f AMI.API\Dockerfile .
* docker run --name ami-api -e "ApiOptions:AllowedCorsOrigins=http://localhost:23600" -e "AppOptions:WorkingDirectory=/tmp/AMI.API" -p 23000:80 ami-api:v0.0.0

Now the API should be accessible on http://localhost:23000/ in your browser.