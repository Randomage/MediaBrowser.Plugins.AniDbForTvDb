﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Functional.Maybe;
using MediaBrowser.Model.Logging;

namespace MediaBrowser.Plugins.Anime.AniDb.Titles
{
    internal class SeriesTitleCache : ISeriesTitleCache
    {
        private readonly IAniDbDataCache _aniDbDataCache;
        private readonly ILogger _log;
        private readonly Lazy<IDictionary<string, TitleListItemData>> _titles;

        public SeriesTitleCache(IAniDbDataCache aniDbDataCache, ILogManager logManager)
        {
            _aniDbDataCache = aniDbDataCache;
            _log = logManager.GetLogger(nameof(SeriesTitleCache));
            _titles = new Lazy<IDictionary<string, TitleListItemData>>(GetTitles);
        }

        public Maybe<TitleListItemData> FindSeriesByTitle(string title)
        {
            var match = FindExactTitleMatch(title).Or(() => FindComparableMatch(title));

            return match;
        }

        private Maybe<TitleListItemData> FindExactTitleMatch(string title)
        {
            _titles.Value.TryGetValue(title, out TitleListItemData match);

            var foundTitle = match.ToMaybe();

            foundTitle.Match(t => _log.Debug($"Found exact title match for '{title}'"),
                () => _log.Debug($"Failed to find exact title match for '{title}'"));

            return foundTitle;
        }

        private Maybe<TitleListItemData> FindComparableMatch(string title)
        {
            title = GetComparableTitle(title);

            _titles.Value.TryGetValue(title, out TitleListItemData match);

            var foundTitle = match.ToMaybe();

            foundTitle.Match(t => _log.Debug($"Found comparable title match for '{title}'"),
                () => _log.Debug($"Failed to find comparable title match for '{title}'"));

            return foundTitle;
        }

        private IDictionary<string, TitleListItemData> GetTitles()
        {
            var titles = new Dictionary<string, TitleListItemData>(StringComparer.OrdinalIgnoreCase);

            var titlesAgainstItems = _aniDbDataCache.TitleList.SelectMany(i => i.Titles.Select(t => new
            {
                t.Title,
                ComparableTitle = GetComparableTitle(t.Title),
                Item = i
            }));

            foreach (var titlesAgainstItem in titlesAgainstItems)
            {
                AddIfMissing(titles, titlesAgainstItem.Title, titlesAgainstItem.Item);
                AddIfMissing(titles, titlesAgainstItem.ComparableTitle, titlesAgainstItem.Item);
            }

            return titles;
        }

        private void AddIfMissing(IDictionary<string, TitleListItemData> dictionary, string key, TitleListItemData value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }

        private string GetComparableTitle(string title)
        {
            title = title.ToLower().Normalize(NormalizationForm.FormKD);

            var actions = new Func<string, string>[]
            {
                RemoveModifiersAndDiacritics,
                RemoveIgnoredCharacters,
                ReplaceSpaceCharacters,
                ExpandAmpersands,
                RemoveErroneousArticles,
                CollapseMultipleSpaces
            };

            foreach (var action in actions)
                title = action(title);

            var comparableTitle = title.Trim();
            
            return comparableTitle;
        }

        private string RemoveModifiersAndDiacritics(string title)
        {
            return new string(title.Where(c => !(c >= 0x2B0 && c <= 0x0333)).ToArray());
        }

        private string RemoveIgnoredCharacters(string title)
        {
            var ignoredCharactersRegex = new Regex($"[{Regex.Escape(@"""'!`?")}]");

            return ignoredCharactersRegex.Replace(title, "");
        }

        private string ReplaceSpaceCharacters(string title)
        {
            var spaceCharactersRegex = new Regex(@"[/,\.:;\\\(\)\{}\[\]\+\-_\=\–\*]");

            return spaceCharactersRegex.Replace(title, " ");
        }

        private string ExpandAmpersands(string title)
        {
            return title.Replace("&", " and ");
        }

        private string CollapseMultipleSpaces(string title)
        {
            var multipleSpaceRegex = new Regex(" {2,}");

            return multipleSpaceRegex.Replace(title, " ");
        }

        private string RemoveErroneousArticles(string title)
        {
            return title.Replace(", the", "").Replace("the ", " ").Replace(" the ", " ");
        }
    }
}