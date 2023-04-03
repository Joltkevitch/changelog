namespace console_dummy
{
    public class CommitBody
    {
        public string? Description { get; set; }

        public CommitBody(string? description)
        {
            Description = description?.Trim();
        }
        
    }
}