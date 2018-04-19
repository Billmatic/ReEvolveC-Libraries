using ReEvolveCSharpLibrary.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            AdjGraph<string> graph = new AdjGraph<string>("VP and Ellesemere");
            graph.AppendNodes("VP and Ellesemere", "VP and Eglington");
            graph.AppendNodes("VP and Ellesemere", "Markham and Ellesemere");
            graph.AppendNodes("Markham and Ellesemere", "Markam and Eglinton");
            graph.AppendNodes("VP and Eglington", "Markam and Eglinton");
            graph.AppendNodes("VP and Eglington", "VP and Kingston Rd");
            graph.AppendNodes("VP and Kingston Rd", "Kingston Rd and Eglinton");
            graph.AppendNodes("Kingston Rd and Eglinton", "Markam and Eglinton");

            List<string> list = graph.DepthFirstTraversal();

            Assert.Fail();
        }

        [TestMethod()]
        public void BreadthFirstTraversalTest()
        {
            AdjGraph<string> graph = new AdjGraph<string>("VP and Ellesemere");
            graph.AppendNodes("VP and Ellesemere", "VP and Eglington");
            graph.AppendNodes("VP and Ellesemere", "Markham and Ellesemere");
            graph.AppendNodes("Markham and Ellesemere", "Markam and Eglinton");
            graph.AppendNodes("VP and Eglington", "Markam and Eglinton");
            graph.AppendNodes("VP and Eglington", "VP and Kingston Rd");
            graph.AppendNodes("VP and Kingston Rd", "Kingston Rd and Eglinton");
            graph.AppendNodes("Kingston Rd and Eglinton", "Markam and Eglinton");

            List<string> list = graph.BreadthFirstTraversal();

            Assert.Fail();
        }

        [TestMethod()]
        public void AdjGraphTest()
        {
            AdjGraph<string> graph = new AdjGraph<string>("VP and Ellesemere");

            if (graph.startNode.data != null)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod()]
        public void PathBetweenABExistTest()
        {
            AdjGraph<int> graph = new AdjGraph<int>(1);
            graph.AppendNodes(1, 2);
            graph.AppendNodes(2, 3);
            graph.AppendNodes(3, 4);
            graph.AppendNodes(4, 2);
            graph.AppendNodes(4, 6);
            graph.AppendNodes(6, 1);

            bool result = graph.PathBetweenABExist(1, 5);

            if (result == true)
            {
                Assert.Fail();
            }

            result = graph.PathBetweenABExist(1, 2, Guid.NewGuid());

            if (result == false)
            {
                Assert.Fail();
            }
        }
    }
}
