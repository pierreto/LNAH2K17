namespace AirHockeyServer.Entities.Messages.Edition
{
    public abstract class AbstractEditionCommand
    {
        protected string objectUuid;

        protected AbstractEditionCommand(string objectUuid)
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