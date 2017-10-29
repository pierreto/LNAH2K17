using System;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using AirHockeyServer.Entities.Messages;

namespace AirHockeyServer.Hubs
{
    public class Groupcaster
    {
        private readonly static Lazy<Groupcaster> _instance = new Lazy<Groupcaster>(() => new Groupcaster());

        // We're going to broadcast to all clients a maximum of 25 times per second
        private readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(40);

        private readonly IHubContext _hubContext;

        private Timer _broadcastLoop;

        //private ShapeModel _model;

        //private bool _modelUpdated;

        private GameSlaveData _gameSlaveData;
        private bool _gameSlaveUpdated = false;

        private GameMasterData _gameMasterData;
        private bool _gameMasterUpdated = false;

        private int _gameId;

        public Groupcaster()
        {
            // Save our hub context so we can easily use it 
            // to send to its connected clients
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<GameWaitingRoomHub>();
            _gameSlaveData = new GameSlaveData();
            _gameMasterData = new GameMasterData();

            // Start the broadcast loop
            _broadcastLoop = new Timer(
              BroadcastData,
              null,
              BroadcastInterval,
              BroadcastInterval);
        }
        public void BroadcastData(object state)
        {
            // No need to send anything if our model hasn't changed
            if (_gameMasterUpdated)
            {
                // This is how we can access the Clients property 
                // in a static hub method or outside of the hub entirely
                //_hubContext.Clients.AllExcept(_model.LastUpdatedBy).updateShape(_model);
                //_modelUpdated = false;

                _hubContext.Clients.Group(_gameId.ToString()).ReceivedMasterData(_gameMasterData);
                _gameMasterUpdated = false;
            }

            if(_gameSlaveUpdated)
            {
                _hubContext.Clients.Group(_gameId.ToString()).ReceiveSlaveData(_gameSlaveData);
                _gameSlaveUpdated = false;
            }
        }
        public void SlaveUpdated(GameSlaveData gameSlaveData)
        {
            _gameSlaveData = gameSlaveData;
            _gameSlaveUpdated = true;
        }

        public void MasterUpdated(GameMasterData gameMasterData)
        {
            _gameMasterData = gameMasterData;
            _gameMasterUpdated = true;
        }

        public void SetGame(int gameId)
        {
            _gameId = gameId;
        }

        public static Groupcaster Instance
        {
            get
            {
                return _instance.Value;
            }
        }
    }
}