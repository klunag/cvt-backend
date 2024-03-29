﻿using BCP.CVT.Services.Interface;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BCP.CVT.WebApi.Auth
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            if (actionContext.Request.Headers.Authorization != null)
            {
                var authToken = actionContext.Request.Headers.Authorization.Parameter;

                var decodeauthToken = System.Text.Encoding.UTF8.GetString(
                    Convert.FromBase64String(authToken));

                var arrUserNameandPassword = decodeauthToken.Split(':');

                if (IsAuthorizedUser(arrUserNameandPassword[0], arrUserNameandPassword[1]))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(arrUserNameandPassword[0]), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }

        public static bool IsAuthorizedUser(string codAplicacion, string apikey)
        {
            var credenciales = ServiceManager<AuthDAO>.Provider.ObtenerApiKey(codAplicacion);
            if (credenciales != null)
                return apikey == credenciales.ApiKey;
            else
                return false;
        }

        public static string ObtenerAuthApiKey(string codAplicacion)
        {
            var dataRpta = ServiceManager<AuthDAO>.Provider.ObtenerApiKey(codAplicacion);
            if (dataRpta != null)
            {
                return dataRpta.ApiKey;
            }
            return "";
        }
    }
}