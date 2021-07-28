﻿namespace GamingWiki.Services.Models.Characters
{
    public class CharacterServiceDetailsModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public string Class { get; set; }

        public int ClassId { get; set; }

        public string Game { get; set; }

        public int GameId { get; set; }

    }
}
