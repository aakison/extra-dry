using System;
using Xunit;

namespace Blazor.ExtraDry.Core.Tests.Rules {
    public class RuleEngineDeleteTests {

        [Fact]
        public void DeleteRequiresItem()
        {
            var rules = new RuleEngine(new ServiceProviderStub());

            Assert.Throws<ArgumentNullException>(() => rules.Delete((object)null, NoOp));
        }

        [Fact]
        public void DeleteSoftDeletesByDefault()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var obj = new SoftDeletable();

            var result = rules.Delete(obj, null);

            Assert.False(obj.Active);
            Assert.Equal(DeleteResult.SoftDeleted, result);
        }

        [Fact]
        public void DeleteHardDeleteBackup()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var obj = new SoftDeletable();
            var deleted = false;

            var result = rules.Delete(new object(), () => deleted = true);

            Assert.True(deleted);
            Assert.Equal(DeleteResult.HardDeleted, result);
        }

        [Fact]
        public void DeleteSoftRequiresItem()
        {
            var rules = new RuleEngine(new ServiceProviderStub());

            Assert.Throws<ArgumentNullException>(() => rules.DeleteSoft((object)null, NoOp, NoOp));
        }

        [Fact]
        public void DeleteSoftChangesActive()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var obj = new SoftDeletable();

            var result = rules.DeleteSoft(obj, NoOp, NoOp);

            Assert.False(obj.Active);
            Assert.Equal(DeleteResult.SoftDeleted, result);
        }

        [Fact]
        public void DeleteSoftFallbackNotNull()
        {
            var rules = new RuleEngine(new ServiceProviderStub());

            Assert.Throws<InvalidOperationException>(
                () => rules.DeleteSoft(new object(), null, null)
            );
        }

        [Fact]
        public void DeleteSoftFallbackExecutes()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var executed = false;

            var result = rules.DeleteSoft(new object(), () => executed = true, null);

            Assert.True(executed);
            Assert.Equal(DeleteResult.HardDeleted, result);
        }

        [Fact]
        public void DeleteSoftFallbackAndCommitExecutes()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var executed = false;
            var committed = false;

            var result = rules.DeleteSoft(new object(), () => executed = true, () => committed = true);

            Assert.True(executed);
            Assert.True(committed);
            Assert.Equal(DeleteResult.HardDeleted, result);
        }


        [Fact]
        public void DeleteHardRequiresItem()
        {
            var rules = new RuleEngine(new ServiceProviderStub());

            Assert.Throws<ArgumentNullException>(() => rules.DeleteHard((object)null, NoOp, NoOp));
        }

        [Fact]
        public void DeleteHardRequiresPrepareAction()
        {
            var rules = new RuleEngine(new ServiceProviderStub());

            Assert.Throws<ArgumentNullException>(() => rules.DeleteHard(new object(), null, NoOp));
        }

        [Fact]
        public void DeleteHardRequiresCommitAction()
        {
            var rules = new RuleEngine(new ServiceProviderStub());

            Assert.Throws<ArgumentNullException>(
                () => rules.DeleteHard(new object(), NoOp, null)
            );
        }

        [Fact]
        public void DeleteHardPrepareCommitCycle()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            int prepared = 0;
            int committed = 0;

            var result = rules.DeleteHard(new object(), () => FakePrepare(ref prepared), () => FakeCommit(ref committed));

            Assert.Equal(1, prepared);
            Assert.Equal(2, committed);
            Assert.Equal(DeleteResult.HardDeleted, result);
        }

        [Fact]
        public void DeleteHardFailHardAndSoft()
        {
            var rules = new RuleEngine(new ServiceProviderStub());

            Assert.Throws<InvalidOperationException>(
                () => rules.DeleteHard(new object(), NoOp, () => throw new NotImplementedException())
            );
        }

        [Fact]
        public void DeleteHardSoftFallback()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var obj = new SoftDeletable();
            var callCount = 0;

            var result = rules.DeleteHard(obj, NoOp,
                () => { if(callCount++ > 0) { throw new Exception(); } } // exception on hard delete (the second call).
            );

            Assert.False(obj.Active);
            Assert.Equal(DeleteResult.SoftDeleted, result);
        }

        [Fact]
        public void SoftDeleteDoesntChangeOtherValues()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var obj = new SoftDeletable();
            var original = obj.Unchanged;
            var unruled = obj.UnRuled;

            var result = rules.DeleteSoft(obj, NoOp, NoOp);

            Assert.Equal(original, obj.Unchanged);
            Assert.Equal(unruled, obj.UnRuled);
            Assert.Equal(DeleteResult.SoftDeleted, result);
        }

        private static void NoOp() { }

        private void FakePrepare(ref int stepStamp) => stepStamp = step++;

        private void FakeCommit(ref int stepStamp) => stepStamp = step++;

        private int step = 1;

        public class SoftDeletable {
            [Rules(DeleteValue = false)]
            public bool Active { get; set; } = true;

            [Rules]
            public int Unchanged { get; set; } = 2;

            public int UnRuled { get; set; } = 3;
        }

    }
}
