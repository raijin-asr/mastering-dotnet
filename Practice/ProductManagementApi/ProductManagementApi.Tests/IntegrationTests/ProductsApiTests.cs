using ProductManagementApi.Tests.Factories;
using System.Net;
using System.Net.Http.Json;



namespace ProductManagementApi.Tests.IntegrationTests
{
    public class ProductsApiTests
    {

        //Flow: 1. Test -> 2. HttpClient -> 3. ASP.NET Core test Host -> 4. Authentication middleware -> 5. Authorization middleware -> 6. ProductsController

        #region GET
        //1. AUTHENTICATION Test
        //Test1: unauthenticated test for later use
        [Fact]
        public async Task GetProducts_WithoutAuthentication_ReturnsUnauthorized()
        {
            // Arrange
            using var factory = new CustomWebApplicationFactory(); //create an instance of custom factory
            //factory starts API in a test host

            using var client = factory.CreateClient(); //use factory to create HttpClient
            //CreateClient() gives an HTTP client connected to that test host.

            // Act
            var response = await client.GetAsync("/api/products"); //test service endpoints
            //sends an HTTP GET request to products endpoints

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        //Test2: Testing Authenticated GET endpoints accessible by Customer
        [Fact]
        public async Task GetProducts_WithCustomerAuthentication_ReturnsOk()
        {
            // Arrange
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/products");
            //Here not specified a Test-Role header, the handler creates a Customer.

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        #endregion

        #region POST

        //2. AUTHORIZATION Test
        //Test3: Test Customer cannot access Admin-only endpoint
        [Fact]
        public async Task CreateProduct_WithCustomerAuthentication_ReturnsForbidden()
        {
            // Arrange
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var product = new
            {
                productName = "Test Mobile",
                price = 500,
                stockLevel = 10
            };

            // Act
            var response = await client.PostAsJsonAsync( 
                "/api/products",
                product);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }


        //Test4: Test Admin can access the endpoint specifying Test-Role
        [Fact]
        public async Task CreateProduct_WithAdminAuthentication_ReturnsCreated()
        {
            // Arrange
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            //Tells fake handler to authenticate this request as an Admin.
            client.DefaultRequestHeaders.Add("Test-Role", "Admin");

            var product = new
            {
                productName = "Test Mobile",
                price = 500,
                stockLevel = 10
            };

            // Act
            var response = await client.PostAsJsonAsync(
                "/api/products",
                product);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }


       

        #endregion

    }
}
