﻿namespace Karmr.DomainUnitTests.Entities
{
    using Builders;
    using Karmr.Contracts;
    using Karmr.Domain.Commands;
    using Karmr.Domain.Entities;
    using Karmr.Domain.Events;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Karmr.DomainUnitTests.Helpers;

    public class ListingTests
    {
        private readonly IClock clock = new StaticClock(DateTime.UtcNow);

        [Test]
        public void HandlingCreateListingCommandUpdatesState()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(command);

            Assert.AreEqual(command.EntityKey, subject.Id);
            Assert.AreEqual(command.UserId, subject.UserId);
            Assert.AreEqual(command.Description, subject.Description);
        }

        [Test]
        public void HandlingCreateListingCommandRaisesEvent()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(command);

            var uncommittedEvents = subject.GetUncommittedEvents();
            Assert.AreEqual(1, subject.Events.Count);
            Assert.AreEqual(1, uncommittedEvents.Count);
            var @event = uncommittedEvents.First() as ListingCreated;
            Assert.NotNull(@event);
            Assert.AreEqual(command.EntityKey, @event.EntityKey);
            Assert.AreEqual(command.UserId, @event.UserId);
            Assert.AreEqual(command.Description, @event.Description);
            Assert.AreEqual(this.clock.UtcNow, @event.Timestamp);
        }

        [Test]
        public void HandlingCreateListingCommandTwiceThrowsException()
        {
            var subject = this.GetSubject();

            var command = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(command);

            Assert.Throws<Exception>(() => subject.Handle(command));
        }

        [Test]
        public void HandlingUpdateListingCommandRequiresCreateEvent()
        {
            var subject = this.GetSubject();
            var command = new CommandBuilder<UpdateListingCommand>().Build();

            Assert.Throws<Exception>(() => subject.Handle(command));
        }

        [Test]
        public void HandlingUpdateListingCommandChecksUserId()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var updateCommand = new CommandBuilder<UpdateListingCommand>().With(x => x.UserId, Guid.NewGuid()).Build();

            Assert.Throws<Exception>(() => subject.Handle(updateCommand));
        }

        [Test]
        public void HandlingUpdateListingCommandUpdatesState()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var updateCommand = new CommandBuilder<UpdateListingCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .With(x => x.Description, createCommand.Description + " tail")
                .Build();

            subject.Handle(updateCommand);

            Assert.AreEqual(updateCommand.Description, subject.Description);
        }

        [Test]
        public void HandlingUpdateListingCommandRaisesEvent()
        {
            var subject = this.GetSubject();
            var createCommand = new CommandBuilder<CreateListingCommand>().Build();
            subject.Handle(createCommand);

            var updateCommand = new CommandBuilder<UpdateListingCommand>()
                .With(x => x.UserId, createCommand.UserId)
                .With(x => x.Description, createCommand.Description + " tail")
                .Build();

            subject.Handle(updateCommand);

            var uncommittedEvents = subject.GetUncommittedEvents();
            Assert.AreEqual(2, subject.Events.Count);
            Assert.AreEqual(2, uncommittedEvents.Count);
            var @event = uncommittedEvents.Last() as ListingUpdated;
            Assert.NotNull(@event);
            Assert.AreEqual(updateCommand.EntityKey, @event.EntityKey);
            Assert.AreEqual(updateCommand.UserId, @event.UserId);
            Assert.AreEqual(updateCommand.Description, @event.Description);
            Assert.AreEqual(this.clock.UtcNow, @event.Timestamp);
        }

        private Listing GetSubject()
        {
            return new Listing(this.clock, new List<IEvent>());
        }
    }
}