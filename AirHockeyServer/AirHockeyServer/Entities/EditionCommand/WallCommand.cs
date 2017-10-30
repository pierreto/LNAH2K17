﻿namespace AirHockeyServer.Entities.Messages.Edition
{
    public class WallCommand : AbstractEditionCommand
    {
        private int[] startPosition;
        private int[] endPosition;

        public WallCommand(string objectUuid, int[] startPosition, int[] endPosition) : base(objectUuid)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
        }

        public int[] StartPosition
        {
            get => startPosition;
            set => startPosition = value;
        }

        public int[] EndPosition
        {
            get => endPosition;
            set => endPosition = value;
        }
    }
}