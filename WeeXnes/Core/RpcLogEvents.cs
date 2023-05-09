namespace WeeXnes.Core
{
    public enum EventType
    {
        ProcessStartedEvent,
        ProcessStoppedEvent,
        RPCUpdateEvent,
        RPCReadyEvent
    }
    public class customEvent
    {
        
        public string Content { get; set; }
        public EventType Type { get; set; }

        public string GradientColor1 { get; set; }
        public string GradientColor2 { get; set; }
        public customEvent(string content, EventType type)
        {
            this.Content = content;
            this.Type = type;
            if (this.Type == EventType.ProcessStartedEvent)
            {
                this.GradientColor1 = "#46db69";
                this.GradientColor2 = "#33a34d";
            }
            else if (this.Type == EventType.ProcessStoppedEvent)
            {
                this.GradientColor1 = "#d1415d";
                this.GradientColor2 = "#a33349";
            }
            else if (this.Type == EventType.RPCUpdateEvent)
            {
                this.GradientColor1 = "#3e65c9";
                this.GradientColor2 = "#3352a3";
            }
            else if (this.Type == EventType.RPCReadyEvent)
            {
                this.GradientColor1 = "#c93eb4";
                this.GradientColor2 = "#a33389";
            }
        }
        

        public override string ToString()
        {
            return this.Content;
        }
    }
}