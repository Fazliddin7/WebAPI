using Domain.Models;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Tests.Common
{
    public  class FlightContextFactory
    {
        public const int FlightIdForDelete = 3;
        public const int FlightIdForUpdate = 4;
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            context.Flights.AddRange(
                FlightEntity.New
                (
                    new Flight()
                    {
                        Id = 1,
                        Arrival = DateTime.Now,
                        Departure = DateTime.Now,
                        Destination  = "Dest1",
                        Origin = "Orig1",
                        Status = Domain.Enums.FlightStatus.InTime
                    }
                ),
                FlightEntity.New
                (
                    new Flight()
                    {
                        Id = 2,
                        Arrival = DateTime.Now,
                        Departure = DateTime.Now,
                        Destination = "Dest1",
                        Origin = "Orig1",
                        Status = Domain.Enums.FlightStatus.InTime
                    }
                ),
                FlightEntity.New
                (
                    new Flight()
                    {
                        Id = FlightIdForDelete,
                        Arrival = DateTime.Now,
                        Departure = DateTime.Now,
                        Destination = "Dest1",
                        Origin = "Orig1",
                        Status = Domain.Enums.FlightStatus.InTime
                    }
                ),
                FlightEntity.New
                (
                    new Flight()
                    {
                        Id = FlightIdForUpdate,
                        Arrival = DateTime.Now,
                        Departure = DateTime.Now,
                        Destination = "Dest1",
                        Origin = "Orig1",
                        Status = Domain.Enums.FlightStatus.InTime
                    }
                )
            );
            context.SaveChanges();
            return context;
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
