using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using ProcCore.Business.DB0;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Work.UnitTests.Controllers
{
    [TestClass]
    public class IndexControllerTest
    {
        [TestMethod]
        public void 驗證不通過測試()
        {
            //var context = new Mock<HttpContextBase>();
            //var session = new Mock<HttpSessionStateBase>();

            //context.Setup(x => x.Session).Returns(session.Object);
            //session.SetupGet(x => x["apply"]).Returns("54321");


            //IndexController controller = new IndexController();
            //ControllerContext conContext = new ControllerContext(new RequestContext(context.Object, new RouteData()), controller);
            //controller.ControllerContext = conContext;

            //var active_id = 1;

            //string result = controller.aj_MasterInsert(active_id, md) as string;
            //Assert.IsNotNull(result, "傳值發生Null");

            //ResultInfo r = (ResultInfo)JsonConvert.DeserializeObject(result, typeof(ResultInfo));
            //Assert.AreEqual(r.message, "驗證碼不正確");
            //Assert.AreEqual(r.result, false);
        }
    }
}

public static class HttpContextFactory
{
    public static void SetFakeAuthenticatedControllerContext(this Controller controller)
    {
        var httpContext = FakeAuthenticatedHttpContext();
        ControllerContext context = new ControllerContext(new RequestContext(httpContext, new RouteData()), controller);
        controller.ControllerContext = context;
    }
    private static HttpContextBase FakeAuthenticatedHttpContext()
    {
        var context = new Mock<HttpContextBase>();
        var request = new Mock<HttpRequestBase>();
        var response = new Mock<HttpResponseBase>();
        var session = new Mock<HttpSessionStateBase>();
        var server = new Mock<HttpServerUtilityBase>();
        var user = new Mock<IPrincipal>();
        var identity = new Mock<IIdentity>();

        context.Setup(ctx => ctx.Request).Returns(request.Object);
        context.Setup(ctx => ctx.Response).Returns(response.Object);
        context.Setup(ctx => ctx.Session).Returns(session.Object);
        context.Setup(ctx => ctx.Server).Returns(server.Object);
        context.Setup(ctx => ctx.User).Returns(user.Object);
        user.Setup(ctx => ctx.Identity).Returns(identity.Object);
        //identity.Setup(id => id.IsAuthenticated).Returns(true);
        //identity.Setup(id => id.Name).Returns("a.ali174");
        return context.Object;
    }
}