﻿using System.Threading.Tasks;
using LanguageExt;
using MediaBrowser.Plugins.AniMetadata.Process;
using MediaBrowser.Plugins.AniMetadata.Process.Sources;

namespace MediaBrowser.Plugins.AniMetadata.SourceDataLoaders
{
    /// <summary>
    ///     Loads season data from AniDb based on the data provided by Emby
    /// </summary>
    internal class AniDbSeasonFromEmbyData : IEmbySourceDataLoader
    {
        private readonly ISources _sources;

        public AniDbSeasonFromEmbyData(ISources sources)
        {
            _sources = sources;
        }

        public SourceName SourceName => SourceNames.AniDb;

        public bool CanLoadFrom(IMediaItemType mediaItemType)
        {
            return mediaItemType == MediaItemTypes.Season;
        }

        public Task<Either<ProcessFailedResult, ISourceData>> LoadFrom(IEmbyItemData embyItemData)
        {
            var resultContext = new ProcessResultContext(nameof(AniDbSeasonFromEmbyData), embyItemData.Identifier.Name,
                embyItemData.ItemType);

            var aniDbSeries = _sources.AniDb.GetSeriesData(embyItemData, resultContext);

            return aniDbSeries.BindAsync(series =>
                    _sources.AniDb.SelectTitle(series.Titles, embyItemData.Language, resultContext))
                .MapAsync(seriesTitle => new ItemIdentifier(embyItemData.Identifier.Index.IfNone(1),
                    embyItemData.Identifier.ParentIndex, seriesTitle))
                .MapAsync(itemIdentifier =>
                    (ISourceData)new IdentifierOnlySourceData(_sources.AniDb, Option<int>.None, itemIdentifier));
        }
    }
}