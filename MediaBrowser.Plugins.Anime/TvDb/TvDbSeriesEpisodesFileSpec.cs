﻿using MediaBrowser.Plugins.Anime.Files;
using MediaBrowser.Plugins.Anime.TvDb.Data;

namespace MediaBrowser.Plugins.Anime.TvDb
{
    internal class TvDbSeriesEpisodesFileSpec : ILocalFileSpec<TvDbSeriesData>
    {
        private readonly ICustomJsonSerialiser _jsonSerialiser;
        private readonly string _rootPath;
        private readonly int _tvDbSeriesId;

        public TvDbSeriesEpisodesFileSpec(ICustomJsonSerialiser jsonSerialiser, string rootPath, int tvDbSeriesId)
        {
            _jsonSerialiser = jsonSerialiser;
            _rootPath = rootPath;
            _tvDbSeriesId = tvDbSeriesId;
        }

        public string LocalPath => System.IO.Path.Combine(_rootPath, $"anidb\\tvdb\\{_tvDbSeriesId}.json");

        public ISerialiser Serialiser => _jsonSerialiser;
    }
}