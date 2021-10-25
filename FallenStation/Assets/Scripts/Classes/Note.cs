public class Note
{
   public string title { get; }  
   public string author { get; }
    public string date { get; }

    public string body { get; }

    public Note(string title, string author, string date, string body)
    {
        this.title = title;
        this.author = author;
        this.date = date;
        this.body = body;
    }

}
