namespace OneNorth.SocialSort.GoogleAnalytics
{
    public class Task
    {
        public void Run()
        {
            var manager = new Manager();
            manager.UpdateCaches();
        }
    }
}