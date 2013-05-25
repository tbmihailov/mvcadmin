using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRouteUnitTester;

[TestClass]
public class AdminAreaRouteTests
{
    [TestMethod]
    public void TestIncomingRoutes()
    {
        // Arrange
        var tester = new RouteTester<MvcAdminResearch.Areas.MvcAdmin.MvcAdminAreaRegistration>();

        // Assert
        tester.WithIncomingRequest("/MvcAdmin").ShouldMatchRoute("Panel", "Dashboard");
        tester.WithIncomingRequest("/MvcAdmin/Dashboard").ShouldMatchRoute("Panel", "Dashboard");
        tester.WithIncomingRequest("/MvcAdmin/NavMenu").ShouldMatchRoute("Panel", "NavMenu");

        tester.WithIncomingRequest("/MvcAdmin/m/Note").ShouldMatchRoute("Note","Index");
    }

    [TestMethod]
    public void TestOutgoingRoutes()
    {
        // Arrange
        var tester = new RouteTester<MvcAdminResearch.Areas.MvcAdmin.MvcAdminAreaRegistration>();

        // Assert
        tester.WithRouteInfo("Admin", "Foo", "Bar").ShouldGenerateUrl("/Admin/Foo/Bar");
        tester.WithRouteInfo("Admin", "Foo", "Bar", new { id = 5 }).ShouldGenerateUrl("/Admin/Foo/Bar/5");
    }
}