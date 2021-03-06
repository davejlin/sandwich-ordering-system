﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SandwichOrderSystemShared.Services;
using System.Collections.Generic;
using System.IO;

namespace SandwichOrderSystemShared.DataAccess.Deserializer.Tests
{
    [TestClass()]
    public class FileSystemManagerTests
    {
        Mock<IDirectoryWrapper> mockDirectory;
        Mock<IErrorHandler> mockErrorHandler;
        IFileSystemManager fileSystemManager;

        [TestInitialize()]
        public void Setup()
        {
            setupMocks();
            fileSystemManager = new FileSystemManager(mockDirectory.Object, mockErrorHandler.Object);
        }

        [TestMethod()]
        public void GetItemNamesTest()
        {
            var item1 = "Sandwich";
            var item2 = "Filling";
            var item3 = "Drink";

            var itemNames = new List<string> { item1, item2, item3 };

            var suffix = ".txt";

            var item1FileName = item1 + suffix;
            var item2FileName = item2 + suffix;
            var item3FileName = item3 + suffix;

            var fileNames = new List<string> { item1FileName, item2FileName, item3FileName };

            mockDirectory.Setup(d => d.GetFiles(It.IsAny<string>())).Returns(fileNames);

            var actualItemNames = fileSystemManager.GetItemNames();

            var i = 0;
            foreach(var item in actualItemNames)
            {
                Assert.AreEqual(itemNames[i++], item, "should be item name");
            }

            Assert.AreEqual(itemNames.Count, i, "should have items");
        }

        [TestMethod()]
        public void GetItemNamesTest_FileNamesWithIncorrectSuffixShouldNotBeReturned()
        {
            var item1 = "Sandwich";
            var item2 = "Filling";
            var item3 = "Drink";

            var itemNames = new List<string> { item1, item2, item3 };

            var suffix = ".txt";

            var item1FileName = item1 + suffix + "t"; // incorrect
            var item2FileName = item2 + suffix;
            var item3FileName = item3 + suffix + "x"; // incorrect

            var fileNames = new List<string> { item1FileName, item2FileName, item3FileName };

            mockDirectory.Setup(d => d.GetFiles(It.IsAny<string>())).Returns(fileNames);

            var actualItemNames = fileSystemManager.GetItemNames();

            var i = 0;
            foreach (var item in actualItemNames)
            {
                Assert.AreEqual(itemNames[1], item, "should be item name");
                i++;
            }

            Assert.AreEqual(1, i, "should have item from correct file name");
        }

        [TestMethod()]
        public void GetItemNamesTest_ReturnsEmptyEnumerableIfDirectoryAlsoReturnsEmptyEnumberable()
        {
            var fileNames = new List<string> { };

            mockDirectory.Setup(d => d.GetFiles(It.IsAny<string>())).Returns(fileNames);

            var actualItemNames = fileSystemManager.GetItemNames();

            Assert.IsNull(actualItemNames.GetEnumerator().Current, "should be empty");
        }

        [TestMethod()]
        public void GetContents()
        {
            var expectedString = "expected string";
            mockDirectory.Setup(d => d.ReadFile(It.IsAny<string>())).Returns(expectedString);

            var actualString = fileSystemManager.GetContents("fileName");

            Assert.AreEqual(expectedString, actualString);
        }

        private void setupMocks()
        {
            mockDirectory = new Mock<IDirectoryWrapper>();
            mockErrorHandler = new Mock<IErrorHandler>();

            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            mockDirectory.Setup(d => d.GetCurrentDirectory()).Returns(directoryInfo);
        }
    }
}