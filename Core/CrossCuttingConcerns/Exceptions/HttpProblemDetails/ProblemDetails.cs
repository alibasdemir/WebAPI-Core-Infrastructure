namespace Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails
{
    public class ProblemDetails
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Type { get; set; }
        public int? Status { get; set; }
    }
}
