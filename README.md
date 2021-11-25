# ApiRateLimiter

A simple rate limiter for Asp.Net Core APIs projects.
#

**Usage:**

Register `AddApiRateLimiter` in `ConfigureServices`.
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddApiRateLimiter(Configuration);
    services.AddInMemoryRateLimiter();
}
```
And adding `UseRateLimiter` middleware.
```
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseRateLimiter();
}
```
`appsettings.json` file:
```json
"ApiRateLimiterOption": {
  "ApiUrls": {
    "/api/values": {
      "limit": 2,
      "duration": 5
    },
    "/api/values/{evryThing}": {
      "limit": 5,
      "duration": 10
    }
  },
  "WhiteList": [
      "127.0.0.1"
  ]
}
```
