using BCP.CVT.Cross;
using BCP.CVT.DTO.ITManagement;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace BCP.CVT.WebApi.Filters
{
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //Validacion de parametros
            //if (actionContext.ActionDescriptor is ReflectedHttpActionDescriptor descriptor)
            //{
            //    var parameters = descriptor.MethodInfo.GetParameters();

            //    foreach (var parameter in parameters)
            //    {
            //        var argument = actionContext.ActionArguments.ContainsKey(parameter.Name) ?
            //            actionContext.ActionArguments[parameter.Name] : null;

            //        EvaluateValidationAttributes(parameter, argument, actionContext.ModelState);
            //    }
            //}

            if (!actionContext.ModelState.IsValid)
            {
                var errors = GetModelStateErrors(actionContext.ModelState);

                var apiResp = new APIResponse<string>(EAPIResponseCode.TL0003.ToString(), 
                    Utilitarios.GetEnumDescription2(EAPIResponseCode.TL0003), 
                    errors);

                var response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, apiResp);
                actionContext.Response = response;
            }
        }

        private static List<string> GetModelStateErrors(ModelStateDictionary ModelState)
        {
            var errors = new List<string>();
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return errors;
        }

        private void EvaluateValidationAttributes(ParameterInfo parameter, object argument, ModelStateDictionary modelState)
        {
            var validationAttributes = parameter.CustomAttributes;

            foreach (var attributeData in validationAttributes)
            {
                var attributeInstance = CustomAttributeExtensions.GetCustomAttribute(parameter, attributeData.AttributeType);

                //var validationAttribute = attributeInstance as FromUriAttribute;

                if (attributeInstance is ValidationAttribute validationAttribute)
                {
                    var isValid = validationAttribute.IsValid(argument);
                    if (!isValid)
                    {
                        modelState.AddModelError(parameter.Name, validationAttribute.FormatErrorMessage(parameter.Name));
                    }
                }
            }
        }

    }
}