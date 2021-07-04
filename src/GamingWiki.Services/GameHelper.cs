using System;
using System.Collections.Generic;
using System.Linq;
using GamingWiki.Data;
using GamingWiki.Models;
using GamingWiki.Models.Enums;
using GamingWiki.Services.Contracts;

namespace GamingWiki.Services
{
    public class GameHelper : IGameHelper
    {
        private readonly ApplicationDbContext dbContext;

        public GameHelper(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Place ParsePlace(string placeName, string placeType)
        {
            var place = this.dbContext.Places
                .FirstOrDefault
                    (p => p.Name == placeName);

            if (place == null)
            {
                place = new Place
                {
                    Name = placeName,
                    PlaceType = Enum.Parse<PlaceType>(placeType)
                };

                this.dbContext.Places.Add(place);
                this.dbContext.SaveChanges();
            }

            return place;
        }

        public IEnumerable<Creator> ParseCreators(string creatorsNames)
        {
            var creators = new List<Creator>();

            foreach (var creatorName in creatorsNames
                .Split(", "))
            {
                var creator = this.dbContext.Creators
                    .FirstOrDefault
                        (c => c.Name == creatorName);

                if (creator == null)
                {
                    creator = new Creator
                    {
                        Name = creatorName
                    };

                    this.dbContext.Creators.Add(creator);
                    this.dbContext.SaveChanges();
                }
                
                creators.Add(creator);
            }

            return creators;
        }
    }
}
