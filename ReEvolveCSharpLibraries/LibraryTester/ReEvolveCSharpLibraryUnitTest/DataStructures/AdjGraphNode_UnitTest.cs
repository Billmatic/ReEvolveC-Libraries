using ReEvolveCSharpLibrary.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReEvolveCSharpLibraryUnitTest.DataStructures
{
    /// <summary>
    /// Summary description for AdjGraphNode_UnitTest
    /// </summary>
    [TestClass]
    public class AdjGraphNode_UnitTest
    {
        public AdjGraphNode_UnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod()]
        public void DepthFirstTraversalTest()
        {
            AdjGraphNode<string> node1 = new AdjGraphNode<string>("VP and Ellesemere");
            AdjGraphNode<string> node2 = new AdjGraphNode<string>("Markham and Ellesemere");
            AdjGraphNode<string> node3 = new AdjGraphNode<string>("Markam and Eglinton");
            AdjGraphNode<string> node4 = new AdjGraphNode<string>("VP and Eglington");
            AdjGraphNode<string> node5 = new AdjGraphNode<string>("VP and Kingston Rd");
            AdjGraphNode<string> node6 = new AdjGraphNode<string>("Kingston Rd and Eglinton");

            node1.neighbours.Add(node4);
            node1.neighbours.Add(node2);
            node2.neighbours.Add(node3);
            node4.neighbours.Add(node3);
            node4.neighbours.Add(node5);
            node5.neighbours.Add(node6);
            node6.neighbours.Add(node3);

            node1.DepthFirstTraversal();

            Assert.Fail();
        }

        [TestMethod()]
        public void BreadthFirstTraversalTest()
        {
            AdjGraphNode<string> node1 = new AdjGraphNode<string>("VP and Ellesemere");
            AdjGraphNode<string> node2 = new AdjGraphNode<string>("Markham and Ellesemere");
            AdjGraphNode<string> node3 = new AdjGraphNode<string>("Markam and Eglinton");
            AdjGraphNode<string> node4 = new AdjGraphNode<string>("VP and Eglington");
            AdjGraphNode<string> node5 = new AdjGraphNode<string>("VP and Kingston Rd");
            AdjGraphNode<string> node6 = new AdjGraphNode<string>("Kingston Rd and Eglinton");

            node1.neighbours.Add(node4);
            node1.neighbours.Add(node2);
            node2.neighbours.Add(node3);
            node4.neighbours.Add(node3);
            node4.neighbours.Add(node5);
            node5.neighbours.Add(node6);
            node6.neighbours.Add(node3);

            node1.BreadthFirstTraversal();

            Assert.Fail();
        }

        [TestMethod()]
        public void AdjGraphNodeTest()
        {
            AdjGraphNode<string> node1 = new AdjGraphNode<string>("VP and Ellesemere");
            node1.neighbours.Add(node1);

            if (node1.data != null)
            {
                return;
            }

            Assert.Fail();
        }
    }
}
