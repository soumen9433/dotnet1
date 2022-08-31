namespace HazGo.BuildingBlocks.Core.Common
{
    public class ValidationError
    {
        public ValidationError(string field, string message)
        {
            Field = field;
            ErrorMessage = message;
        }

        public string Field { get; }

        public string ErrorMessage { get; }
    }
}
