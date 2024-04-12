// A utility class PollyUtility that provides a method GetPolicy to get a Polly retry policy. 
// This policy handles transient HTTP errors and HTTP 404 errors by waiting for 2 seconds and then retrying the request. 
//The request will be retried forever until it succeeds.

using System.Net;
using Polly;
using Polly.Extensions.Http;

// Define the namespace for the utility class
namespace SearchService.Utilities
{
    // Define the PollyUtility class
    public class PollyUtility
    {
        // Define a static method to get a Polly retry policy
        public static IAsyncPolicy<HttpResponseMessage> GetPolicy()
        // Use the lambda expression to define the policy
        => HttpPolicyExtensions
            // Handle transient HTTP errors (like timeouts and 500-series server errors)
            .HandleTransientHttpError()
            // Also handle HTTP 404 (Not Found) errors
            .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
            // If an error occurs, wait for 2 seconds and then retry the request
            // Retry forever until the request succeeds
            .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(2));
    }
}