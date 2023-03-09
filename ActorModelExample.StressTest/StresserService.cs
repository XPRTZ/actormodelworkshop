using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ActorModelExample.StressTest
{
    public class StresserService : BackgroundService
    {
        private int _numberOfConcurrentConnections;
        private CancellationTokenSource _startCancel;
        private HttpClient _client;
        private int _numberOfSeats;
        private LiveEvent[] _liveEvents;

        public StresserService(int numberOfConcurrentConnections) 
        {
            _numberOfConcurrentConnections = numberOfConcurrentConnections;
            _client = new HttpClient();            
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _startCancel = new CancellationTokenSource();         
        }



        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _startCancel.Cancel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _startCancel = new CancellationTokenSource();
            
            _liveEvents = await _client.GetFromJsonAsync<LiveEvent[]>("https://localhost:7164/api/venue/LiveEvents");
            _numberOfSeats = await _client.GetFromJsonAsync<int>("https://localhost:7164/api/venue/NumberOfSeats");

            var tasks = new Task[_numberOfConcurrentConnections];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = SeatBooker.CreateNewSeatBooker(_liveEvents, _numberOfSeats).Run(_startCancel.Token);
            }
            Task.WaitAll(tasks);
        }
    }
}
