using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using YaleRSS.Controllers;

namespace YaleRss.Test
{
    [TestFixture]
    public class RssControllerTest
    {
        [Test]
        public void Test()
        {


            RssController controller = new RssController();
            var d = controller.Get();
            DataContractSerializer ser = new DataContractSerializer(d.GetType());
            
            Assert.DoesNotThrow(() => ser.WriteObject(new MemoryStream(), d) );
        }
    }
}
