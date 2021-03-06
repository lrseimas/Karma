﻿using System;
using System.Collections.Generic;
using Karmr.Common.Types;
using Karmr.Domain.Commands;

namespace Karmr.DomainUnitTests.Builders
{
    internal static class CommandBuilderHelper
    {
        private static readonly Dictionary<Type, Dictionary<string, object>> CommandConstructorArguments = new Dictionary<Type, Dictionary<string, object>>();

        static CommandBuilderHelper()
        {
            CommandConstructorArguments.Add(typeof(CreateListingCommand), new Dictionary<string, object>
            {
                { "UserId", Guid.NewGuid() },
                { "Name", "Name" },
                { "Description", "Description" },
                { "LocationName", "Location" },
                { "Location", new GeoLocation(1.23m, 42.123m) }
            });

            CommandConstructorArguments.Add(typeof(UpdateListingCommand), new Dictionary<string, object>
            {
                { "EntityKey", Guid.NewGuid() },
                { "UserId", Guid.NewGuid() },
                { "Name", "Name" },
                { "Description", "Description" },
                { "LocationName", "Location" },
                { "Location", new GeoLocation(1.23m, 42.123m) }
            });

            CommandConstructorArguments.Add(typeof(ArchiveListingCommand), new Dictionary<string, object>
            {
                { "EntityKey", Guid.NewGuid() },
                { "UserId", Guid.NewGuid() }
            });

            // listing discussion
            CommandConstructorArguments.Add(typeof(CreateListingDiscussionThreadCommand), new Dictionary<string, object>
            {
                { "EntityKey", Guid.NewGuid() },
                { "UserId", Guid.NewGuid() },
                { "Content", "Thread content" }
            });

            CommandConstructorArguments.Add(typeof(CreateListingDiscussionPostCommand), new Dictionary<string, object>
            {
                { "EntityKey", Guid.NewGuid() },
                { "UserId", Guid.NewGuid() },
                { "ThreadId", Guid.NewGuid() },
                { "Content", "Post content" }
            });

            // listing offers
            CommandConstructorArguments.Add(typeof(CreateListingOfferCommand), new Dictionary<string, object>
            {
                { "EntityKey", Guid.NewGuid() },
                { "UserId", Guid.NewGuid() }
            });

            CommandConstructorArguments.Add(typeof(AcceptListingOfferCommand), new Dictionary<string, object>
            {
                { "EntityKey", Guid.NewGuid() },
                { "UserId", Guid.NewGuid() },
                { "OfferId", Guid.NewGuid() }
            });
        }

        internal static Dictionary<string, object> GetConstructorArguments(Type type)
        {
            if (!CommandConstructorArguments.ContainsKey(type))
            {
                throw new KeyNotFoundException(string.Format("CommandBuilderHelper - default constructor parameters not found for type {0}", type.Name));
            }
            return new Dictionary<string, object>(CommandConstructorArguments[type]);
        }
    }
}