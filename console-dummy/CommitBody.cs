namespace console_dummy
{
    public class CommitBody
    {
        public string? Description { get; set; }

        public CommitBody(string? description)
        {
            Description = string.Join(" ", description?.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }

    }
}