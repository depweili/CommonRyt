using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Util;

namespace Common.Web.Tests
{
    /// <summary>
    /// UnitTest3 的摘要说明
    /// </summary>
    [TestClass]
    public class UnitTest3
    {
        public UnitTest3()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
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

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO:  在此处添加测试逻辑
            //
        }

        [TestMethod]
        public void TestEncode()
        {
            string app = "wx845cc082612156f6";

            string encode = Base64DEncrypt.Base64ForUrlEncode(app);

            Console.WriteLine(encode);

            app = "9677320954b4652bd6b868c1a68ec5b9";

            encode = Base64DEncrypt.Base64ForUrlEncode(app);

            Console.WriteLine(encode);

            //string decode = Base64DEncrypt.Base64ForUrlDecode(encode);
        }

        [TestMethod]
        public void TestTripleDESDEncrypt()
        {
            string key = "29ccd152603790114d2c1e02afb87639";//wx845cc082612156f6
            //CJwn/xNScEJiG1yux7kXqBg1Kl0Qnt1zREn2v0RLDdjrckSKKHaM0w==
            string encode = TripleDESDEncrypt.Encrypt(key);

            Console.WriteLine(encode);

            string decode = TripleDESDEncrypt.Decrypt(encode);

            Console.WriteLine(decode);

            //string decode = Base64DEncrypt.Base64ForUrlDecode(encode);
        }


        [TestMethod]
        public void TestHelper()
        {
            

            var data = new string[]{ "123", "234", "13811289537", "8613811289537", "33811289534" };

            foreach (var s in data)
            {
                var res = ValidatorHelper.IsMobile(s);

                Console.WriteLine(s+" : "+res);
            }
        }

    }
}
