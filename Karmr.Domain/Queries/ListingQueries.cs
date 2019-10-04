﻿using System;
using System.Collections.Generic;
using Karmr.Common.Contracts;
using Karmr.Domain.Queries.Models;

namespace Karmr.Domain.Queries
{
    public sealed class ListingQueries
    {
        private IQueryRepository repository;

        public ListingQueries(IQueryRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Listing> GetAll()
        {
            return this.repository.Query<Listing>("SELECT [Id], [Name], [Description], [LocationName], [Created], [Modified] FROM ReadModel.Listing");
        }

        public IEnumerable<Listing> GetByUserId(Guid userId)
        {
            return this.repository.Query<Listing>(
                "SELECT [Id], [Name], [Description], [LocationName], [Created], [Modified] FROM ReadModel.Listing WHERE [UserId] = @UserId",
                new { userId });
        }

        public Listing GetById(Guid id)
        {
            return this.repository.QuerySingle<Listing>(
                "SELECT [Id], [Name], [Description], [LocationName], [Created], [Modified] FROM ReadModel.Listing WHERE [Id] = @Id",
                new { id });
        }
    }
}