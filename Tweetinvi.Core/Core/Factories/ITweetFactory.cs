﻿using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface ITweetFactory
    {
        ITweet CreateTweet(string text, TweetMode? tweetMode, ITwitterClient client);

        // Generate Tweet From Json
        ITweet GenerateTweetFromJson(string json);
        ITweet GenerateTweetFromJson(string json, TweetMode? tweetMode, ITwitterClient client);

        // Generate Tweet from DTO
        ITweet[] GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO, TweetMode? tweetMode, ITwitterClient client);

        ITweetWithSearchMetadata GenerateTweetWithSearchMetadataFromDTO(ITweetWithSearchMetadataDTO tweetDTO);
        IEnumerable<ITweetWithSearchMetadata> GenerateTweetsWithSearchMetadataFromDTOs(IEnumerable<ITweetWithSearchMetadataDTO> tweetsDTO);

        // Generate OEmbedTweet from DTO
        IOEmbedTweet GenerateOEmbedTweetFromDTO(IOEmbedTweetDTO oEmbedTweetDTO);

        // Generate Mention from DTO
        IMention GenerateMentionFromDTO(ITweetDTO tweetDTO);
        IEnumerable<IMention> GenerateMentionsFromDTO(IEnumerable<ITweetDTO> tweetsDTO);
        IOEmbedTweet GenerateOEmbedTweetFromJson(string json);
        ITweet GenerateTweetFromDTO(ITweetDTO tweetDTO, TweetMode? tweetMode, ITwitterClient client);
    }
}