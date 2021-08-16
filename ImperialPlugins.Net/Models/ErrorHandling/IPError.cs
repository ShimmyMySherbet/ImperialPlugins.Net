using System.Collections.Generic;

namespace ImperialPlugins.Models.ErrorHandling
{
    public class IPError
    {
        public string Code;
        public string Message;
        public string Details;

        public List<ValidationError> ValidationErrors;
    }
}