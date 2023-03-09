using ActorModelExample.Domain.Common;
using ActorModelExample.Domain.Models;
using ActorModelExample.Domain.Services;
using ActorModelExample.WebApp.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ActorModelExample.StressTest
{
    public class SeatBooker
    {
        private HttpClient _client;
        private LiveEvent[] _events;
        private int _nrOfSeats;

        private bool _keepRunning = true;

        public SeatBooker(LiveEvent[] liveEvents, int nrOfSeats) 
        { 
            _client= new HttpClient();
            _events = liveEvents;
            _nrOfSeats = nrOfSeats;
        }

        public static SeatBooker CreateNewSeatBooker(LiveEvent[] liveEvents, int nrOfSeats)
        {
            return new SeatBooker(liveEvents, nrOfSeats);
        }

        public async Task Run(CancellationToken token)
        {            
            while(!token.IsCancellationRequested && _keepRunning)
            {
                await BookSeat();
            }
        }

        private async Task BookSeat()
        {
            var liveEvent = _events[new Random().Next(1, 5)];
            try
            {
                var bookingId = Guid.NewGuid();
                var seatStatus = await _client.GetFromJsonAsync<Dictionary<SeatStatus, List<int>>>($"https://localhost:7164/api/venue/GetSeatStatus/{liveEvent.Id}/{bookingId}");
                var availableSeats = seatStatus[SeatStatus.Available].ToArray();
                var seat = availableSeats[new Random().Next(0, availableSeats.Length - 1)];

                Console.WriteLine($"Booking seat: {seat} for {liveEvent.Artist}");

                var reserveResponse = await _client.PostAsJsonAsync("https://localhost:7164/api/venue/ReserveSeat", new ReserveRequest { LiveEvent = liveEvent, BookingId = bookingId, Seat = seat });

                if (reserveResponse != null && reserveResponse.IsSuccessStatusCode)
                {
                    var confrimResponse = await _client.PostAsJsonAsync("https://localhost:7164/api/venue/Confirm", new ConfirmRequest { LiveEvent = liveEvent, BookingId = bookingId, Seats = new List<int> { seat } });
                }
                else
                {
                    Console.WriteLine("Auw! (reserve)");
                }
            }
            catch (Exception ex)
            {
                _keepRunning = false;
                Console.WriteLine($"Bye...");
                return;
            }
        }
    }
}
