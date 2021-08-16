using ImperialPlugins.Models.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Net;

namespace ImperialPlugins.Models.Exceptions
{
    public class ImperialPluginsException : Exception
    {
        private string m_Message;
        public string[] Errors { get; private set; }
        public override string Message => m_Message;

        public string Details { get; private set; }

        public string Code { get; private set; }

        public WebException UnderlyingError { get; private set; }

        public ImperialPluginsException(IPError error, WebException webException)
        {
            m_Message = error.Message;
            UnderlyingError = webException;
            var errors = new List<string>();
            Details = error.Details;
            Code = error.Code;
            if (error.ValidationErrors != null)
            {
                foreach (var validation in error.ValidationErrors)
                {
                    errors.Add(validation.Message);
                }
            }
            Errors = errors.ToArray();
        }
    }
}