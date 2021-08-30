using System;
using System.Text.Json.Serialization;
using Xunit;

namespace Blazor.ExtraDry.Core.Tests.Rules {
    public class RuleEngineCreateTests {
        [Fact]
        public void CreateRequiresItem()
        {
            var rules = new RuleEngine(new ServiceProviderStub());

            Assert.Throws<ArgumentNullException>(() => rules.Create((object)null));
        }

        [Fact]
        public void CreateWithValueTypesByDefault()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var exemplar = new Entity {
                DefaultInteger = 123,
                DefaultString = "Hello World",
                DefaultGuid = Guid.NewGuid(),
                DefaultState = State.Active
            };

            var entity = rules.Create(exemplar);

            Assert.NotNull(entity);
            Assert.Equal(exemplar.DefaultInteger, entity.DefaultInteger);
            Assert.Equal(exemplar.DefaultString, entity.DefaultString);
            Assert.Equal(exemplar.DefaultGuid, entity.DefaultGuid);
            Assert.Equal(exemplar.DefaultState, entity.DefaultState);
        }

        [Fact]
        public void CreateWithValueTypes()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var exemplar = new Entity {
                Integer = 123,
                String = "Hello World",
                Guid = Guid.NewGuid(),
                State = State.Active
            };

            var entity = rules.Create(exemplar);

            Assert.NotNull(entity);
            Assert.Equal(exemplar.Integer, entity.Integer);
            Assert.Equal(exemplar.String, entity.String);
            Assert.Equal(exemplar.Guid, entity.Guid);
            Assert.Equal(exemplar.State, entity.State);
        }

        [Fact]
        public void CreateWithValueTypesByInvalidAction()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var exemplar = new Entity {
                CreateInteger = 123
            };

            Assert.Throws<InvalidOperationException>(() => rules.Create(exemplar));
        }

        [Fact]
        public void CreateWithReferenceTypeByDefault()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var exemplar = new Entity {
                DefaultTestObject = new() {
                    PropertyOne = "John Doe",
                    PropertyTwo = "Jane Doe",
                    PropertyThree = "Hello World"
                }
            };

            var entity = rules.Create(exemplar);

            Assert.NotNull(entity);
            Assert.NotNull(entity.DefaultTestObject);
            Assert.NotEqual(exemplar.DefaultTestObject, entity.DefaultTestObject);
            Assert.Equal(exemplar.DefaultTestObject.PropertyOne, entity.DefaultTestObject.PropertyOne);
            Assert.Equal(exemplar.DefaultTestObject.PropertyTwo, entity.DefaultTestObject.PropertyTwo);
            Assert.Equal(exemplar.DefaultTestObject.PropertyThree, entity.DefaultTestObject.PropertyThree);
        }

        [Fact]
        public void CreateWithReferenceType()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var exemplar = new Entity {
                TestObject = new() {
                    PropertyOne = "John Doe",
                    PropertyTwo = "Jane Doe",
                    PropertyThree = "Hello World"
                }
            };

            var entity = rules.Create(exemplar);

            Assert.NotNull(entity);
            Assert.NotNull(entity.TestObject);
            Assert.NotEqual(exemplar.TestObject, entity.TestObject);
            Assert.Equal(exemplar.TestObject.PropertyOne, entity.TestObject.PropertyOne);
            Assert.Equal(exemplar.TestObject.PropertyTwo, entity.TestObject.PropertyTwo);
            Assert.Equal(exemplar.TestObject.PropertyThree, entity.TestObject.PropertyThree);
        }

        [Fact]
        public void CreateWithReferenceTypeByLinkExisting()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var exemplar = new Entity {
                ExistingTestObject = new() {
                    PropertyOne = "John Doe",
                    PropertyTwo = "Jane Doe",
                    PropertyThree = "Hello World"
                }
            };

            var entity = rules.Create(exemplar);

            Assert.NotNull(entity);
            Assert.NotNull(entity.ExistingTestObject);
            Assert.Equal(exemplar.ExistingTestObject, entity.ExistingTestObject);
            Assert.Equal(exemplar.ExistingTestObject.PropertyOne, entity.ExistingTestObject.PropertyOne);
            Assert.Equal(exemplar.ExistingTestObject.PropertyTwo, entity.ExistingTestObject.PropertyTwo);
            Assert.Equal(exemplar.ExistingTestObject.PropertyThree, entity.ExistingTestObject.PropertyThree);
        }

        [Fact]
        public void CreateWithInvalidReferenceType()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var exemplar = new InvalidReferenceTypes {
                TestObject = new()
            };

            Assert.Throws<InvalidOperationException>(() => rules.Create(exemplar));
        }
    }

    class Entity {
        public int DefaultInteger { get; set; }
        public string DefaultString { get; set; }
        public Guid DefaultGuid { get; set; }
        public State DefaultState { get; set; }
        public ChildEntity DefaultTestObject { get; set; }

        [Rules(CreateAction = CreateAction.LinkExisting)]
        public int Integer { get; set; }
        [Rules(CreateAction = CreateAction.LinkExisting)]
        public string String { get; set; }
        [Rules(CreateAction = CreateAction.LinkExisting)]
        public Guid Guid { get; set; }
        [Rules(CreateAction = CreateAction.LinkExisting)]
        public State State { get; set; }
        [Rules(CreateAction = CreateAction.Create)]
        public ChildEntity TestObject { get; set; }
        [Rules(CreateAction = CreateAction.LinkExisting)]
        public ChildEntity ExistingTestObject { get; set; }

        [Rules(CreateAction = CreateAction.Create)]
        public int CreateInteger { get; set; }
    }
        
    public class ChildEntity {
        public string PropertyOne { get; set; }
        public string PropertyTwo { get; set; }
        public string PropertyThree { get; set; }
    }

    class InvalidReferenceTypes {
        public InvalidTestObject TestObject { get; set; }
    }

    class InvalidTestObject { }

    enum State {
        Unknowm = 0,
        Active = 1,
    }
}
