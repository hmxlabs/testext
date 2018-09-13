using NUnit.Framework;

namespace HmxLabs.TestExt
{
    /// <summary>
    /// Helper class used to validate that the equals functions are correctly implemented.
    /// 
    /// This is useful when writing bespoke <code>Equals</code> methods on an object that override the default
    /// behaviour.
    /// </summary>
    public class EqualityAssert
    {
        /// <summary>
        /// Asserts that the two objects provided are equal, however the validation is done such as to ensure
        /// that the equality check is commutative i.e.
        /// one.Equals(two) and two.Equals(one)
        /// are both true.
        /// 
        /// Additionally neither object should be null and a null assertion is performed.
        /// </summary>
        /// <param name="one_">The first object to compare</param>
        /// <param name="two_">The second object to compare</param>
        public static void AreEqual(object one_, object two_)
        {
            Assert.AreEqual(one_, two_);
            Assert.True(one_.Equals(two_));
            Assert.True(two_.Equals(one_));
            
            Assert.AreNotEqual(one_, null);
            Assert.False(one_.Equals(null));
            
            Assert.AreNotEqual(two_, null);
            Assert.False(two_.Equals(null));
        }

        /// <summary>
        /// Similar to the AreEqual method except that it checks for non equality.
        /// </summary>
        /// <param name="one_"></param>
        /// <param name="two_"></param>
        public static void AreNotEqual(object one_, object two_)
        {
            Assert.AreNotEqual(one_, two_);
            Assert.False(one_.Equals(two_));
            Assert.False(two_.Equals(one_));
        }
    }
}
