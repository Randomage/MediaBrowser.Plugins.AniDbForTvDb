﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
using MediaBrowser.Plugins.AniMetadata.Process.Providers;

namespace MediaBrowser.Plugins.AniMetadata.EntryPoints
{
    public class SeasonProviderEntryPoint : IRemoteMetadataProvider<Season, SeasonInfo>
    {
        private readonly SeasonProvider _seasonProvider;

        public SeasonProviderEntryPoint(IApplicationHost applicationHost)
        {
            _seasonProvider = DependencyConfiguration.Resolve<SeasonProvider>(applicationHost);
        }

        public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeasonInfo searchInfo,
            CancellationToken cancellationToken)
        {
            return _seasonProvider.GetSearchResults(searchInfo, cancellationToken);
        }

        public Task<MetadataResult<Season>> GetMetadata(SeasonInfo info, CancellationToken cancellationToken)
        {
            return _seasonProvider.GetMetadata(info, cancellationToken);
        }

        public string Name => _seasonProvider.Name;

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            return _seasonProvider.GetImageResponse(url, cancellationToken);
        }
    }
}