﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BCP.CVT.WebApi
{
    public class CorsMessageHandler : DelegatingHandler
    {
        const string Origin = "Origin";
        const string AccessControlRequestMethod = "Access-Control-Request-Method";
        const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";
        const string AccessControlMaxAge = "Access-Control-Max-Age";


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return request.Headers.Contains(Origin) ?
                ProcessCorsRequest(request, ref cancellationToken) :
                base.SendAsync(request, cancellationToken);
        }

        private Task<HttpResponseMessage> ProcessCorsRequest(HttpRequestMessage request, ref CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options)
            {
                return Task.Factory.StartNew<HttpResponseMessage>(() =>
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    AddCorsResponseHeaders(request, response);
                    return response;
                }, cancellationToken);
            }
            else
            {
                return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(task =>
                {
                    HttpResponseMessage resp = null;
                    try
                    {
                        resp = task.Result;
                    }catch(Exception ex)
                    {

                    }
                    if (resp != null)
                    {
                        resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                    }
                    return resp;
                });
            }
        }

        private static void AddCorsResponseHeaders(HttpRequestMessage request, HttpResponseMessage response)
        {
            response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

            string accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
            if (accessControlRequestMethod != null)
            {
                response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
            }

            string requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
            if (!string.IsNullOrEmpty(requestedHeaders))
            {
                response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
            }

            response.Headers.Add(AccessControlMaxAge, "1728000");
        }
    }
}