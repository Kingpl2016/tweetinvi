﻿using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Web;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Credentials.AuthHttpHandlers;
using Tweetinvi.Credentials.Properties;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Credentials
{
    public interface IAuthFactory
    {
        bool InitializeApplicationBearer(ITwitterCredentials credentials);

        ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationToken authToken);
        bool InvalidateCredentials(ITwitterCredentials credentials);
    }

    public class AuthFactory : IAuthFactory
    {
        private readonly IExceptionHandlerFactory _exceptionHandlerFactory;
        private readonly ITwitterRequestHandler _twitterRequestHandler;
        private readonly IOAuthWebRequestGenerator _oAuthWebRequestGenerator;
        private readonly IJObjectStaticWrapper _jObjectStaticWrapper;

        public AuthFactory(
            IExceptionHandlerFactory exceptionHandlerFactory,
            ITwitterRequestHandler twitterRequestHandler,
            IOAuthWebRequestGenerator oAuthWebRequestGenerator,
            IJObjectStaticWrapper jObjectStaticWrapper)
        {
            _exceptionHandlerFactory = exceptionHandlerFactory;
            _twitterRequestHandler = twitterRequestHandler;
            _oAuthWebRequestGenerator = oAuthWebRequestGenerator;
            _jObjectStaticWrapper = jObjectStaticWrapper;
        }

        // Step 2 - Generate User Credentials
        public ITwitterCredentials GetCredentialsFromVerifierCode(string verifierCode, IAuthenticationToken authToken)
        {
            try
            {
                if (authToken == null)
                {
                    throw new ArgumentNullException("Authentication Token cannot be null.");
                }

                if (verifierCode == null)
                {
                    throw new ArgumentNullException("VerifierCode",
                        "If you've received a verifier code that is null, " +
                        "it means that authentication has failed!");
                }

                var callbackParameter = _oAuthWebRequestGenerator.GenerateParameter("oauth_verifier", verifierCode, true,
                    true, false);

                var authHandler = new AuthHttpHandler(callbackParameter, authToken);
                var response = _twitterRequestHandler.ExecuteQuery(Resources.OAuthRequestAccessToken, HttpMethod.POST,
                    authHandler,
                    new TwitterCredentials(authToken.ConsumerCredentials));

                if (response == null)
                {
                    return null;
                }

                var responseInformation = Regex.Match(response.Text, Resources.OAuthTokenAccessRegex);
                if (responseInformation.Groups["oauth_token"] == null ||
                    responseInformation.Groups["oauth_token_secret"] == null)
                {
                    return null;
                }

                var credentials = new TwitterCredentials(
                    authToken.ConsumerKey,
                    authToken.ConsumerSecret,
                    responseInformation.Groups["oauth_token"].Value,
                    responseInformation.Groups["oauth_token_secret"].Value);

                return credentials;
            }
            catch (TwitterException ex)
            {
                IExceptionHandler exceptionHandler = _exceptionHandlerFactory.Create();
                if (exceptionHandler.LogExceptions)
                {
                    exceptionHandler.AddTwitterException(ex);
                }

                if (!exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }
            }

            return null;
        }

        public bool InitializeApplicationBearer(ITwitterCredentials credentials)
        {
            if (credentials == null)
            {
                throw new TwitterNullCredentialsException();
            }

            if (string.IsNullOrEmpty(credentials.AccessToken) ||
                string.IsNullOrEmpty(credentials.AccessTokenSecret))
            {
                try
                {
                    var response = _twitterRequestHandler.ExecuteQuery("https://api.twitter.com/oauth2/token", HttpMethod.POST, new BearerHttpHandler(), credentials);
                    var accessToken = Regex.Match(response.Text, "access_token\":\"(?<value>.*)\"").Groups["value"].Value;
                    credentials.ApplicationOnlyBearerToken = accessToken;
                    return true;
                }
                catch (TwitterException ex)
                {
                    IExceptionHandler exceptionHandler = _exceptionHandlerFactory.Create();
                    if (exceptionHandler.LogExceptions)
                    {
                        exceptionHandler.AddTwitterException(ex);
                    }

                    if (!exceptionHandler.SwallowWebExceptions)
                    {
                        throw;
                    }
                }
            }

            return false;
        }

        public bool InvalidateCredentials(ITwitterCredentials credentials)
        {
            var url = "https://api.twitter.com/oauth2/invalidate_token";

            var response = _twitterRequestHandler.ExecuteQuery(url, HttpMethod.POST, new InvalidateTokenHttpHandler(), credentials);
            var jobject = _jObjectStaticWrapper.GetJobjectFromJson(response.Text);

            JToken unused;
            if (jobject.TryGetValue("access_token", out unused))
            {
                return true;
            }

            try
            {
                var errorsObject = jobject["errors"];
                var errors = _jObjectStaticWrapper.ToObject<ITwitterExceptionInfo[]>(errorsObject);

                IExceptionHandler exceptionHandler = _exceptionHandlerFactory.Create();
                exceptionHandler.TryLogExceptionInfos(errors, url);
            }
            catch (Exception)
            {
                // Something is definitely wrong!
            }

            return false;
        }
    }
}