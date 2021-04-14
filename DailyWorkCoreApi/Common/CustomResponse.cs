using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting; 

namespace DailyWorkCoreApi.Common
{
    public static class CustomResponse
    {
        public enum ResponseType { XML, JSON }
        public static HttpResponseMessage GetResponseMessage<T>(T rst, HttpStatusCode status, ResponseType type = ResponseType.JSON)
        { 
            try
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage
                {
                    StatusCode = status
                };

                switch (type)
                {
                    case ResponseType.XML:
                        {
                            responseMessage.Content = new ObjectContent<T>(rst, new XmlMediaTypeFormatter());
                            break;
                        }
                    case ResponseType.JSON:
                        {
                            responseMessage.Content = new ObjectContent<T>(rst, new JsonMediaTypeFormatter());
                            break;
                        }
                }

                return responseMessage;

            }
            catch (Exception ex)
            {

                throw ex;

            }
        }

        public static string GetIPAddress(IHttpContextAccessor httpContextAccessor)
        {
            string i_ip = string.Empty;
            i_ip= httpContextAccessor.HttpContext.Connection.LocalIpAddress + ":" + httpContextAccessor.HttpContext.Connection.LocalPort + "[" + httpContextAccessor.HttpContext.Connection.RemoteIpAddress + ":" + httpContextAccessor.HttpContext.Connection.RemotePort+"]";
            //if (!String.IsNullOrEmpty(context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            //{
            //    i_ip = context.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //}
            //else
            //{
            //    i_ip = context.Current.Request.ServerVariables["REMOTE_ADDR"];
            //}

            return i_ip;
        }

    }
}
