using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            rules.Delete(obj, null);

            Assert.False(obj.Active);
        }

        [Fact]
        public void DeleteHardDeleteBackup()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var obj = new SoftDeletable();
            var deleted = false;

            rules.Delete(new object(), () => deleted = true);

            Assert.True(deleted);
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

            rules.DeleteSoft(obj, NoOp, NoOp);

            Assert.False(obj.Active);
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

            rules.DeleteSoft(new object(), () => executed = true, null);

            Assert.True(executed);
        }

        [Fact]
        public void DeleteSoftFallbackAndCommitExecutes()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var executed = false;
            var committed = false;

            rules.DeleteSoft(new object(), () => executed = true, () => committed = true);

            Assert.True(executed);
            Assert.True(committed);
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

            rules.DeleteHard(new object(), () => FakePrepare(ref prepared), () => FakeCommit(ref committed));

            Assert.Equal(1, prepared);
            Assert.Equal(2, committed);
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

            rules.DeleteHard(obj, NoOp,
                () => { if(obj.Active == true) { throw new Exception(); } } // exception on hard delete, not after soft-delete; mimic EF .SaveChanges().
            );

            Assert.False(obj.Active);
        }

        [Fact]
        public void SoftDeleteDoesntChangeOtherValues()
        {
            var rules = new RuleEngine(new ServiceProviderStub());
            var obj = new SoftDeletable();
            var original = obj.Unchanged;
            var unruled = obj.UnRuled;

            rules.DeleteSoft(obj, NoOp, NoOp);

            Assert.Equal(original, obj.Unchanged);
            Assert.Equal(unruled, obj.UnRuled);
        }

        private static void NoOp() { }

        private void FakePrepare(ref int stepStamp) => stepStamp = step++;

        private void FakeCommit(ref int stepStamp) => stepStamp = step++;

        private int step = 1;

        public class ServiceProviderStub : IServiceProvider {
            public object GetService(Type serviceType)
            {
                throw new NotImplementedException();
            }
        }

        public class SoftDeletable {
            [Rules(DeleteValue = false)]
            public bool Active { get; set; } = true;

            [Rules]
            public int Unchanged { get; set; } = 2;

            public int UnRuled { get; set; } = 3;
        }

    }
}
