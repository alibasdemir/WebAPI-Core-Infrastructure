namespace Core.Application.Pipelines.Performance
{
    public interface IIntervalRequest
    {
        /// <summary>
        /// The maximum expected duration for the request in milliseconds.
        /// If the execution time exceeds this value, a performance log will be generated.
        /// </summary>
        public int Interval { get; }

    }
}