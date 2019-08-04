using System.Linq;
using AMI.Core.Entities.Models;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;
using RNS.Framework.Tools;

namespace AMI.API.Extensions.ServiceCollectionExtensions
{
    /// <summary>
    /// Extensions related to <see cref="IServiceCollection"/>
    /// </summary>
    public static class OpenApiExtensions
    {
        /// <summary>
        /// Adds services required for OpenAPI 3.0 generation (change document settings to generate Swagger 2.0).
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="appInfo">The information about the application.</param>
        public static void AddCustomOpenApiDocument(this IServiceCollection services, AppInfo appInfo)
        {
            Ensure.ArgumentNotNull(services, nameof(services));
            Ensure.ArgumentNotNull(appInfo, nameof(appInfo));

            // Customise the Swagger specification
            var openApiSecurityScheme = new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            };
            services.AddOpenApiDocument(document =>
            {
                document.Title = "AMI API";
                document.Version = appInfo.AppVersion;
                document.DocumentName = "default";
                document.AddSecurity("JWT", Enumerable.Empty<string>(), openApiSecurityScheme);
                document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
                document.Description = @"REST service for [Animated Medical Imaging](https://github.com/niklr/animated-medical-imaging) (AMI)

## Introduction
Integrating this Application Programming Interface (API) is the easiest way to submit data to AMI. 
The following documentation will help you to get full advantage of the API by understanding and implementing all endpoints. 
With AMI API you will be able to upload files and submit tasks containing information needed for processing and receive the processed results. 
Requests with a body are sent in JavaScript Object Notation (JSON) format over HTTP(S), using the PUT or POST method.

Please, feel free to ask any questions, report bugs, suggest new features, and more on [GitHub](https://github.com/niklr/animated-medical-imaging/issues)

## Authentication
Authentication of requests is accomplished by using JSON Web Tokens (JWT). Tokens can be obtained by providing your credentials to the tokens API endpoint.
If you don't have a registered account, you still can obtain tokens with the special API endpoint for anonymous users. 
The response of the tokens API endpoint contains 3 different tokens: 

- **Access Token**: An authorization credential that can be used by the application to access the API.
- **ID Token**: Contains user profile information (such as the user's name and email) which is represented in the form of claims.
- **Refresh Token**: Contains the information required to obtain a new Access Token or ID Token.

For each API request you need to include the encoded JWT Access Token with a ""Bearer"" prefix.

## Requests, Responses, and Errors
Status codes are issued by the API in response to a client's request made to the API.

> A **successful** completion of a request returns one of three possible status codes.

Status code | Name | Description
---- | ---- | ----
200 | OK | The default status code for successful requests.
201 | Created | Returned on successful POST requests when one or more new entities have been created.
204 | No Content | Returned on successful DELETE requests.

> An **unsuccessful** completion of a request returns one of the following status codes.

Status code | Name | Description
---- | ---- | ----
400 | Bad Request | The format of the URL and/or of values in the parameter list is not valid.
401 | Unauthorized | Usually caused by using a wrong/expired access token or by not using one at all.
403 | Forbidden | The request was valid, but insufficient permissions for the resource.
404 | Not Found | The requested entity does not (or no longer) exist.
409 | Conflict | Indicates a mismatch in the current state of the resource.
429 | Too Many Requests | Indicates that the rate limit has been reached.
500 | Internal Server Error | An exception occurred that has no adequate handling.

## HTTP method definitions
The Hypertext Transfer Protocol (HTTP) defines a set of request methods to indicate the desired action to be performed for a given resource/endpoint. 
Although they can also be nouns, these request methods are sometimes referred as HTTP verbs.
An idempotent HTTP method can be called many times without different outcomes.
AMI API makes use of the following HTTP methods:

Method | Description
---- | ----
GET | Getting a resource. (e.g. GET *https://localhost/objects/23* without body)
POST | Creating a resource. (e.g. POST *https://localhost/tasks* with a body containing JSON data)
PUT | Updating a resource. (e.g. PUT *https://localhost/tasks/7* with a body containing JSON data)
DELETE | Deleting a resource. (e.g. DELETE *https://localhost/objects/23* without body)

## Rate Limits
Requests are limited on a 60 seconds basis to provide equal access to the API for everyone.
The rate limit information is indicated in the header of the response e.g.

- X-Rate-Limit-Limit: 1m
- X-Rate-Limit-Remaining: 56
- X-Rate-Limit-Reset: 2019-07-25T09:44:13.4658862Z

## Pagination
All endpoints returning a list of entities are paginated by default.
Returning a limited amount of entities is easier to handle, instead of hundreds or thousands.

Option | Description
---- | ---- 
limit | Defines the limit to constrain the number of items. (Allowed values: 10, 25, 50)
page | Defines the current page number. (Allowed values: 0, 1, 2, ...)

## Date format
All dates are in UTC (Universal Time Coordinated) and represented in ISO 8601 format (International Organization for Standardization).

> 2019-04-17T07:52:41.4700000Z

## Webhooks
After we process and complete your request you will be notified via webhook depending on the type of request.
Dedicated resources enable you to specify new webhooks, get missed webhooks and clear missed webhooks.
For testing purposes you can use a service like [hookbin.com](https://hookbin.com/).

";
            });
        }
    }
}
