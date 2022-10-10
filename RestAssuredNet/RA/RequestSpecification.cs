﻿// <copyright file="RequestSpecification.cs" company="On Test Automation">
// Copyright 2019 the original author or authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
using System.Text;
using RestAssuredNet.RA.Internal;

namespace RestAssuredNet.RA
{
    /// <summary>
    /// The request to be sent.
    /// </summary>
    public class RequestSpecification : IDisposable
    {
        private HttpRequestMessage request = new HttpRequestMessage();
        private string requestBody = string.Empty;
        private string contentTypeHeader = "application/json";
        private Encoding contentEncoding = Encoding.UTF8;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestSpecification"/> class.
        /// </summary>
        public RequestSpecification()
        {
        }

        /// <summary>
        /// Adds a request header and the associated value to the request object to be sent.
        /// </summary>
        /// <param name="key">The header key that is to be added to the request.</param>
        /// <param name="value">The associated header value that is to be added to the request.</param>
        /// <returns>The current <see cref="RequestSpecification"/>.</returns>
        public RequestSpecification Header(string key, object value)
        {
            this.request.Headers.Add(key, value.ToString());
            return this;
        }

        /// <summary>
        /// Add a request header and the associated values to the request object to be sent.
        /// </summary>
        /// <param name="key">The header key that is to be added to the request.</param>
        /// <param name="values">The associated header values that are to be added to the request.</param>
        /// <returns>The current <see cref="RequestSpecification"/>.</returns>
        public RequestSpecification Header(string key, IEnumerable<string> values)
        {
            this.request.Headers.Add(key, values);
            return this;
        }

        /// <summary>
        /// Add a Content-Type header and the specified value to the request object to be sent.
        /// </summary>
        /// <param name="contentType">The value for the Content-Type header to be added.</param>
        /// <returns>The current <see cref="RequestSpecification"/> object.</returns>
        public RequestSpecification ContentType(string contentType)
        {
            this.contentTypeHeader = contentType;
            return this;
        }

        /// <summary>
        /// Adds a request body to the request object to be sent.
        /// </summary>
        /// <param name="body">The body that is to be sent with the request as a string.</param>
        /// <returns>The current <see cref="RequestSpecification"/>.</returns>
        public RequestSpecification Body(string body)
        {
            this.requestBody = body;
            return this;
        }

        /// <summary>
        /// Syntactic sugar (for now) to help indicate the start of the 'Act' part of a test.
        /// </summary>
        /// <returns>The current <see cref="RequestSpecification"/>.</returns>
        public RequestSpecification When()
        {
            return this;
        }

        /// <summary>
        /// Performs an HTTP GET.
        /// </summary>
        /// <param name="endpoint">The endpoint to invoke in the HTTP GET request.</param>
        /// <returns>The HTTP response object.</returns>
        public Response Get(string endpoint)
        {
            return this.Send(HttpMethod.Get, endpoint);
        }

        /// <summary>
        /// Performs an HTTP POST.
        /// </summary>
        /// <param name="endpoint">The endpoint to invoke in the HTTP POST request.</param>
        /// <returns>The HTTP response object.</returns>
        public Response Post(string endpoint)
        {
            return this.Send(HttpMethod.Post, endpoint);
        }

        /// <summary>
        /// Performs an HTTP PUT.
        /// </summary>
        /// <param name="endpoint">The endpoint to invoke in the HTTP PUT request.</param>
        /// <returns>The HTTP response object.</returns>
        public Response Put(string endpoint)
        {
            return this.Send(HttpMethod.Put, endpoint);
        }

        /// <summary>
        /// Performs an HTTP PATCH.
        /// </summary>
        /// <param name="endpoint">The endpoint to invoke in the HTTP PATCH request.</param>
        /// <returns>The HTTP response object.</returns>
        public Response Patch(string endpoint)
        {
            return this.Send(HttpMethod.Patch, endpoint);
        }

        /// <summary>
        /// Performs an HTTP DELETE.
        /// </summary>
        /// <param name="endpoint">The endpoint to invoke in the HTTP DELETE request.</param>
        /// <returns>The HTTP response object.</returns>
        public Response Delete(string endpoint)
        {
            return this.Send(HttpMethod.Delete, endpoint);
        }

        /// <summary>
        /// Implements Dispose() method of IDisposable interface.
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            this.Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implements Dispose(bool) method of IDisposable interface.
        /// </summary>
        /// <param name="disposing">Flag indicating whether objects should be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.request.Dispose();
            this.disposed = true;
        }

        /// <summary>
        /// Sends the request object to the <see cref="HttpRequestProcessor"/>.
        /// </summary>
        /// <param name="httpMethod">The HTTP method to use in the request.</param>
        /// <param name="endpoint">The endpoint to be used in the request.</param>
        /// <returns>An object representing the HTTP response corresponding to the request.</returns>
        private Response Send(HttpMethod httpMethod, string endpoint)
        {
            this.request.Method = httpMethod;
            this.request.RequestUri = new Uri(endpoint);
            this.request.Content = new StringContent(this.requestBody, this.contentEncoding, this.contentTypeHeader);

            Task<Response> task = HttpRequestProcessor.Send(this.request);
            return task.Result;
        }
    }
}
