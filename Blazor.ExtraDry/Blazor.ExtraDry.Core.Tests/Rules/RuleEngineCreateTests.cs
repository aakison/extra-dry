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
        public void CreateWithReferenceTypeByDefault()
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
        public int Integer { get; set; }
        public string String { get; set; }
        public Guid Guid { get; set; }
        public State State { get; set; }
        public ChildEntity TestObject { get; set; }
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
