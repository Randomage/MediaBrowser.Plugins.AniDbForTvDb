﻿using System.IO;
using MediaBrowser.Plugins.AniMetadata.Files;
using MediaBrowser.Plugins.AniMetadata.TvDb.Data;

namespace MediaBrowser.Plugins.AniMetadata.TvDb
{
    internal class TvDbSeriesEpisodesFileSpec : ILocalFileSpec<TvDbEpisodeCollection>
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

        public string LocalPath => Path.Combine(_rootPath, $"anidb\\tvdb\\{_tvDbSeriesId}_Episodes.json");

        public ISerialiser Serialiser => _jsonSerialiser;
    }
}