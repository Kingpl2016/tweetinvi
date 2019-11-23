﻿using System;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Credentials;
using Tweetinvi.Models;

namespace Tweetinvi
{
    /// <summary>
    /// Authenticate user or application with existing credentials.
    /// If you need to create new credentials, use AuthFlow.
    /// </summary>
    public class Auth
    {
        [ThreadStatic]
        private static ICredentialsAccessor _credentialsAccessor;

        /// <summary>
        /// Object responsible to retrieve the current thread credentials.
        /// </summary>
        public static ICredentialsAccessor CredentialsAccessor
        {
            get
            {
                if (_credentialsAccessor == null)
                {
                    Initialize();
                }

                return _credentialsAccessor;
            }
        }

        [ThreadStatic]
        private static IAuthFactory _authFactoryForCurrentThread;
        private static IAuthFactory AuthFactory
        {
            get
            {
                if (_authFactoryForCurrentThread == null)
                {
                    Initialize();
                }

                return _authFactoryForCurrentThread;
            }
        }

        static Auth()
        {
            Initialize();
        }

        private static void Initialize()
        {
            _credentialsAccessor = TweetinviContainer.Resolve<ICredentialsAccessor>();
            _authFactoryForCurrentThread = TweetinviContainer.Resolve<IAuthFactory>();
        }

        /// <summary>
        /// Application wide credentials. When a new thread is starting, the thread credentials will be defaulted to this value.
        /// </summary>
        public static ITwitterCredentials ApplicationCredentials
        {
            get { return CredentialsAccessor.ApplicationCredentials; }
            set { CredentialsAccessor.ApplicationCredentials = value; }
        }

        /// <summary>
        /// Current Thread credentials.
        /// </summary>
        public static ITwitterCredentials Credentials
        {
            get { return CredentialsAccessor.CurrentThreadCredentials; }
            set { CredentialsAccessor.CurrentThreadCredentials = value; }
        }

        /// <summary>
        /// Set the credentials of the running thread
        /// </summary>
        public static void SetCredentials(ITwitterCredentials credentials)
        {
            Credentials = credentials;
        }

        /// <summary>
        /// Invalidate application only credentials so that they can no longer be used to access Twitter.
        /// </summary>
        public static bool InvalidateCredentials(ITwitterCredentials credentials = null)
        {
            return AuthFactory.InvalidateCredentials(credentials ?? CredentialsAccessor.CurrentThreadCredentials).Result;
        }

        /// <summary>
        /// Execute an action with a specific set of credentials and returns the value.
        /// </summary>
        public static T ExecuteOperationWithCredentials<T>(ITwitterCredentials credentials, Func<T> operation)
        {
            return CredentialsAccessor.ExecuteOperationWithCredentials(credentials, operation);
        }

        /// <summary>
        /// Execute an action with a specific set of credentials.
        /// </summary>
        public static void ExecuteOperationWithCredentials(ITwitterCredentials credentials, Action operation)
        {
            CredentialsAccessor.ExecuteOperationWithCredentials(credentials, operation);
        }
    }
}