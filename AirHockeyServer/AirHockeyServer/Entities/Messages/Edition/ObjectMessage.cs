namespace AirHockeyServer.Entities.Messages.Edition
{
    public abstract class ObjectMessage
    {
        protected string objectUuid;

        protected ObjectMessage(string objectUuid)
        {
            this.objectUuid = objectUuid;
        }
        public string ObjectUuid
        {
            get => objectUuid;
            set => objectUuid = value;
        }
    }
}