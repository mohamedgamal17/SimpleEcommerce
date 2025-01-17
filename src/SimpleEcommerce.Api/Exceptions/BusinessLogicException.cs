namespace SimpleEcommerce.Api.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public string Error { get;  } 
        public BusinessLogicException(string error)  : base(error)
        {
            Error = error;
        }
   }
}
