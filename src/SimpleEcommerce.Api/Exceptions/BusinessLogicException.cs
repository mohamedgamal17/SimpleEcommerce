namespace SimpleEcommerce.Api.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public BusinessLogicException(string error) 
        {
            Errors = new List<string>
            {
                error
            };
        }

        public BusinessLogicException(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }
}
