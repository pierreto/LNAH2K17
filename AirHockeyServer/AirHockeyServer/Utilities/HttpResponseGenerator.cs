using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace AirHockeyServer.Utilities
{
    ///////////////////////////////////////////////////////////////////////////////
    /// @file HttpResponseGenerator.cs
    /// @author Ariane Tourangeau
    /// @date 2017-10-02
    /// @version 0.1
    ///
    /// Cette classe sert d'utilitaire pour créer des httpResponseMessage
    ///////////////////////////////////////////////////////////////////////////////
    public class HttpResponseGenerator
    {
        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn static HttpResponseMessage CreateSuccesResponseMessage(HttpStatusCode statusCode, object content  = null)
        ///
        /// Cette fonction crée une httpresponseMessage à partir d'un status code et 
        /// d'un contenu au besoin. Le contenu est sérialiser
        /// 
        /// @return la réponse http
        ///
        ////////////////////////////////////////////////////////////////////////
        public static HttpResponseMessage CreateSuccesResponseMessage(HttpStatusCode statusCode, object content  = null)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(statusCode);

            if(content != null)
            {
                responseMessage.Content = new StringContent(JsonConvert.SerializeObject(content));
            }

            return responseMessage;
        }

        ////////////////////////////////////////////////////////////////////////
        ///
        /// @fn static HttpResponseMessage CreateErrorResponseMessage(HttpStatusCode statusCode, object content = null)
        ///
        /// Cette fonction crée une httpresponseMessage à partir d'un status code dans le case
        /// qu'une erreur est survenue.
        /// 
        /// @return la réponse http
        ///
        ////////////////////////////////////////////////////////////////////////
        public static HttpResponseMessage CreateErrorResponseMessage(HttpStatusCode statusCode, string errorMessage = "")
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(statusCode);

            if (errorMessage != string.Empty)
            {
                responseMessage.Content = new StringContent(errorMessage);
            }

            return responseMessage;
        }
    }
}